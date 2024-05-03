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

        public InspeccionesFicherosController(InspeccionesService inspeccionesService)
        {
            _inspeccionesService = inspeccionesService;
        }

        [HttpPost("List")]
        [Authorize]
        public async Task<ActionResult<dynamic>> List(JsonObject data)
        {
            JsonReturn objReturn = new JsonReturn();

            try
            {
                var objData         = Globals.JsonData(data);
                var idInspeccion    = Globals.ParseGuid(objData.idInspeccion);

                Inspeccion objModel = await _inspeccionesService.FindSelectorById(idInspeccion, "");

                var objInspeccion = new
                {
                    objModel.UnidadNumeroEconomico,
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
    }
}
