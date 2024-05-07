using API.Inspecciones.Models;
using API.Inspecciones.Persistence;
using API.Inspecciones.ViewModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Numerics;
using System.Security.Claims;
using Workcube.Interfaces;
using Workcube.Libraries;
using Workcube.ViewModels;

namespace API.Inspecciones.Services
{
    public class InspeccionesService : IGlobal<Inspeccion>
    {
        private readonly Context _context;
        private readonly IMapper _mapper;

        public InspeccionesService(Context context, IMapper mapper)
        {
            _context    = context;
            _mapper     = mapper;
        }

        public async Task Cancel(dynamic data, ClaimsPrincipal user)
        {
            var objTransaction = _context.Database.BeginTransaction();

            string idInspeccion = Globals.ParseGuid(data.idInspeccion);

            // ENCONTRAR INSPECCION PARA CANCELAR
            Inspeccion objModel = await Find(idInspeccion);

            if (objModel == null) { throw new ArgumentException("No se encontró la inspección."); }

            if (objModel.FechaInspeccionFinal != null || objModel.IdInspeccionEstatus == "ea52bdfd-8af6-4f5a-b182-2b99e554eb34")
            {
                throw new ArgumentException("La inspección ya fue cancelada anteriormente.");
            }

            // CANCELAR INSPECCION
            objModel.IdInspeccionEstatus    = "ea52bdfd-8af6-4f5a-b182-2b99e554eb34";
            objModel.InspeccionEstatusName  = "CANCELADO";
            objModel.SetUpdated(Globals.GetUser(user));

            _context.Inspecciones.Update(objModel);
            await _context.SaveChangesAsync();
            objTransaction.Commit();
        }

        public async Task Create(dynamic data, ClaimsPrincipal user)
        {
            var objTransaction = _context.Database.BeginTransaction();

            // GUARDAR INSPECCION
            Inspeccion objModel = new Inspeccion();

            objModel.IdInspeccion               = Guid.NewGuid().ToString();
            objModel.FechaProgramada            = Globals.DateTime(data.fechaProgramada);
            objModel.IdInspeccionEstatus        = "ea52bdfd-8af6-4f5a-b182-2b99e554eb31";
            objModel.InspeccionEstatusName      = "POR EVALUAR";
            objModel.IdInspeccionTipo           = Globals.ParseGuid(data.idInspeccionTipo);
            objModel.InspeccionTipoCodigo       = Globals.ToUpper(data.inspeccionTipoCodigo);
            objModel.InspeccionTipoName         = Globals.ToUpper(data.inspeccionTipoName);
            objModel.IdBase                     = Globals.ParseGuid(data.idBase);
            objModel.BaseName                   = Globals.ToUpper(data.baseName);
            objModel.IdUnidad                   = Globals.ParseGuid(data.idUnidad);
            objModel.UnidadNumeroEconomico      = Globals.ToUpper(data.unidadNumeroEconomico);
            objModel.IsUnidadTemporal           = Globals.ParseBool(data.isUnidadTemporal);
            objModel.IdUnidadTipo               = Globals.ParseGuid(data.idUnidadTipo);
            objModel.UnidadTipoName             = Globals.ToUpper(data.unidadTipoName);
            objModel.IdUnidadMarca              = Globals.ParseGuid(data.idUnidadMarca);
            objModel.UnidadMarcaName            = Globals.ToUpper(data.unidadMarcaName);
            objModel.IdUnidadPlacaTipo          = Globals.ParseGuid(data.idUnidadPlacaTipo);
            objModel.UnidadPlacaTipoName        = Globals.ToUpper(data.unidadPlacaTipoName);
            objModel.Placa                      = Globals.ToUpper(data.placa);
            objModel.NumeroSerie                = Globals.ToUpper(data.numeroSerie);
            objModel.Modelo                     = Globals.ToUpper(data.modelo);
            objModel.AnioEquipo                 = Globals.ToString(data.anioEquipo);
            objModel.Capacidad                  = Globals.ParseDecimal(data.capacidad);
            objModel.IdUnidadCapacidadMedida    = Globals.ParseGuid(data.idUnidadCapacidadMedida);
            objModel.UnidadCapacidadMedidaName  = Globals.ToUpper(data.unidadCapacidadMedidaName);
            objModel.Evaluado                   = false;
            objModel.Locacion                   = Globals.ToUpper(data.locacion);
            objModel.TipoPlataforma             = Globals.ToUpper(data.tipoPlataforma);
            objModel.Odometro                   = Globals.ParseInt(data.odometro)   ?? 0;
            objModel.Horometro                  = Globals.ParseInt(data.horometro)  ?? 0;

            NextFolio(ref objModel);
            objModel.SetCreated(Globals.GetUser(user));

            _context.Inspecciones.Add(objModel);
            await _context.SaveChangesAsync();
            objTransaction.Commit();
        }

