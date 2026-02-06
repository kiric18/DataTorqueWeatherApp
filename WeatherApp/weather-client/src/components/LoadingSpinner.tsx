import './LoadingSpinner.css';

export function LoadingSpinner() {
  return (
    <div className="loading-spinner-container">
      <div className="loading-spinner"></div>
      <p className="loading-text">Fetching weather data...</p>
    </div>
  );
}