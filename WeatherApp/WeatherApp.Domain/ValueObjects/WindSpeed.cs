using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Domain.ValueObjects
{
    public readonly record struct WindSpeed
    {
        public double KilometersPerHour { get; }

        private WindSpeed(double kph)
        {
            KilometersPerHour = kph;
        }

        public static WindSpeed FromKph(double kph) => new(kph);

        public static WindSpeed FromMetersPerSecond(double mps) => new(mps * 3.6);

        public bool IsWindy(double threshold = 30) => KilometersPerHour >= threshold;

        public double Rounded(int decimals = 1) => Math.Round(KilometersPerHour, decimals);
    }
}