        public async Task CreateFromRequerimientos(dynamic data, ClaimsPrincipal user)
        {
            // GUARDAR INSPECCION DESDE REQUERIMIENTOS
            Inspeccion objModel = new Inspeccion();

            objModel.IdInspeccion               = Guid.NewGuid().ToString();
            objModel.IdRequerimiento            = Globals.ParseGuid(data.idRequerimiento);
            objModel.RequerimientoFolio         = Globals.ToUpper(data.requerimientoFolio);
            objModel.FechaProgramada            = Globals.DateTime(data.fechaProgramada);
            objModel.IdInspeccionEstatus        = "ea52bdfd-8af6-4f5a-b182-2b99e554eb31";
            objModel.InspeccionEstatusName      = "POR EVALUAR";
            objModel.IdInspeccionTipo           = Globals.ParseGuid(data.idInspeccionTipo);
            objModel.InspeccionTipoCodigo       = Globals.ToUpper(data.inspeccionTipoCodigo);
            objModel.InspeccionTipoName         = Globals.ToUpper(data.inspeccionTipoName);
            objModel.IdBase                     = Globals.ParseGuid(data.idBase);
            objModel.BaseName                   = Globals.ToUpper(data.baseName);
            objModel.IdUnidad                   = Globals.ParseGuid(data.idUnidad);
            objModel.UnidadNumeroEconomico      = Globals.ToUpper(data.unidadNumeroEconomico);
            objModel.IsUnidadTemporal           = Globals.ParseBool(data.isUnidadTemporal);
            objModel.IdUnidadTipo               = Globals.ParseGuid(data.idUnidadTipo);
            objModel.UnidadTipoName             = Globals.ToUpper(data.unidadTipoName);
            objModel.IdUnidadMarca              = Globals.ParseGuid(data.idUnidadMarca);
            objModel.UnidadMarcaName            = Globals.ToUpper(data.unidadMarcaName);
            objModel.IdUnidadPlacaTipo          = Globals.ParseGuid(data.idUnidadPlacaTipo);
            objModel.UnidadPlacaTipoName        = Globals.ToUpper(data.unidadPlacaTipoName);
            objModel.Placa                      = Globals.ToUpper(data.placa);
            objModel.NumeroSerie                = Globals.ToUpper(data.numeroSerie);
            objModel.Modelo                     = Globals.ToUpper(data.modelo);
            objModel.AnioEquipo                 = Globals.ToString(data.anioEquipo);
            objModel.Capacidad                  = Globals.ParseDecimal(data.capacidad);
            objModel.IdUnidadCapacidadMedida    = Globals.ParseGuid(data.idUnidadCapacidadMedida);
            objModel.UnidadCapacidadMedidaName  = Globals.ToUpper(data.unidadCapacidadMedidaName);
            objModel.Evaluado                   = false;
            objModel.Locacion                   = Globals.ToUpper(data.locacion);
            objModel.TipoPlataforma             = Globals.ToUpper(data.tipoPlataforma);
            objModel.Odometro                   = Globals.ParseInt(data.odometro)   ?? 0;
            objModel.Horometro                  = Globals.ParseInt(data.horometro)  ?? 0;

            NextFolio(ref objModel);
            objModel.SetCreated(Globals.GetUser(user));

            _context.Inspecciones.Add(objModel);
            await _context.SaveChangesAsync();
        }

        private void NextFolio(ref Inspeccion objInspeccion)
        {
            var anio        = DateTime.Now.Year.ToString().Substring(2, 2);
            var contains    = "INS-" + anio + "-VH";

            int indexKey = NextIndexKey(contains);

            string folio = contains + "-" + indexKey.ToString().PadLeft(6, '0');
            objInspeccion.Folio = folio;
        }

        public int NextIndexKey(string contains)
        {
            return _context.Inspecciones.Where(item => item.Folio.Contains(contains)).Count() + 1;
        }

