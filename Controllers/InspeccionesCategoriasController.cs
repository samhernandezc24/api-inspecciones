using API.Inspecciones.Interfaces;
using API.Inspecciones.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;
using Workcube.Libraries;

namespace API.Inspecciones.Controllers
{
    [Route("api/Inspecciones/Categorias")]
    [ApiController]
    public class InspeccionesCategoriasController : ControllerBase
    {
        private readonly InspeccionesCategoriasService _inspeccionesCategoriasService;

        public InspeccionesCategoriasController(InspeccionesCategoriasService inspeccionesCategoriasService)
        {
            _inspeccionesCategoriasService = inspeccionesCategoriasService;
        }
        
        [HttpPost("List")]
        [Authorize]
        public async Task<ActionResult<dynamic>> List()
        {
            JsonReturn objReturn = new JsonReturn();

            try
            {
                var lstInspeccionesCategorias = await _inspeccionesCategoriasService.List();

                objReturn.Result = new
                {
                    InspeccionesCategorias = lstInspeccionesCategorias,
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

        [HttpPost("ListByIdInspeccion")]
        [Authorize]
        public async Task<ActionResult<dynamic>> ListByIdInspeccion(JsonObject data)
        {
            JsonReturn objReturn = new JsonReturn();

            try
            {
                var objData                     = Globals.JsonData(data);
                var idInspeccion                = Globals.ParseGuid(objData.idInspeccion);
                var lstInspeccionesCategorias   = await _inspeccionesCategoriasService.List(idInspeccion);

                objReturn.Result = new
                {
                    InspeccionesCategorias = lstInspeccionesCategorias,
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
                await _inspeccionesCategoriasService.Create(Globals.JsonData(data), User);

                objReturn.Title     = "Nueva categoría";
                objReturn.Message   = "Categoría creada exitosamente";
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
                await _inspeccionesCategoriasService.Update(Globals.JsonData(data), User);

                objReturn.Title     = "Actualización";
                objReturn.Message   = "Categoría actualizada exitosamente";
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
                await _inspeccionesCategoriasService.Delete(Globals.JsonData(data), User);

                objReturn.Title     = "Eliminado";
                objReturn.Message   = "Categoría eliminada exitosamente";
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
