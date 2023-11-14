using System.Threading.Tasks;
using Hackathon.Informing.Abstractions.Models.Templates;
using Hackathon.Informing.Abstractions.Repositories;
using Hackathon.Informing.DAL.Entities;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Informing.DAL.Repositories;

public class TemplateRepository: ITemplateRepository
{
    private readonly InformingDbContext _dbContext;
    private readonly IMapper _mapper;

    public TemplateRepository(
        InformingDbContext dbContext, 
        IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<TemplateModel> GetAsync(string templateId)
    {
        var templateEntity = await _dbContext.Templates
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.Id == templateId);

        return _mapper.Map<TemplateEntity, TemplateModel>(templateEntity);
    }
}
