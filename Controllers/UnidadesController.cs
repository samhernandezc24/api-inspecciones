using System.Text.Json.Nodes;
using API.Inspecciones.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workcube.Libraries;

namespace API.Inspecciones.Controllers
{
    [Route("/api/Inspecciones/Unidades")]
    [ApiController]
    public class UnidadesController : ControllerBase
    {
        private readonly UnidadesService _unidadesService;
        public UnidadesController(UnidadesService unidadesService)
        {
            _unidadesService = unidadesService;
        }

        [HttpPost("Index")]
        [Authorize]
        public async Task<ActionResult<dynamic>> Index()
        {
            JsonReturn objReturn = new JsonReturn();

            try
            {                
                List<dynamic> lstUsuarios   = await _unidadesService.ListUsuarios();
                var lstUnidadesMarcas       = await HttpReq.Post("unidades", "unidadesMarcas/List");
                var lstUnidadesTipos        = await HttpReq.Post("unidades", "unidades/tipos/List");

                var dataSourcePersistence = await HttpReq.Post("account", "DataSourcePersistence/find", Globals.TableDataSource("Unidades", User));

                objReturn.Result = new
                {
                    dataSourcePersistence   = dataSourcePersistence,
                    Usuarios                = lstUsuarios,
                    UnidadesMarcas          = lstUnidadesMarcas,
                    UnidadesTipos           = lstUnidadesTipos,
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

        [HttpPost("DataSource")]
        [Authorize]
        public async Task<ActionResult<dynamic>> DataSource(JsonObject data)
        {
            JsonReturn objReturn = new JsonReturn();

            try
            {
                objReturn.Result = await _unidadesService.DataSource(Globals.JsonData(data), User);

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
                var lstBases            = await HttpReq.Post("catalogos", "bases/List");
                var lstUnidadesMarcas   = await HttpReq.Post("unidades", "unidadesMarcas/List");
                var lstUnidadesTipos    = await HttpReq.Post("unidades", "unidades/tipos/List");

                objReturn.Result = new
                {
                    Bases           = lstBases,
                    UnidadesMarcas  = lstUnidadesMarcas,
                    UnidadesTipos   = lstUnidadesTipos,
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
                await _unidadesService.Create(Globals.JsonData(data), User);

                objReturn.Title     = "Nueva unidad";
                objReturn.Message   = "La unidad se ha creado exitosamente";
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
                var objData = Globals.JsonData(data);

                objReturn.Result = await _unidadesService.Predictive(objData);

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

        [HttpPost("Reporte")]
        [Authorize]
        public async Task<ActionResult<dynamic>> Reporte(JsonObject data)
        {
            JsonReturn objReturn = new JsonReturn();

            try
            {
                byte[] file = await _unidadesService.Reporte(Globals.JsonData(data));

                objReturn.Result = Globals.GetBase64(file);

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
