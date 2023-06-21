using StackExchange.Redis;

namespace FreeCourse.Services.Basket.Services
{
    public class RedisService
    {
        private readonly string _host;
        private readonly string _port;
        private ConnectionMultiplexer _ConnectionMultiplexer;

        public RedisService(string host, string port)
        {
            _port = port;
            _host = host;
        }

        public void Connect() => _ConnectionMultiplexer = ConnectionMultiplexer.Connect($"{_host}:{_port}");

        public IDatabase GetDB(int db = 1) => _ConnectionMultiplexer.GetDatabase(db);

    }
}
