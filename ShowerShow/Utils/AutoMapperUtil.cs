using AutoMapper;
using ShowerShow.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowerShow.Utils
{
    public class AutoMapperUtil
    {
        public static Mapper ReturnMapper(MapperConfiguration config)
        {
            return new Mapper(config);
        }
    }
}
