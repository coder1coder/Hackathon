using System.Threading.Tasks;
using Hackathon.Abstraction.User;
using Hackathon.Common.Models.User;
using Hackathon.Entities;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.DAL.Repositories;

public class EmailConfirmationRepository: IEmailConfirmationRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public EmailConfirmationRepository(ApplicationDbContext dbContext, IMapper mapper)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<EmailConfirmationRequestModel> GetByUserIdAsync(long userId)
    {
        var entity = await _dbContext
            .EmailConfirmations
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.UserId == userId);

        return entity is null
            ? null
            : _mapper.Map<EmailConfirmationRequestEntity, EmailConfirmationRequestModel>(entity);
    }

    public async Task AddAsync(EmailConfirmationRequestParameters parameters)
    {
        var entity = _mapper.Map<EmailConfirmationRequestParameters, EmailConfirmationRequestEntity>(parameters);

        _dbContext.Add(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(EmailConfirmationRequestParameters parameters)
    {
        var entity = await _dbContext.EmailConfirmations
            .FirstOrDefaultAsync(x =>
            x.UserId == parameters.UserId);

        if (entity is not null)
        {
            _mapper.Map(parameters, entity);
            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
