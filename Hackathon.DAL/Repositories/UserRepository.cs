using System.Linq;
using System.Threading.Tasks;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Entities;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.User;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.DAL.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(
            IMapper mapper,
            ApplicationDbContext dbContext
            )
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<long> CreateAsync(SignUpModel signUpModel)
        {
            var entity = _mapper.Map<UserEntity>(signUpModel);

            await _dbContext.Users.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<UserModel> GetAsync(long userId)
        {
            var entity = await _dbContext.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == userId);

            return _mapper.Map<UserModel>(entity);
        }

        public async Task<BaseCollectionModel<UserModel>> GetAsync(GetFilterModel<UserFilterModel> getFilterModel)
        {
            var query = _dbContext.Users.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(getFilterModel.Filter.Username))
                query = query.Where(x=>x.UserName.ToLower() == getFilterModel.Filter.Username.ToLower());

            if (!string.IsNullOrWhiteSpace(getFilterModel.Filter.Email))
                query = query.Where(x=>x.Email.ToLower() == getFilterModel.Filter.Email.ToLower());

            var totalCount = await query.LongCountAsync();

            if (!string.IsNullOrWhiteSpace(getFilterModel.SortBy))
            {
                query = getFilterModel.SortBy switch
                {
                    nameof(UserEntity.UserName) => getFilterModel.SortOrder == SortOrder.Asc
                        ? query.OrderBy(x => x.UserName)
                        : query.OrderByDescending(x => x.UserName),

                    nameof(UserEntity.Email) => getFilterModel.SortOrder == SortOrder.Asc
                        ? query.OrderBy(x => x.Email)
                        : query.OrderByDescending(x => x.Email),

                    _ => getFilterModel.SortOrder == SortOrder.Asc
                        ? query.OrderBy(x => x.Id)
                        : query.OrderByDescending(x => x.Id)
                };
            }

            var page = getFilterModel.Page ?? 1;
            var pageSize = getFilterModel.PageSize ?? 1000;

            return new BaseCollectionModel<UserModel>
            {
                Items = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ProjectToType<UserModel>()
                    .ToListAsync(),

                TotalCount = totalCount
            };
        }

        public async Task<bool> ExistAsync(long userId)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .AnyAsync(x => x.Id == userId);
        }
    }
}