        public async Task<dynamic> DataSource(dynamic data, ClaimsPrincipal user)
        {
            IQueryable<InspeccionViewModel> lstData = DataSourceExpression(data);
            DataSourceBuilder<InspeccionViewModel> objDataTableBuilder = new DataSourceBuilder<InspeccionViewModel>(data, lstData);
            var objDataBuilder = await objDataTableBuilder.build();

            // CONSTRUCCION RETORNO DE DATOS
            List<InspeccionViewModel> lstOriginal = objDataBuilder.rows;
            List<dynamic> lstRows = new List<dynamic>();

            lstOriginal.ForEach(item =>
            {
                lstRows.Add(new
                {
                    IdInspeccion                    = item.IdInspeccion,
                    RequerimientoFolio              = item.RequerimientoFolio,
                    HasRequerimiento                = item.HasRequerimiento,
                    Folio                           = item.Folio,
                    FechaProgramada                 = item.FechaProgramada,
                    FechaProgramadaNatural          = item.FechaProgramadaNatural,
                    FechaInspeccionInicialNatural   = item.FechaInspeccionInicialNatural,
                    UserInspeccionInicialName       = item.UserInspeccionInicialName,
                    FechaInspeccionFinalNatural     = item.FechaInspeccionFinalNatural,
                    UserInspeccionFinalName         = item.UserInspeccionFinalName,
                    IsValid                         = item.IsValid,
                    IdInspeccionEstatus             = item.IdInspeccionEstatus,
                    InspeccionEstatusName           = item.InspeccionEstatusName,
                    IsCancelado                     = item.IdInspeccionEstatus == "ea52bdfd-8af6-4f5a-b182-2b99e554eb34",
                    InspeccionTipoCodigo            = item.InspeccionTipoCodigo,
                    InspeccionTipoName              = item.InspeccionTipoName,
                    BaseName                        = item.BaseName,
                    IdUnidad                        = item.IdUnidad,
                    UnidadNumeroEconomico           = item.UnidadNumeroEconomico,
                    IsUnidadTemporal                = item.IsUnidadTemporal,
                    UnidadTipoName                  = item.UnidadTipoName,
                    UnidadMarcaName                 = item.UnidadMarcaName,
                    UnidadPlacaTipoName             = item.UnidadPlacaTipoName,
                    Placa                           = item.Placa,
                    NumeroSerie                     = item.NumeroSerie,
                    Modelo                          = item.Modelo,
                    AnioEquipo                      = item.AnioEquipo,
                    Capacidad                       = item.Capacidad,
                    UnidadCapacidadMedidaName       = item.UnidadCapacidadMedidaName,
                    Evaluado                        = item.Evaluado,
                    FechaEvaluacionNatural          = item.FechaEvaluacionNatural,
                    Locacion                        = item.Locacion,
                    TipoPlataforma                  = item.TipoPlataforma,
                    Odometro                        = item.Odometro,
                    Horometro                       = item.Horometro,
                    Observaciones                   = item.Observaciones,
                    FirmaOperador                   = item.FirmaOperador,
                    FirmaVerificador                = item.FirmaVerificador,
                    CreatedUserName                 = item.CreatedUserName,
                    CreatedFechaNatural             = item.CreatedFechaNatural,
                    UpdatedUserName                 = item.UpdatedUserName,
                    UpdatedFechaNatural             = item.UpdatedFechaNatural
                });
            });

            var objResult = new
            {
                rows    = lstRows,
                count   = objDataBuilder.count,
                length  = objDataBuilder.length,
                pages   = objDataBuilder.pages,
                page    = objDataBuilder.page,
            };

            return objResult;
        }

