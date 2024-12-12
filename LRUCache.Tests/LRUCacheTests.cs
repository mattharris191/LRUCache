namespace LRUCache.Tests
{
    public class LRUCacheTests
    {
        [Fact]
        public void Get_Integer_ReturnsCorrectValue()
        {
            var cache = new LRUCache<int>(2);

            cache.Add(1, 2);

            Assert.Equal(2, cache.Get(1));
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(2, 5)]
        [InlineData(3, 8)]
        public void Get_IntegerFromMultiple_ReturnsCorrectValue(int key, int expected)
        {
            var cache = new LRUCache<int>(3);

            cache.Add(1, 2);
            cache.Add(2, 5);
            cache.Add(3, 8);

            Assert.Equal(expected, cache.Get(key));
        }

        [Fact]
        public void Get_UpdatedInteger_ReturnsCorrectValue()
        {
            var cache = new LRUCache<int>(2);

            cache.Add(1, 1);
            cache.Add(1, 2);

            Assert.Equal(2, cache.Get(1));
        }

        [Fact]
        public void Get_UpdatedIntegerAfterEviction_ReturnsCorrectValue()
        {
            var cache = new LRUCache<int>(2);

            cache.Add(1, 1);
            cache.Add(2, 2);
            cache.Add(3, 3);
            cache.Add(1, 2);

            Assert.Equal(2, cache.Get(1));
        }

        [Fact]
        public void Get_String_ReturnsCorrectValue()
        {
            var cache = new LRUCache<string>(2);

            cache.Add(1, "0");
            
            Assert.Equal("0", cache.Get(1));
        }

        [Fact]
        public void Get_Array_ReturnsCorrectValue()
        {
            var cache = new LRUCache<string[]>(2);

            cache.Add(1, ["0", "1"]);

            Assert.Equal(["0", "1"], cache.Get(1));
        }

        [Fact]
        public void Get_WithoutAdding_KeyNotFound()
        {
            var cache = new LRUCache<string>(2);

            Assert.Throws<KeyNotFoundException>(() => cache.Get(1));
        }

        [Fact]
        public void Get_IncorrectKey_KeyNotFound()
        {
            var cache = new LRUCache<string>(2);

            cache.Add(1, "0");

            Assert.Throws<KeyNotFoundException>(() => cache.Get(2));
        }

        [Fact]
        public void Get_EvictedKey_KeyNotFound()
        {
            var cache = new LRUCache<string>(2);

            cache.Add(1, "0");
            cache.Add(2, "0");
            cache.Add(3, "0");

            Assert.Throws<KeyNotFoundException>(() => cache.Get(1));
        }

        [Fact]
        public void Get_ZeroCapacity_KeyNotFound()
        {
            var cache = new LRUCache<string>(0);

            cache.Add(1, "0");

            Assert.Throws<KeyNotFoundException>(() => cache.Get(1));
        }

        [Fact]
        public void Add_WithCallback_ShouldReturnEvicted()
        {
            KeyValuePair<int, string>? evicted = null;
            var cache = new LRUCache<string>(2, (KeyValuePair<int, string> result) => { evicted = result; });

            cache.Add(1, "1");
            cache.Add(2, "2");
            cache.Add(3, "3");

            Assert.Equal(new KeyValuePair<int, string>(1, "1"), evicted);
        }

    }
}
