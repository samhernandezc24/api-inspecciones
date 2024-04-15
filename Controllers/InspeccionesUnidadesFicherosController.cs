using API.Inspecciones.Models;
using API.Inspecciones.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;
using Workcube.Libraries;

namespace API.Inspecciones.Controllers
{
    [Route("api/Inspecciones/Unidades/Ficheros")]
    [ApiController]
    public class InspeccionesUnidadesFicherosController : ControllerBase
    {
        private readonly InspeccionesUnidadesService _inspeccionesUnidadesService;
        private readonly InspeccionesUnidadesFicherosService _inspeccionesUnidadesFicherosService;

        public InspeccionesUnidadesFicherosController(InspeccionesUnidadesService inspeccionesUnidadesService, InspeccionesUnidadesFicherosService inspeccionesUnidadesFicherosService)
        {
            _inspeccionesUnidadesService            = inspeccionesUnidadesService;
            _inspeccionesUnidadesFicherosService    = inspeccionesUnidadesFicherosService;
        }

        [HttpPost("List")]
        [Authorize]
        public async Task<ActionResult<dynamic>> List(JsonObject data)
        {
            JsonReturn objReturn = new JsonReturn();

            try
            {
                var objData                 = Globals.JsonData(data);
                string idInspeccionUnidad   = Globals.ParseGuid(objData.idInspeccionUnidad);

                InspeccionUnidad objModel = await _inspeccionesUnidadesService.FindSelectorById(idInspeccionUnidad, "Folio,UnidadNumeroEconomico");

                var objInspeccionUnidad = new
                {
                    Folio                   = objModel.Folio,
                    UnidadNumeroEconomico   = objModel.UnidadNumeroEconomico,
                };

                var objFotos = await _inspeccionesUnidadesFicherosService.List(idInspeccionUnidad);

                objReturn.Result = new
                {
                    InspeccionUnidad    = objInspeccionUnidad,
                    FotosGenerales      = objFotos,
                };

                objReturn.Success(SuccessMessage.REQUEST);
            }
            catch (AppException appException)
            {
                objReturn.Exception(appException);
            }
            catch (Exception exception)
            {
                objReturn.Exception(ExceptionMessage.RawException(exception));
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
                await _inspeccionesUnidadesFicherosService.Create(Globals.JsonData(data), User);

                objReturn.Success(SuccessMessage.REQUEST);
            }
            catch (AppException appException)
            {
                objReturn.Exception(appException);
            }
            catch (Exception exception)
            {
                objReturn.Exception(ExceptionMessage.RawException(exception));
            }

            return objReturn.build();
        }

        [HttpPost("Update")]
        [Authorize]
        public async Task<ActionResult<dynamic>> Update(JsonObject data)
        {
            JsonReturn objReturn = new JsonReturn();

            try
            {
                await _inspeccionesUnidadesFicherosService.Update(Globals.JsonData(data), User);

                objReturn.Title     = "Actualización";
                objReturn.Message   = "Datos actualizados exitosamente";
            }
            catch (AppException appException)
            {
                objReturn.Exception(appException);
            }
            catch (Exception exception)
            {
                objReturn.Exception(ExceptionMessage.RawException(exception));
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
                await _inspeccionesUnidadesFicherosService.Delete(Globals.JsonData(data), User);

                objReturn.Title     = "Eliminado";
                objReturn.Message   = "Foto eliminada exitosamente";
            }
            catch (AppException appException)
            {
                objReturn.Exception(appException);
            }
            catch (Exception exception)
            {
                objReturn.Exception(ExceptionMessage.RawException(exception));
            }

            return objReturn.build();
        }
    }
}
