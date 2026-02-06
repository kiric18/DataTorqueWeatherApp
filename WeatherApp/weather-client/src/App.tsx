import React, { useEffect, useRef } from 'react';
import { useWeather } from './hooks/useWeather';
import { LocationInput } from './components/LocationInput';
import { WeatherCard } from './components/WeatherCard';
import { LoadingSpinner } from './components/LoadingSpinner';
import { ErrorMessage } from './components/ErrorMessage';
import { Coordinates } from './types/weather';

function App() {
    const { weather, isLoading, error, getWeather, clearError } = useWeather();
    const initialLoadRef = useRef(false);

    // Load Wellington weather on initial mount only once
    useEffect(() => {
        if (initialLoadRef.current) {
            return;
        }
        initialLoadRef.current = true;
        getWeather({ latitude: -41.2865, longitude: 174.7762 });
    }, [getWeather]);

    const handleSearch = (coordinates: Coordinates) => {
        clearError();
        getWeather(coordinates);
    };

    const handleRetry = () => {
        getWeather({ latitude: -41.2865, longitude: 174.7762 });
    };

    return (
        <div className="app">
            <header className="app-header">
                <h1>🌤️ Wellington Weather</h1>
                <p className="app-subtitle">Check the weather before heading to the office</p>
            </header>

            <main className="app-content">
                <LocationInput onSearch={handleSearch} isLoading={isLoading} />

                {isLoading && <LoadingSpinner />}

                {error && !isLoading && (
                    <ErrorMessage message={error} onRetry={handleRetry} />
                )}

                {weather && !isLoading && !error && (
                    <WeatherCard weather={weather} />
                )}
            </main>

            <footer className="app-footer">
                <p>Data provided by OpenWeatherMap • Built with React + .NET 8</p>
            </footer>
        </div>
    );
}

export default App;