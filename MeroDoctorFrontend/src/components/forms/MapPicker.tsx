import { MapContainer, Marker, TileLayer, useMapEvents } from "react-leaflet";
import { useState } from "react";
import L from "leaflet";

const markerIcon = new L.Icon({
  iconUrl: "https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon.png",
  shadowUrl:
    "https://unpkg.com/leaflet@1.9.4/dist/images/marker-shadow.png",
  iconSize: [25, 41],
  iconAnchor: [12, 41]
});

type Props = {
  onLocationSelect: (
    latitude: number,
    longitude: number
  ) => void;
};

function LocationMarker({
  onLocationSelect
}: Props) {
  const [position, setPosition] = useState<
    [number, number] | null
  >(null);

  useMapEvents({
    click(e) {
      const lat = e.latlng.lat;
      const lng = e.latlng.lng;

      setPosition([lat, lng]);
      onLocationSelect(lat, lng);
    }
  });

  return position ? (
    <Marker
      position={position}
      icon={markerIcon}
    />
  ) : null;
}

export default function MapPicker({
  onLocationSelect
}: Props) {
  return (
    <MapContainer
      center={[27.7172, 85.324]}
      zoom={13}
      style={{
        height: "300px",
        width: "100%"
      }}
    >
      <TileLayer url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png" />

      <LocationMarker
        onLocationSelect={onLocationSelect}
      />
    </MapContainer>
  );
}