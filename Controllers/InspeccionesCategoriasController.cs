using API.Inspecciones.Models;
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
        private readonly InspeccionesService _inspeccionesService;
        private readonly InspeccionesCategoriasService _inspeccionesCategoriasService;
        private readonly CategoriasService _categoriasService;

        public InspeccionesCategoriasController(InspeccionesService inspeccionesService, InspeccionesCategoriasService inspeccionesCategoriasService, CategoriasService categoriasService)
        {
            _inspeccionesService            = inspeccionesService;
            _inspeccionesCategoriasService  = inspeccionesCategoriasService;
            _categoriasService              = categoriasService;
        }       

        [HttpPost("GetPreguntas")]
        [Authorize]
        public async Task<ActionResult<dynamic>> GetPreguntas(JsonObject data)
        {
            JsonReturn objReturn = new JsonReturn();

            try
            {
                var idInspeccion = Globals.ParseGuid(Globals.JsonData(data).idInspeccion);

                Inspeccion objModel = await _inspeccionesService.FindSelectorById(idInspeccion, "Folio,UnidadNumeroEconomico,IdUnidadTipo,UnidadTipoName,UnidadMarcaName,NumeroSerie,IdInspeccionTipo,InspeccionTipoName,Locacion,FechaInspeccionInicial,FechaEvaluacion");

                var objInspeccion = new
                {
                    objModel.Folio,
                    objModel.UnidadNumeroEconomico,
                    objModel.UnidadTipoName,
                    objModel.UnidadMarcaName,
                    objModel.NumeroSerie,
                    objModel.InspeccionTipoName,
                    objModel.Locacion,
                    objModel.FechaInspeccionInicial,
                };

                List<dynamic> lstCategorias = new List<dynamic>();
                if (objModel.FechaEvaluacion.HasValue)
                {
                    lstCategorias = await _inspeccionesCategoriasService.ListEvaluacion(idInspeccion);
                } 
                else
                {
                    lstCategorias = await _categoriasService.ListEvaluacion(objModel.IdInspeccionTipo);
                }

                objReturn.Result = new
                {
                    Inspeccion = objInspeccion,
                    Categorias = lstCategorias,
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
                bool isParcial = await _inspeccionesCategoriasService.Create(Globals.JsonData(data), User);

                objReturn.Title     = isParcial ? "Guardado parcial" : "Finalizado";
                objReturn.Message   = isParcial ? "La evaluación ha sido guardada de forma parcial" : "Evaluación finalizada exitosamente";
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
