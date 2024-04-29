using API.Inspecciones.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using Workcube.Libraries;

namespace API.Inspecciones.Controllers
{
    [Route("api/Inspecciones")]
    [ApiController]
    public class InspeccionesController : ControllerBase
    {
        private readonly InspeccionesService _inspeccionesService;
        private readonly InspeccionesTiposService _inspeccionesTiposService;

        public InspeccionesController(InspeccionesService inspeccionesService, InspeccionesTiposService inspeccionesTipos)
        {
            _inspeccionesService        = inspeccionesService;
            _inspeccionesTiposService   = inspeccionesTipos;
        }

        [HttpPost("DataSource")]
        [Authorize]
        public async Task<ActionResult<dynamic>> DataSource(JsonObject data)
        {
            JsonReturn objReturn = new JsonReturn();

            try
            {
                objReturn.Result = await _inspeccionesService.DataSource(Globals.JsonData(data), User);

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

        [HttpPost("Create")]
        [Authorize]
        public async Task<ActionResult<dynamic>> Create()
        {
            JsonReturn objReturn = new JsonReturn();

            try
            {
                List<dynamic> lstInspeccionesTipos  = await _inspeccionesTiposService.List();

                objReturn.Result = new
                {
                    InspeccionesTipos   = lstInspeccionesTipos,
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
                await _inspeccionesService.Create(Globals.JsonData(data), User);

                objReturn.Title     = "Nueva inspección";
                objReturn.Message   = "La inspección se ha creado exitosamente";
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

        [HttpPost("PredictiveEOS")]
        [Authorize]
        public async Task<ActionResult<dynamic>> PredictiveEOS(JsonObject data)
        {
            JsonReturn objReturn = new JsonReturn();

            try
            {
                objReturn.Result = await _inspeccionesService.Predictive(Globals.JsonData(data));

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

        [HttpPost("Cancel")]
        [Authorize]
        public async Task<ActionResult<dynamic>> Cancel(JsonObject data)
        {
            JsonReturn objReturn = new JsonReturn();

            try
            {
                await _inspeccionesService.Cancel(Globals.JsonData(data), User);

                objReturn.Title     = "Cancelado";
                objReturn.Message   = "La inspección ha sido cancelada exitosamente";
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

        [HttpPost("Finish")]
        [Authorize]
        public async Task<ActionResult<dynamic>> Finish(JsonObject data)
        {
            JsonReturn objReturn = new JsonReturn();

            try
            {
                await _inspeccionesService.Finish(Globals.JsonData(data), User);

                objReturn.Title     = "Finalizado";
                objReturn.Message   = "La inspección ha sido finalizada exitosamente";
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

        // EXTERNAL APIS
        [HttpPost("StoreFromRequerimientos")]
        public async Task<ActionResult<dynamic>> StoreFromRequerimientos(JsonObject data)
        {
            try
            {
                await _inspeccionesService.CreateFromRequerimientos(Globals.JsonData(data), User);
                return true;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Find")]
        public async Task<ActionResult<dynamic>> Find(JsonObject data)
        {
            try
            {
                dynamic argData = Globals.JsonData(data);

                string id = Globals.ParseGuid(argData.idInspeccion);

                return await _inspeccionesService.Find(id);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost("FindSelector")]
        public async Task<ActionResult<dynamic>> FindSelector(JsonObject data)
        {
            try
            {
                dynamic argData = Globals.JsonData(data);

                string id       = Globals.ParseGuid(argData.idInspeccion);
                string fields   = Globals.ToString(argData.fields);

                return await _inspeccionesService.FindSelectorById(id, fields);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost("FindLastByIds")]
        public async Task<ActionResult<List<dynamic>>> FindLastByIds(JsonObject data)
        {
            try
            {
                var objData = Globals.JsonData(data);

                List<string> lstIds = JsonConvert.DeserializeObject<List<string>>(Globals.ToString(objData.lstIds));

                return await _inspeccionesService.FindLastByIds(lstIds);
            }
            catch (Exception exception)
            {

                return BadRequest(exception.Message);
            }
        }

        [HttpPost("List")]
        public async Task<ActionResult<List<dynamic>>> List()
        {
            try
            {
                return await _inspeccionesService.List();
            }
            catch (Exception exception)
            {

                return BadRequest(exception.Message);
            }
        }

        [HttpPost("ListSelector")]
        public async Task<ActionResult<List<dynamic>>> ListSelector(JsonObject data)
        {
            try
            {
                dynamic argData = Globals.JsonData(data);

                string idInspeccion     = Globals.ParseGuid(argData.idInspeccion);
                string fields           = Globals.ToString(argData.fields);

                return await _inspeccionesService.ListSelector(idInspeccion, fields);
            }
            catch (Exception exception)
            {

                return BadRequest(exception.Message);
            }
        }
    }
}
