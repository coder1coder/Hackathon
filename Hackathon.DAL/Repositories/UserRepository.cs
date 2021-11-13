using System;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.Common;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Entities;
using Hackathon.Common.Models.User;
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

        public async Task<UserModel> GetAsync(string userName)
        {
            var entity = await _dbContext.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.UserName == userName);

            return _mapper.Map<UserModel>(entity);
        }

        public async Task<bool> CanCreateAsync(SignUpModel signUpModel)
        {
            var query = _dbContext.Users
                .AsNoTracking()
                .Where(x => x.UserName.ToLower() == signUpModel.UserName.ToLower());

            if (signUpModel.Email != null)
                query = query.Where(x => x.Email.ToLower() == signUpModel.Email.ToLower());

            var user = await query.FirstOrDefaultAsync();

            return user == null;
        }
    }
}