from flask import Flask, request, jsonify
from keras.models import load_model, Model
from keras.preprocessing import image
import numpy as np
import os
import cv2
import uuid
import pandas as pd
import matplotlib.cm as cm
from PIL import Image
import tensorflow as tf
from sklearn.neighbors import NearestNeighbors

app = Flask(__name__)

MODEL_PATH = "./ai_model/densenetpneumogradcammodel.h5"

# Automatically detect the base path (up to wwwroot)
BASE_DIR = os.path.abspath(os.path.join(os.path.dirname(__file__), '..', 'wwwroot'))
DOTNET_UPLOADS_PATH = os.path.join(BASE_DIR, 'uploads', 'xray-images')
os.makedirs(DOTNET_UPLOADS_PATH, exist_ok=True)

# Load Model
model = load_model(MODEL_PATH, compile=False)
class_names = ["Normal", "Pneumonia"]

# Load and preprocess hospital data
CSV_PATH = os.path.join(os.path.dirname(__file__), 'ai_model', 'hospitals_nepal_pneumonia.csv')
df = pd.read_csv(CSV_PATH)
df.columns = df.columns.str.strip()
df = df.dropna(subset=['Province', 'Hospital Name', 'Pneumonia Treatment', 'Latitude', 'Longitude'])
df = df[df['Latitude'].between(26, 30) & df['Longitude'].between(80, 88)]
df['Latitude'] = df['Latitude'].astype(float)
df['Longitude'] = df['Longitude'].astype(float)
df['Pneumonia Treatment'] = df['Pneumonia Treatment'].astype(int)
df = df.drop_duplicates(subset=['Hospital Name', 'Latitude', 'Longitude'])
hospital_df = df[df['Pneumonia Treatment'] == 1].reset_index(drop=True)
hospital_coords = hospital_df[['Latitude', 'Longitude']].values

knn = NearestNeighbors(n_neighbors=5, algorithm='ball_tree')
knn.fit(hospital_coords)

def preprocess(img_path):
    img = image.load_img(img_path, target_size=(224, 224))
    img_array = image.img_to_array(img)
    img_array = np.expand_dims(img_array, axis=0)
    img_array /= 255.0
    return img_array, img

def generate_gradcam(model, img_array, class_index, last_conv_layer="conv5_block3_2_conv"):
    grad_model = Model(inputs=model.input, outputs=[model.get_layer(last_conv_layer).output, model.output])
    with tf.GradientTape() as tape:
        conv_outputs, predictions = grad_model(img_array)
        loss = predictions[:, class_index] if predictions.shape[1] != 1 else (
            predictions[:, 0] if class_index == 1 else 1 - predictions[:, 0]
        )
    grads = tape.gradient(loss, conv_outputs)
    pooled_grads = tf.reduce_mean(grads, axis=(0, 1, 2))
    conv_outputs = conv_outputs[0]
    heatmap = np.mean(conv_outputs * pooled_grads, axis=-1)
    heatmap = np.maximum(heatmap, 0)
    heatmap /= np.max(heatmap)
    return heatmap

def overlay_heatmap(heatmap, original_img):
    heatmap = cv2.resize(heatmap, (original_img.size[0], original_img.size[1]))
    heatmap_colored = cm.jet(heatmap)[:, :, :3]
    heatmap_colored = np.uint8(255 * heatmap_colored)
    if len(np.array(original_img).shape) == 2:
        original_img = cv2.cvtColor(np.array(original_img), cv2.COLOR_GRAY2RGB)
    overlayed_img = cv2.addWeighted(np.array(original_img), 0.6, heatmap_colored, 0.4, 0)
    return overlayed_img

def haversine_distance(lat1, lon1, lat2, lon2):
    R = 6371
    dlat = np.radians(lat2 - lat1)
    dlon = np.radians(lon2 - lon1)
    a = np.sin(dlat/2)**2 + np.cos(np.radians(lat1)) * np.cos(np.radians(lat2)) * np.sin(dlon/2)**2
    c = 2 * np.arctan2(np.sqrt(a), np.sqrt(1-a))
    return R * c
# (same imports and initial setup)
@app.route('/predict', methods=['POST'])
def predict():
    if 'xray' not in request.files:
        return jsonify({'error': 'No image'}), 400

    user_lat = request.form.get('latitude', type=float)
    user_lon = request.form.get('longitude', type=float)

    image_file = request.files['xray']
    filename = f"{uuid.uuid4().hex}.png"
    image_path = os.path.join(DOTNET_UPLOADS_PATH, filename)
    image_file.save(image_path)

    img_array, original_img = preprocess(image_path)
    prediction = model.predict(img_array)[0][0]
    result = "Pneumonia Detected" if prediction > 0.5 else "Normal"
    class_index = 1 if result == "Pneumonia Detected" else 0

    heatmap = generate_gradcam(model, img_array, class_index)
    overlayed_img = overlay_heatmap(heatmap, original_img)
    gradcam_filename = f"gradcam_{uuid.uuid4().hex}.jpg"
    gradcam_path = os.path.join(DOTNET_UPLOADS_PATH, gradcam_filename)
    cv2.imwrite(gradcam_path, overlayed_img)
    relative_gradcam_url = f"/uploads/xray-images/{gradcam_filename}"

    recommendations = []

    if result == "Pneumonia Detected":
        if user_lat is not None and user_lon is not None:
            if not (26 <= user_lat <= 30 and 80 <= user_lon <= 88):
                recommendations = ["Invalid latitude/longitude for Nepal."]
            else:
                user_coords = np.array([[user_lat, user_lon]])
                distances, indices = knn.kneighbors(user_coords)
                for idx in indices[0]:
                    row = hospital_df.iloc[idx]
                    distance_km = haversine_distance(user_lat, user_lon, row['Latitude'], row['Longitude'])
                    recommendations.append({
                        "hospital": row['Hospital Name'],
                        "province": row['Province'],
                        "latitude": row['Latitude'],
                        "longitude": row['Longitude'],
                        "distance_km": round(distance_km, 2)
                    })
        else:
            recommendations = ["No recommendation (location not provided)."]
    else:
        recommendations = ["No recommendation"]

    return jsonify({
        "result": result,
        "gradCamUrl": relative_gradcam_url,
        "recommendations": recommendations
    })
if __name__ == '__main__':
    app.run(debug=True,  port=5000)
