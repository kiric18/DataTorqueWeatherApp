using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Domain.ValueObjects
{
    public readonly record struct Coordinates
    {
        public double Latitude { get; }
        public double Longitude { get; }

        private Coordinates(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public static Result<Coordinates> Create(double latitude, double longitude)
        {
            if (latitude < -90 || latitude > 90)
            {
                return Result<Coordinates>.Failure("Latitude must be between -90 and 90");
            }

            if (longitude < -180 || longitude > 180)
            {
                return Result<Coordinates>.Failure("Longitude must be between -180 and 180");
            }

            return Result<Coordinates>.Success(new Coordinates(latitude, longitude));
        }
    }
}
