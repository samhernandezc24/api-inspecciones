﻿using API.Inspecciones.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Inspecciones.Controllers
{
    [Route("api/unidades/registros")]
    [ApiController]
    public class UnidadesRegistrosController : ControllerBase
    {
        private readonly UnidadesRegistrosService _unidadesRegistrosService;

        public UnidadesRegistrosController(UnidadesRegistrosService unidadesRegistrosService)
        {
            _unidadesRegistrosService = unidadesRegistrosService;
        }
    }
}
