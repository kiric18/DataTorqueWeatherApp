using WeatherApp.Application.Services;
using Xunit;

namespace WeatherApp.Application.Tests.Services
{
    public class RequestCounterTests
    {
        [Fact]
        public void IncrementAndGet_ReturnsSequentialValues()
        {
            // Arrange
            var counter = new RequestCounter();

            // Act & Assert
            Assert.Equal(1, counter.IncrementAndGet());
            Assert.Equal(2, counter.IncrementAndGet());
            Assert.Equal(3, counter.IncrementAndGet());
        }

        [Fact]
        public void Reset_ResetsCounter()
        {
            // Arrange
            var counter = new RequestCounter();
            counter.IncrementAndGet();
            counter.IncrementAndGet();

            // Act
            counter.Reset();

            // Assert
            Assert.Equal(1, counter.IncrementAndGet());
        }

        [Fact]
        public void IncrementAndGet_IsThreadSafe()
        {
            // Arrange
            var counter = new RequestCounter();
            var tasks = new List<Task>();
            var results = new System.Collections.Concurrent.ConcurrentBag<int>();

            // Act - Run 100 increments concurrently
            for (int i = 0; i < 100; i++)
            {
                tasks.Add(Task.Run(() => results.Add(counter.IncrementAndGet())));
            }
            Task.WaitAll(tasks.ToArray());

            // Assert - Should have 100 unique values from 1-100
            var sorted = results.OrderBy(x => x).ToList();
            Assert.Equal(100, sorted.Count);
            Assert.Equal(Enumerable.Range(1, 100), sorted);
        }
    }
}
