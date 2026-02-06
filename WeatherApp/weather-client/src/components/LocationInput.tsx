import { useState } from 'react';
import { Coordinates, LocationPreset } from '../types/weather';
import './LocationInput.css';

const PRESET_LOCATIONS: LocationPreset[] = [
  { name: 'Wellington CBD', coordinates: { latitude: -41.2865, longitude: 174.7762 } },
  { name: 'Wellington Airport', coordinates: { latitude: -41.3272, longitude: 174.8053 } },
  { name: 'Lower Hutt', coordinates: { latitude: -41.2127, longitude: 174.8970 } },
  { name: 'Porirua', coordinates: { latitude: -41.1337, longitude: 174.8408 } },
  { name: 'Petone', coordinates: { latitude: -41.2270, longitude: 174.8712 } },
];

interface LocationInputProps {
  onSearch: (coordinates: Coordinates) => void;
  isLoading: boolean;
}

export function LocationInput({ onSearch, isLoading }: LocationInputProps) {
  const [latitude, setLatitude] = useState<string>('-41.2865');
  const [longitude, setLongitude] = useState<string>('174.7762');
  const [selectedPreset, setSelectedPreset] = useState<string>('Wellington CBD');

  const handlePresetChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const presetName = e.target.value;
    setSelectedPreset(presetName);

    if (presetName === 'custom') {
      return;
    }

    const preset = PRESET_LOCATIONS.find(p => p.name === presetName);
    if (preset) {
      setLatitude(preset.coordinates.latitude.toString());
      setLongitude(preset.coordinates.longitude.toString());
    }
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    const lat = parseFloat(latitude);
    const lon = parseFloat(longitude);

    if (isNaN(lat) || isNaN(lon)) {
      return;
    }

    onSearch({ latitude: lat, longitude: lon });
  };

  const handleUseCurrentLocation = () => {
    if ('geolocation' in navigator) {
      navigator.geolocation.getCurrentPosition(
        (position) => {
          setLatitude(position.coords.latitude.toFixed(4));
          setLongitude(position.coords.longitude.toFixed(4));
          setSelectedPreset('custom');
        },
        (error) => {
          console.error('Geolocation error:', error);
          alert('Unable to get your location. Please enter coordinates manually.');
        }
      );
    } else {
      alert('Geolocation is not supported by your browser.');
    }
  };

  return (
    <form className="location-input" onSubmit={handleSubmit}>
      <div className="preset-selector">
        <label htmlFor="preset">Location</label>
        <select
          id="preset"
          value={selectedPreset}
          onChange={handlePresetChange}
          disabled={isLoading}
        >
          {PRESET_LOCATIONS.map(preset => (
            <option key={preset.name} value={preset.name}>
              {preset.name}
            </option>
          ))}
          <option value="custom">Custom Coordinates</option>
        </select>
      </div>

      <div className="coordinates-row">
        <div className="coordinate-input">
          <label htmlFor="latitude">Latitude</label>
          <input
            id="latitude"
            type="number"
            step="any"
            min="-90"
            max="90"
            value={latitude}
            onChange={(e) => {
              setLatitude(e.target.value);
              setSelectedPreset('custom');
            }}
            disabled={isLoading}
            placeholder="-41.2865"
          />
        </div>

        <div className="coordinate-input">
          <label htmlFor="longitude">Longitude</label>
          <input
            id="longitude"
            type="number"
            step="any"
            min="-180"
            max="180"
            value={longitude}
            onChange={(e) => {
              setLongitude(e.target.value);
              setSelectedPreset('custom');
            }}
            disabled={isLoading}
            placeholder="174.7762"
          />
        </div>
      </div>

      <div className="button-row">
        <button
          type="button"
          className="location-button"
          onClick={handleUseCurrentLocation}
          disabled={isLoading}
        >
          📍 Use My Location
        </button>

        <button
          type="submit"
          className="search-button"
          disabled={isLoading}
        >
          {isLoading ? 'Loading...' : 'Get Weather'}
        </button>
      </div>
    </form>
  );
}