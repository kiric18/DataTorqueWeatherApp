using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Domain.ValueObjects
{
    public readonly record struct Temperature
    {
        public double Celsius { get; }

        private Temperature(double celsius)
        {
            Celsius = celsius;
        }

        public static Temperature FromCelsius(double celsius) => new(celsius);

        public static Temperature FromKelvin(double kelvin) => new(kelvin - 273.15);

        public bool IsAbove(double threshold) => Celsius > threshold;

        public bool IsBelow(double threshold) => Celsius < threshold;

        public double Rounded(int decimals = 1) => Math.Round(Celsius, decimals);
    }
}
