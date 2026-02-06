import { WeatherResponse, Coordinates } from '../types/weather';

// Match your API's actual port
const API_BASE_URL = 'http://localhost:5099';

export class WeatherServiceError extends Error {
    constructor(
        message: string,
        public statusCode: number
    ) {
        super(message);
        this.name = 'WeatherServiceError';
    }
}

export async function fetchWeather(coordinates: Coordinates): Promise<WeatherResponse> {
    const { latitude, longitude } = coordinates;
    const url = `${API_BASE_URL}/weather?latitude=${latitude}&longitude=${longitude}`;

    try {
        const response = await fetch(url);

        if (!response.ok) {
            if (response.status === 503) {
                throw new WeatherServiceError(
                    'Weather service is temporarily unavailable. Please try again.',
                    503
                );
            }
            if (response.status === 400) {
                const error = await response.json();
                throw new WeatherServiceError(error.error || 'Invalid coordinates', 400);
            }
            throw new WeatherServiceError('Failed to fetch weather data', response.status);
        }

        return response.json();
    } catch (error) {
        if (error instanceof WeatherServiceError) {
            throw error;
        }
        console.error('Fetch error:', error);
        throw new WeatherServiceError('Unable to connect to weather service. Is the API running?', 0);
    }
}