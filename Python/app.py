from flask import Flask, request, jsonify
from keras.models import load_model, Model
from keras.preprocessing import image
import numpy as np
import os
import cv2
import uuid
import matplotlib.cm as cm
from PIL import Image
import tensorflow as tf

app = Flask(__name__)
MODEL_PATH = "./ai_model/densenetpneumogradcammodel.h5"
model = load_model(MODEL_PATH, compile=False)
class_names = ["Normal", "Pneumonia"]

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

@app.route('/predict', methods=['POST'])
def predict():
    if 'xray' not in request.files:
        return jsonify({'error': 'No image'}), 400

    image_file = request.files['xray']
    image_path = f"./uploads/{uuid.uuid4().hex}.png"
    image_file.save(image_path)

    img_array, original_img = preprocess(image_path)
    prediction = model.predict(img_array)[0][0]
    result = "Pneumonia Detected" if prediction > 0.5 else "Normal"
    class_index = 1 if result == "Pneumonia Detected" else 0

    heatmap = generate_gradcam(model, img_array, class_index)
    overlayed_img = overlay_heatmap(heatmap, original_img)
    gradcam_path = f"./uploads/gradcam_{uuid.uuid4().hex}.jpg"
    cv2.imwrite(gradcam_path, overlayed_img)

    return jsonify({
        "result": result,
        "gradCamUrl": f"http://localhost:5000/{gradcam_path[2:]}"  # expose via nginx/public folder
    })

if __name__ == '__main__':
    app.run(debug=True, port=5000)
