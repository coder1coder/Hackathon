using Hackathon.Common.Abstraction.Block;
using Hackathon.Common.Models.Block;
using Hackathon.DAL.Entities.Block;
using Hackathon.DAL.Entities.Event;
using MapsterMapper;
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

    public async Task<long> CreateAsync(BlockingModel block)
    {
        var blockEntity = _mapper.Map<BlockingEntity>(block);

        await _dbContext.AddAsync(blockEntity);
        await _dbContext.SaveChangesAsync();

        return blockEntity.Id;
    }
}
