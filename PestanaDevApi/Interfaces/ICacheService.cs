namespace PestanaDevApi.Interfaces
{
    public interface ICacheService
    {
        /// <summary>
        /// Retrieves a value from the cache using the specified key.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the cached value.
        /// </typeparam>
        /// <param name="key">
        /// The key associated with the cached value.
        /// </param>
        /// <returns>
        /// The cached value if it exists; otherwise, <see langword="null"/>.
        /// </returns>

        T? Get<T>(string key);
        /// <summary>
        /// Stores a value in the cache with the specified key and expiration time.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value to cache.
        /// </typeparam>
        /// <param name="key">
        /// The key used to store the cached value.
        /// </param>
        /// <param name="value">
        /// The value to be stored in the cache.
        /// </param>
        /// <param name="expiration">
        /// The optional cache expiration time. If not provided,
        /// the value expires after 24 hours.
        /// </param>
        void Set<T>(string key, T value, TimeSpan? expiration = null);

        /// <summary>
        /// Removes a cached value associated with the specified key.
        /// </summary>
        /// <param name="key">
        /// The key of the cached value to remove.
        /// </param>
        void Remove(string key);
    }
}
