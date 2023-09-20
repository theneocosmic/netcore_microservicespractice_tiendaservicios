using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaServicios.Api.Libros.Aplicacion;
using TiendaServicios.Api.Libros.Model;

namespace TiendaServicios.Api.Libros.Test
{
	public class MappingTest:Profile
	{
        public MappingTest()
        {
            CreateMap<LibreriaMaterial, LibroMaterialDto>();
        }
    }
}
