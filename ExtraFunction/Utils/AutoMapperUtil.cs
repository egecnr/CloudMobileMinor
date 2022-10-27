using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraFunction.Utils
{
    public class AutoMapperUtil
    {
        public static Mapper ReturnMapper(MapperConfiguration config)
        {
            return new Mapper(config);
        }
    }
}
