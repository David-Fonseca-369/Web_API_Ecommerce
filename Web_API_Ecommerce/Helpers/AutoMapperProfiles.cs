using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Ecommerce.DTOs.Categoria;
using Web_API_Ecommerce.Entities;

namespace Web_API_Ecommerce.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
            CreateMap<CategoriaCreacionDTO, Categoria>();
        }
    }
}
