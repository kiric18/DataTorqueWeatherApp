export interface WeatherResponse {
    temperatureCelsius: number;
    windSpeedKph: number;
    condition: WeatherCondition;
    recommendation: string;
}

export type WeatherCondition = 'Sunny' | 'Windy' | 'Rainy' | 'Snowing';

export interface Coordinates {
    latitude: number;
    longitude: number;
}

export interface LocationPreset {
    name: string;
    coordinates: Coordinates;
}