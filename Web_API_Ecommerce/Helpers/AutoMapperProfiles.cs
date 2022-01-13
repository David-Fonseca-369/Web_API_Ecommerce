using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Ecommerce.DTOs.Categoria;
using Web_API_Ecommerce.DTOs.Producto;
using Web_API_Ecommerce.DTOs.Usuario;
using Web_API_Ecommerce.DTOs.Venta;
using Web_API_Ecommerce.Entities;

namespace Web_API_Ecommerce.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
            CreateMap<CategoriaCreacionDTO, Categoria>();

            CreateMap<Producto, ProductoDTO>().ReverseMap();
            CreateMap<Producto, ProductoVerDTO>();
            CreateMap<ImagenProducto, ImagenProductoDTO>();
            CreateMap<Usuario, UsuarioDTO>();

            CreateMap<VentaCabecera, VentasDTO>();
        }
    }
}