        public IQueryable<InspeccionViewModel> DataSourceExpression(dynamic data)
        {
            // INCLUDES
            IQueryable<InspeccionViewModel> lstItems;

            // APLICAR FILTROS DINAMICOS
            // FILTROS
            var filters = new Dictionary<string, Func<string, Expression<Func<Inspeccion, bool>>>>
            {
                {"HasRequerimiento",    (strValue) => item => !string.IsNullOrEmpty(item.RequerimientoFolio) == Globals.ParseBool(strValue)},
                {"IdCreatedUser",       (strValue) => item => item.IdCreatedUser == strValue},
                {"IdUpdatedUser",       (strValue) => item => item.IdUpdatedUser == strValue},
            };

            // FILTROS MULTIPLE
            var filtersMultiple = new Dictionary<string, Func<string, Expression<Func<Inspeccion, bool>>>>
            {
                {"IdUnidadTipo",            (strValue) => item => item.IdUnidadTipo == strValue},
                {"IdInspeccionEstatus",     (strValue) => item => item.IdInspeccionEstatus == strValue},
                {"IdInspeccionTipo",        (strValue) => item => item.IdInspeccionTipo == strValue},
            };

            // FILTROS FECHAS
            DateTime? dateFrom  = SourceExpression<Inspeccion>.Date((string)data.dateFrom);
            DateTime? dateTo    = SourceExpression<Inspeccion>.Date((string)data.dateTo);

            var dates = new Dictionary<string, DateExpression<Inspeccion>>()
            {
                { "FechaProgramada",            new DateExpression<Inspeccion>{ dateFrom = item => item.FechaProgramada.Date                >= dateFrom, dateTo = item => item.FechaProgramada.Date                 <= dateTo } },
                { "FechaInspeccionInicial",     new DateExpression<Inspeccion>{ dateFrom = item => item.FechaInspeccionInicial.Value.Date   >= dateFrom, dateTo = item => item.FechaInspeccionInicial.Value.Date    <= dateTo } },
                { "FechaInspeccionFinal",       new DateExpression<Inspeccion>{ dateFrom = item => item.FechaInspeccionFinal.Value.Date     >= dateFrom, dateTo = item => item.FechaInspeccionFinal.Value.Date      <= dateTo } },
                { "FechaEvaluacion",            new DateExpression<Inspeccion>{ dateFrom = item => item.FechaEvaluacion.Value.Date          >= dateFrom, dateTo = item => item.FechaEvaluacion.Value.Date           <= dateTo } },                
                { "CreatedFecha",               new DateExpression<Inspeccion>{ dateFrom = item => item.CreatedFecha.Date                   >= dateFrom, dateTo = item => item.CreatedFecha.Date                    <= dateTo } },
                { "UpdatedFecha",               new DateExpression<Inspeccion>{ dateFrom = item => item.UpdatedFecha.Date                   >= dateFrom, dateTo = item => item.UpdatedFecha.Date                    <= dateTo } }
            };

            Expression<Func<Inspeccion, bool>> ExpFullWhere = SourceExpression<Inspeccion>.GetExpression(data, filters, dates, filtersMultiple);

            // ORDER BY
            var orderColumn     = Globals.ToString(data.sort.column);
            var orderDirection  = Globals.ToString(data.sort.direction);

            Expression<Func<Inspeccion, object>> sortExpression;

            switch (orderColumn)
            {
                case "folio"                                : sortExpression = (x => x.Folio);                                  break;
                case "requerimientoFolio"                   : sortExpression = (x => x.RequerimientoFolio);                     break;
                case "fechaProgramdaNatural"                : sortExpression = (x => x.FechaProgramada);                        break;
                case "inspeccionEstatusName"                : sortExpression = (x => x.InspeccionEstatusName);                  break;
                case "createdUserName"                      : sortExpression = (x => x.CreatedUserName);                        break;
                case "createdFechaNatural"                  : sortExpression = (x => x.CreatedFecha);                           break;
                case "updatedUserName"                      : sortExpression = (x => x.UpdatedUserName);                        break;
                case "updatedFechaNatural"                  : sortExpression = (x => x.UpdatedFecha);                           break;
                default                                     : sortExpression = (x => x.CreatedFecha);                           break;
            }

            // COMPLETE
            IQueryable<Inspeccion> lstRows = _context.Inspecciones.AsNoTracking();

            lstRows = (orderDirection == "asc") ? lstRows.OrderBy(sortExpression) : lstRows.OrderByDescending(sortExpression);

            string strfields = "IdInspeccion,Folio,FechaProgramada,IdBase,BaseName,IdInspeccionEstatus,InspeccionEstatusName,IdInspeccionTipo,InspeccionTipoCodigo,InspeccionTipoName,IdRequerimiento,RequerimientoFolio,IdUnidad,UnidadNumeroEconomico,IsUnidadTemporal,IdUnidadTipo,UnidadTipoName,IdUnidadMarca,UnidadMarcaName,IdUnidadPlacaTipo,UnidadPlacaTipoName,Placa,NumeroSerie,Modelo,Locacion,AnioEquipo,CreatedUserName,CreatedFecha,UpdatedUserName,UpdatedFecha";

            lstItems = lstRows
                        .Where(x => !x.Deleted)
                        .Where(ExpFullWhere)
                        .Select(Globals.BuildSelector<Inspeccion, Inspeccion>(strfields))
                        .ProjectTo<InspeccionViewModel>(_mapper.ConfigurationProvider);

            return lstItems;
        }

        public Task Delete(dynamic data, ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }

