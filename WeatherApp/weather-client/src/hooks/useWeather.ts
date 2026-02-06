import { useState, useCallback, useRef } from 'react';
import { WeatherResponse, Coordinates } from '../types/weather';
import { fetchWeather, WeatherServiceError } from '../services/weatherService';

interface UseWeatherResult {
    weather: WeatherResponse | null;
    isLoading: boolean;
    error: string | null;
    getWeather: (coordinates: Coordinates) => Promise<void>;
    clearError: () => void;
}

export function useWeather(): UseWeatherResult {
    const [weather, setWeather] = useState<WeatherResponse | null>(null);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const fetchingRef = useRef(false);

    const getWeather = useCallback(async (coordinates: Coordinates) => {
        // Prevent duplicate calls
        if (fetchingRef.current) {
            return;
        }

        fetchingRef.current = true;
        setIsLoading(true);
        setError(null);

        try {
            const data = await fetchWeather(coordinates);
            setWeather(data);
        } catch (err) {
            if (err instanceof WeatherServiceError) {
                setError(err.message);
            } else {
                setError('An unexpected error occurred. Please try again.');
            }
            setWeather(null);
        } finally {
            setIsLoading(false);
            fetchingRef.current = false;
        }
    }, []);

    const clearError = useCallback(() => {
        setError(null);
    }, []);

    return { weather, isLoading, error, getWeather, clearError };
}