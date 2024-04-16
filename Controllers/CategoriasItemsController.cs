using API.Inspecciones.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;
using Workcube.Libraries;

namespace API.Inspecciones.Controllers
{
    [Route("api/Inspecciones/Tipos/Categorias/Items")]
    [ApiController]
    public class CategoriasItemsController : ControllerBase
    {
        private readonly CategoriasItemsService _categoriasItemsService;
        private readonly FormulariosTiposService _formulariosTiposService;

        public CategoriasItemsController(CategoriasItemsService categoriasItemsService, FormulariosTiposService formulariosTiposService)
        {
            _categoriasItemsService     = categoriasItemsService;
            _formulariosTiposService    = formulariosTiposService;
        }

        [HttpPost("List")]
        [Authorize]
        public async Task<ActionResult<dynamic>> List(JsonObject data)
        {
            JsonReturn objReturn = new JsonReturn();

            try
            {
                var objData         = Globals.JsonData(data);
                var idCategoria     = Globals.ParseGuid(objData.idCategoria);

                List<dynamic> lstCategoriasItems = await _categoriasItemsService.List(idCategoria);
                List<dynamic> lstFormulariosTipos = await _formulariosTiposService.List();

                objReturn.Result = new
                {
                    categoriasItems     = lstCategoriasItems,
                    formulariosTipos    = lstFormulariosTipos,
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
                await _categoriasItemsService.Create(Globals.JsonData(data), User);

                objReturn.Title     = "Nueva pregunta";
                objReturn.Message   = "La pregunta se ha creado exitosamente";
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

        [HttpPost("StoreDuplicate")]
        [Authorize]
        public async Task<ActionResult<dynamic>> StoreDuplicate(JsonObject data)
        {
            JsonReturn objReturn = new JsonReturn();

            try
            {
                await _categoriasItemsService.CreateDuplicate(Globals.JsonData(data), User);

                objReturn.Title     = "Nueva pregunta duplicada";
                objReturn.Message   = "La pregunta se ha duplicado exitosamente";
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
                await _categoriasItemsService.Update(Globals.JsonData(data), User);

                objReturn.Title     = "Actualizado";
                objReturn.Message   = "La pregunta se ha actualizado exitosamente";
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
                await _categoriasItemsService.Delete(Globals.JsonData(data), User);

                objReturn.Title     = "Eliminado";
                objReturn.Message   = "La pregunta se ha eliminado exitosamente";
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
