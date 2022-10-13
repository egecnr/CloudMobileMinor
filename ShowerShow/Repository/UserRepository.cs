using AutoMapper;
using ShowerShow.DAL;
using ShowerShow.DTO;
using ShowerShow.Models;
using ShowerShow.Repository.Interface;
using ShowerShow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Repository
{
    public class UserRepository:IUserRepository
    {
        private DatabaseContext dbContext;

        public UserRepository(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateUser(CreateUserDTO user)
        {
            Mapper mapper = AutoMapperUtil.ReturnMapper(new MapperConfiguration(con => con.CreateMap<CreateUserDTO, User>()));
            User fullUser = mapper.Map<User>(user);
            var achievements = dbContext.InitAchievemnets();
            fullUser.Achievements = achievements;
            fullUser.PasswordHash =PasswordHasher.HashPassword(fullUser.PasswordHash);
            dbContext.Users?.Add(fullUser);
            await dbContext.SaveChangesAsync();
        }
    }
}
