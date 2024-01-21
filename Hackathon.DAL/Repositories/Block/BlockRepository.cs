using Hackathon.Common.Abstraction.Block;
using Hackathon.Common.Models.Block;
using Hackathon.DAL.Entities.Block;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Hackathon.DAL.Repositories.Block;

public class BlockRepository : IBlockingRepository
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _dbContext;

    public BlockRepository(
        IMapper mapper,
        ApplicationDbContext dbContext
    )
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<long> CreateAsync(BlockingModel blocking)
    {
        var blockingEntity = _mapper.Map<BlockingEntity>(blocking);

        await _dbContext.AddAsync(blockingEntity);
        await _dbContext.SaveChangesAsync();

        return blockingEntity.Id;
    }

    public async Task RemoveAsync(BlockingModel blocking)
    {
        if (blocking == null)
            return;

        var blockingEntity = await _dbContext.Blockings
            .FirstOrDefaultAsync(x => x.Id == blocking.Id);

        if (blockingEntity != null )
        {
            _dbContext.Remove(blockingEntity);
            _dbContext.SaveChanges();
        }
    }
}
