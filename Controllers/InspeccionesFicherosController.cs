using System.Text.Json.Nodes;
using API.Inspecciones.Models;
using API.Inspecciones.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workcube.Libraries;

namespace API.Inspecciones.Controllers
{
    [Route("/api/Inspecciones/Ficheros")]
    [ApiController]
    public class InspeccionesFicherosController : ControllerBase
    {
        private readonly InspeccionesService _inspeccionesService;
        private readonly InspeccionesFicherosService _inspeccionesFicherosService;

        public InspeccionesFicherosController(InspeccionesService inspeccionesService, InspeccionesFicherosService inspeccionesFicherosService)
        {
            _inspeccionesService            = inspeccionesService;
            _inspeccionesFicherosService    = inspeccionesFicherosService;
        }

        [HttpPost("List")]
        [Authorize]
        public async Task<ActionResult<dynamic>> List(JsonObject data)
        {
            JsonReturn objReturn = new JsonReturn();

            try
            {
                var idInspeccion    = Globals.ParseGuid(Globals.JsonData(data).idInspeccion);

                Inspeccion objModel = await _inspeccionesService.FindSelectorById(idInspeccion, "UnidadNumeroEconomico,UnidadTipoName,NumeroSerie,IdInspeccionEstatus");

                var objInspeccion = new
                {
                    UnidadNumeroEconomico   = objModel.UnidadNumeroEconomico,
                    UnidadTipoName          = objModel.UnidadTipoName,
                    NumeroSerie             = objModel.NumeroSerie,
                    IdInspeccionEstatus     = objModel.IdInspeccionEstatus,
                };

                List<dynamic> lstFicheros = await _inspeccionesFicherosService.List(idInspeccion);

                objReturn.Result = new
                {
                    Inspeccion  = objInspeccion,
                    Ficheros    = lstFicheros,
                };

                objReturn.Success(SuccessMessage.REQUEST);
            }
            catch (AppException ex)
            {
                objReturn.Exception(ex);
            }
            catch (Exception ex)
            {
                objReturn.Exception(ExceptionMessage.RawException(ex));
            }

            return objReturn.build();
        }

        [HttpPost("Store")]
        [Authorize]
        public async Task<ActionResult<dynamic>> Store(JsonObject data)
        {
            JsonReturn objReturn = new JsonReturn();

            try
            {
                await _inspeccionesFicherosService.Create(Globals.JsonData(data), User);

                objReturn.Success(SuccessMessage.REQUEST);
            }
            catch (AppException ex)
            {
                objReturn.Exception(ex);
            }
            catch (Exception ex)
            {
                objReturn.Exception(ExceptionMessage.RawException(ex));
            }

            return objReturn.build();
        }

        [HttpPost("Delete")]
        [Authorize]
        public async Task<ActionResult<dynamic>> Delete(JsonObject data)
        {
            JsonReturn objReturn = new JsonReturn();

            try
            {
                await _inspeccionesFicherosService.Delete(Globals.JsonData(data), User);

                objReturn.Title     = "Eliminado";
                objReturn.Message   = "La fotografía se ha eliminado exitosamente";
            }
            catch (AppException ex)
            {
                objReturn.Exception(ex);
            }
            catch (Exception ex)
            {
                objReturn.Exception(ExceptionMessage.RawException(ex));
            }

            return objReturn.build();
        }
    }
}
