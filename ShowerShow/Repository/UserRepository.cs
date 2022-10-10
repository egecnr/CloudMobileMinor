using AutoMapper;
using ShowerShow.DAL;
using ShowerShow.DTO;
using ShowerShow.Models;
using ShowerShow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Repository
{
    public class UserRepository
    {
        private DatabaseContext dbContext;

        public UserRepository(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateUser(CreateUserDTO user)
        {
            Mapper mapper = AutoMapperUtil.ReturnMapper(new MapperConfiguration(con => con.CreateMap<CreateUserDTO, User>()));
            dbContext.Users?.Add(mapper.Map<User>(user));
            await dbContext.SaveChangesAsync();
        }
    }
}
