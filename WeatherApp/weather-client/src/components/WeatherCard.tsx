import { WeatherResponse, WeatherCondition } from '../types/weather';
import './WeatherCard.css';

interface WeatherCardProps {
  weather: WeatherResponse;
}

const WEATHER_ICONS: Record<WeatherCondition, string> = {
  Sunny: '☀️',
  Windy: '💨',
  Rainy: '🌧️',
  Snowing: '❄️',
};

const WEATHER_BACKGROUNDS: Record<WeatherCondition, string> = {
  Sunny: 'linear-gradient(135deg, #fbbf24 0%, #f59e0b 100%)',
  Windy: 'linear-gradient(135deg, #94a3b8 0%, #64748b 100%)',
  Rainy: 'linear-gradient(135deg, #60a5fa 0%, #3b82f6 100%)',
  Snowing: 'linear-gradient(135deg, #e2e8f0 0%, #cbd5e1 100%)',
};

export function WeatherCard({ weather }: WeatherCardProps) {
  const icon = WEATHER_ICONS[weather.condition];
  const background = WEATHER_BACKGROUNDS[weather.condition];

  return (
    <div className="weather-card">
      <div
        className="weather-header"
        style={{ background }}
      >
        <span className="weather-icon">{icon}</span>
        <span className="weather-condition">{weather.condition}</span>
      </div>

      <div className="weather-details">
        <div className="weather-stat">
          <span className="stat-icon">🌡️</span>
          <div className="stat-content">
            <span className="stat-value">{weather.temperatureCelsius}°C</span>
            <span className="stat-label">Temperature</span>
          </div>
        </div>

        <div className="weather-stat">
          <span className="stat-icon">💨</span>
          <div className="stat-content">
            <span className="stat-value">{weather.windSpeedKph} km/h</span>
            <span className="stat-label">Wind Speed</span>
          </div>
        </div>
      </div>

      <div className="weather-recommendation">
        <span className="recommendation-icon">👔</span>
        <p className="recommendation-text">{weather.recommendation}</p>
      </div>
    </div>
  );
}