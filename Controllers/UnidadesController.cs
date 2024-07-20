using System.Numerics;
using System.Text.Json.Nodes;
using API.Inspecciones.Models;
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
        private readonly UnidadesCapacidadesMedidadesService _unidadesCapacidadesMedidadesService;

        public UnidadesController(UnidadesService unidadesService, UnidadesCapacidadesMedidadesService unidadesCapacidadesMedidadesService)
        {
            _unidadesService                        = unidadesService;
            _unidadesCapacidadesMedidadesService    = unidadesCapacidadesMedidadesService;
        }

        [HttpPost("Index")]
        [Authorize]
        public async Task<ActionResult<dynamic>> Index()
        {
            JsonReturn objReturn = new JsonReturn();

            try
            {
                var lstUnidadesTipos        = await HttpReq.Post("unidades", "unidades/tipos/List");
                var lstUnidadesMarcas       = await HttpReq.Post("unidades", "unidadesMarcas/List");
                List<dynamic> lstUsuarios   = await _unidadesService.ListUsuarios();

                var dataSourcePersistence = await HttpReq.Post("account", "DataSourcePersistence/find", Globals.TableDataSource("Unidades", User));

                objReturn.Result = new
                {
                    dataSourcePersistence       = dataSourcePersistence,
                    UnidadesTipos               = lstUnidadesTipos,
                    UnidadesMarcas              = lstUnidadesMarcas,
                    Usuarios                    = lstUsuarios,
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

                List<dynamic> lstUnidadesCapacidadesMedidas = await _unidadesCapacidadesMedidadesService.List();

                objReturn.Result = new
                {
                    Bases                       = lstBases,
                    UnidadesCapacidadesMedidas  = lstUnidadesCapacidadesMedidas,
                    UnidadesMarcas              = lstUnidadesMarcas,
                    UnidadesTipos               = lstUnidadesTipos,
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

        [HttpPost("Store/Externo")]
        [Authorize]
        public async Task<ActionResult<dynamic>> StoreExterno(JsonObject data)
        {
            JsonReturn objReturn = new JsonReturn();

            try
            {
                await _unidadesService.CreateFromRequerimientos(Globals.JsonData(data), User);

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

        [HttpPost("Edit")]
        [Authorize]
        public async Task<ActionResult<dynamic>> Edit(JsonObject data)
        {
            JsonReturn objReturn = new JsonReturn();

            try
            {
                string idUnidad = Globals.ParseGuid(Globals.JsonData(data).idUnidad);

                Unidad objModel = await _unidadesService.FindSelectorById(idUnidad, "IdUnidad,NumeroEconomico,IdBase,IdUnidadTipo,IdUnidadMarca,IdUnidadPlacaTipo,Placa,NumeroSerie,Modelo,AnioEquipo,Descripcion,Capacidad,IdUnidadCapacidadMedida,Horometro,Odometro");

                var lstBases            = await HttpReq.Post("catalogos", "bases/List");
                var lstUnidadesMarcas   = await HttpReq.Post("unidades", "unidadesMarcas/List");
                var lstUnidadesTipos    = await HttpReq.Post("unidades", "unidades/tipos/List");

                List<dynamic> lstUnidadesCapacidadesMedidas = await _unidadesCapacidadesMedidadesService.List();

                var objUnidad = new
                {
                    IdUnidad                    = objModel.IdUnidad,
                    NumeroEconomico             = objModel.NumeroEconomico,
                    IdBase                      = objModel.IdBase,
                    IdUnidadTipo                = objModel.IdUnidadTipo,
                    IdUnidadMarca               = objModel.IdUnidadMarca,
                    IdUnidadPlacaTipo           = objModel.IdUnidadPlacaTipo,
                    Placa                       = objModel.Placa,
                    NumeroSerie                 = objModel.NumeroSerie,
                    Modelo                      = objModel.Modelo,
                    AnioEquipo                  = objModel.AnioEquipo,
                    Descripcion                 = objModel.Descripcion,
                    Capacidad                   = objModel.Capacidad,
                    IdUnidadCapacidadMedida     = objModel.IdUnidadCapacidadMedida,
                    Horometro                   = objModel.Horometro,
                    Odometro                    = objModel.Odometro,
                };

                objReturn.Result = new
                {
                    Unidad                      = objUnidad,
                    Bases                       = lstBases,
                    UnidadesCapacidadesMedidas  = lstUnidadesCapacidadesMedidas,
                    UnidadesMarcas              = lstUnidadesMarcas,
                    UnidadesTipos               = lstUnidadesTipos,
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

        [HttpPost("Update")]
        [Authorize]
        public async Task<ActionResult<dynamic>> Update(JsonObject data)
        {
            JsonReturn objReturn = new JsonReturn();

            try
            {
                await _unidadesService.Update(Globals.JsonData(data), User);

                objReturn.Title     = "Actualizado";
                objReturn.Message   = "La unidad se ha actualizado exitosamente";
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
                await _unidadesService.Delete(Globals.JsonData(data), User);

                objReturn.Title     = "Eliminado";
                objReturn.Message   = "La unidad se ha eliminado exitosamente";
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

        [HttpPost("Predictive")]
        [Authorize]
        public async Task<ActionResult<dynamic>> Predictive(JsonObject data)
        {
            JsonReturn objReturn = new JsonReturn();

            try
            {
                List<dynamic> lstRows = await _unidadesService.Predictive(Globals.JsonData(data));

                objReturn.Result = new { rows = lstRows };

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