using StackExchange.Redis;

namespace Hackathon.DAL.Repositories;

public abstract class RedisRepository
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    protected IDatabase RedisDatabase => _connectionMultiplexer.GetDatabase();

    protected RedisRepository(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }
}