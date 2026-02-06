using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Application.Interfaces;

namespace WeatherApp.Application.Services
{
    public class RequestCounter : IRequestCounter
    {
        private int _count;
        private readonly object _lock = new();

        public int IncrementAndGet()
        {
            lock (_lock)
            {
                return ++_count;
            }
        }

        public void Reset()
        {
            lock (_lock)
            {
                _count = 0;
            }
        }
    }
}
