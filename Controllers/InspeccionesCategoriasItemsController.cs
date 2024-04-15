using API.Inspecciones.Interfaces;
using API.Inspecciones.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;
using Workcube.Libraries;

namespace API.Inspecciones.Controllers
{
    [Route("api/Inspecciones/Categorias/Items")]
    [ApiController]
    public class InspeccionesCategoriasItemsController : ControllerBase
    {
        private readonly InspeccionesCategoriasItemsService _inspeccionesCategoriasItemsService;

        public InspeccionesCategoriasItemsController(InspeccionesCategoriasItemsService inspeccionesCategoriasItemsService)
        {
            _inspeccionesCategoriasItemsService = inspeccionesCategoriasItemsService;
        }

        [HttpPost("Index")]
        [Authorize]
        public async Task<ActionResult<dynamic>> Index()
        {
            JsonReturn objReturn = new JsonReturn();

            try
            {
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

        [HttpPost("List")]
        [Authorize]
        public async Task<ActionResult<dynamic>> List()
        {
            JsonReturn objReturn = new JsonReturn();

            try
            {
                List<dynamic> lstInspeccionesCategoriasItems = await _inspeccionesCategoriasItemsService.List();

                objReturn.Result = new
                {
                    InspeccionesCategoriasItems = lstInspeccionesCategoriasItems,
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
                objReturn.Result = await _inspeccionesCategoriasItemsService.Create(Globals.JsonData(data), User);

                objReturn.Title     = "Nuevo formulario";
                objReturn.Message   = "Formulario creado exitosamente";
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
                await _inspeccionesCategoriasItemsService.Update(Globals.JsonData(data), User);

                objReturn.Title     = "Actualización";
                objReturn.Message   = "Formulario actualizado exitosamente";
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
                await _inspeccionesCategoriasItemsService.Delete(Globals.JsonData(data), User);

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
