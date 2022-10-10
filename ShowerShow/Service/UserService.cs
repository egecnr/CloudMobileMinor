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

namespace ShowerShow.Service
{
    public class UserService: IUserService
    {
        private DatabaseContext dbContext;

        public UserService(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }
     
        public async Task CreateUser(CreateUserDTO user)
        {
            Mapper mapper= AutoMapperUtil.ReturnMapper(new MapperConfiguration(con => con.CreateMap<CreateUserDTO, User>()));
            dbContext.Users?.Add(mapper.Map<User>(user));
            await dbContext.SaveChangesAsync();
        }
    }
}
