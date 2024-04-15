using API.Inspecciones.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using Workcube.Libraries;

namespace API.Inspecciones.Controllers
{
    [Route("api/Inspecciones/Unidades")]
    [ApiController]
    public class InspeccionesUnidadesController : ControllerBase
    {
        private readonly InspeccionesUnidadesService _inspeccionesUnidadesService;

        public InspeccionesUnidadesController(InspeccionesUnidadesService inspeccionesUnidadesService)
        {
            _inspeccionesUnidadesService = inspeccionesUnidadesService;
        }

        // ENDPOINT PARA APIS EXTERNAS
        [HttpPost("FindLastInspeccionByIds")]
        public async Task<ActionResult<List<dynamic>>> FindLastInspeccionByIds(JsonObject data)
        {
            try
            {
                var objData         = Globals.JsonData(data);
                string idInspeccion = Globals.ParseGuid(objData.idInspeccion);

                List<string> lstIds = JsonConvert.DeserializeObject<List<string>>(Globals.ToString(objData.lstIds));

                return await _inspeccionesUnidadesService.FindLastInspeccionByIds(lstIds);
            }
            catch (Exception exception)
            {

                return BadRequest(exception.Message);
            }
        }
    }
}
