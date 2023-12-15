using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace OnlineStore.Services
{
	public class CacheService : ICacheService
	{
		private readonly IDatabase _db;
		private readonly IConfiguration _configuration;
        private readonly ILogger<CacheService> _logger;

		public CacheService(IConfiguration configuration, ILogger<CacheService> logger)
		{
            _logger = logger;
			_configuration = configuration;
            _db = ConnectionMultiplexer.Connect(_configuration["RedisUrl"]).GetDatabase();
        }

        public T? GetData<T>(string key)
        {
            try
            {
                var value = _db.StringGet(key);
                if (!string.IsNullOrEmpty(value))
                {
                    return DeserializeGetObject<T>(value);
                }
                return default;
            }
            catch(Exception ex)
            {
                _logger.LogError($"exception occured in GetData From Redis: {ex.Message}");
                return default;
            }
        }

        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            try
            {
                TimeSpan expiryTime = CalculateExpireTime(expirationTime);

                var val = SerializeGetObject(value);
                var isSet = _db.StringSet(key, val, expiryTime);
                return isSet;
            }
            catch(Exception ex)
            {
                _logger.LogError($"exception occured in SetData To Redis: {ex.Message}");
                return false;
            }
        }


        public object RemoveData(string key)
        {
            try
            {
                bool _isKeyExist = _db.KeyExists(key);
                if (_isKeyExist == true)
                {
                    return _db.KeyDelete(key);
                }
                return false;
            }
            catch(Exception ex)
            {
                _logger.LogError($"exception occured in RemoveData From Redis: {ex.Message}");
                return false;
            }
        }

        private static TimeSpan CalculateExpireTime(DateTimeOffset expirationTime)
        {
            TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);

            return expiryTime;
        }

        private static T? DeserializeGetObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        private static string? SerializeGetObject<T>(T value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }
}