        public async Task Finish(dynamic data, ClaimsPrincipal user)
        {
            var objTransaction = _context.Database.BeginTransaction();

            string idInspeccion = Globals.ParseGuid(data.idInspeccion);

            // ENCONTRAR INSPECCION PARA FINALIZAR
            Inspeccion objModel = await Find(idInspeccion);

            if (objModel == null) { throw new ArgumentException("No se encontró la inspección."); }

            if (objModel.FechaInspeccionFinal != null || objModel.IdInspeccionEstatus == "ea52bdfd-8af6-4f5a-b182-2b99e554eb33" || objModel.IdInspeccionEstatus == "ea52bdfd-8af6-4f5a-b182-2b99e554eb35")
            {
                throw new ArgumentException("La inspección ya fue finalizada o cancelada anteriormente.");
            }

            // FINALIZAR INSPECCION
            objModel.IdInspeccionEstatus    = "ea52bdfd-8af6-4f5a-b182-2b99e554eb33";
            objModel.InspeccionEstatusName  = "FINALIZADO";
            objModel.SetUpdated(Globals.GetUser(user));

            _context.Inspecciones.Update(objModel);
            await _context.SaveChangesAsync();
            objTransaction.Commit();
        }

        public async Task<Inspeccion> Find(string id)
        {
            return await _context.Inspecciones.FindAsync(id);
        }

        public async Task<List<dynamic>> FindLastByIds(List<string> lstIds)
        {
            var lstResult = await _context.Inspecciones
                                    .AsNoTracking()
                                    .Where(x => lstIds.Contains(x.IdUnidad) && !x.Deleted)
                                    .OrderByDescending(x => x.FechaProgramada)
                                    .ToListAsync<dynamic>();

            var lstGroup = lstResult.GroupBy(x => x.IdUnidad, x => x, (key, data) => new
            {
                key  = key, 
                data = data,
            }).Select(x => new
            {
                IdUnidad    = x.key,
                Inspeccion  = x.data.FirstOrDefault() ?? null,
                IsValid     = x.data.FirstOrDefault()?.IsValid ?? false,
            }).ToList<dynamic>();

            return lstGroup;
        }

        public async Task<Inspeccion> FindSelectorById(string id, string fields)
        {
            return await _context.Inspecciones.AsNoTracking().Where(x => x.IdInspeccion == id)
                            .Select(Globals.BuildSelector<Inspeccion, Inspeccion>(fields)).FirstOrDefaultAsync();
        }

        public Task<List<dynamic>> List()
        {
            throw new NotImplementedException();
        }

        public async Task<List<dynamic>> ListSelector(string id, string fields)
        {
            return await _context.Inspecciones
                            .AsNoTracking()
                            .Where(x => x.IdInspeccion == id && !x.Deleted)
                            .OrderBy(x => x.FechaProgramada)
                            .Select(Globals.BuildSelector<Inspeccion, Inspeccion>(fields))
                            .ToListAsync<dynamic>();
        }

        public async Task<List<dynamic>> ListUnidadesCapacidadesMedidas()
        {
            return await _context.Inspecciones
                            .AsNoTracking()
                            .Where(x => !string.IsNullOrEmpty(x.IdUnidadCapacidadMedida))
                            .Select(x => new
                            {
                                IdUnidadCapacidadMedida = x.IdUnidadCapacidadMedida,
                                Name                    = x.UnidadCapacidadMedidaName,
                            })
                            .Distinct()
                            .ToListAsync<dynamic>();
        }

        public async Task<List<dynamic>> ListUsuarios()
        {
            var lstUsuarios = await _context.Inspecciones
                                        .AsNoTracking()
                                        .Select(x => new
                                        {
                                            IdCreatedUser   = x.IdCreatedUser,
                                            CreatedUserName = x.CreatedUserName,
                                            IdUpdatedUser   = x.IdUpdatedUser,
                                            UpdatedUserName = x.UpdatedUserName,
                                        })
                                        .Distinct()
                                        .ToListAsync();

            var rows = lstUsuarios.SelectMany(x => new[]
            {
                new { Id = x.IdCreatedUser, NombreCompleto = x.CreatedUserName },
                new { Id = x.IdUpdatedUser, NombreCompleto = x.UpdatedUserName },
            })
            .OrderBy(x => x.NombreCompleto)
            .GroupBy(x => x.Id)
            .Select(x => x.First())
            .ToList<dynamic>();

            return rows;
        }

        public Task<byte[]> Reporte(dynamic data)
        {
            throw new NotImplementedException();
        }

        public Task Update(dynamic data, ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }
    }
}