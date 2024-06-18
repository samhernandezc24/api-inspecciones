using API.Inspecciones.Models;
using API.Inspecciones.Persistence;
using API.Inspecciones.ViewModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
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
        private readonly IWebHostEnvironment _root;

        public InspeccionesService(Context context, IMapper mapper, IWebHostEnvironment root)
        {
            _context    = context;
            _mapper     = mapper;
            _root       = root;
        }

        public async Task Cancel(dynamic data, ClaimsPrincipal user)
        {
            var objTransaction = _context.Database.BeginTransaction();

            string idInspeccion = Globals.ParseGuid(data.idInspeccion);

            // ENCONTRAR INSPECCION PARA CANCELAR
            Inspeccion objModel = await Find(idInspeccion);

            if (objModel == null) { throw new ArgumentException("No se encontró la inspección."); }

            if (objModel.FechaInspeccionFinal != null || objModel.IdInspeccionEstatus == "ea52bdfd-8af6-4f5a-b182-2b99e554eb35")
            {
                throw new ArgumentException("La inspección ya fue cancelada anteriormente.");
            }

            // CANCELAR INSPECCION
            objModel.IdInspeccionEstatus    = "ea52bdfd-8af6-4f5a-b182-2b99e554eb35";
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
            IQueryable<InspeccionViewModel> lstItems = DataSourceExpression(data);
            DataSourceBuilder<InspeccionViewModel> objDataTableBuilder = new DataSourceBuilder<InspeccionViewModel>(data, lstItems);
            var objDataTableResult = await objDataTableBuilder.build();

            // CONSTRUCCION RETORNO DE DATOS
            List<InspeccionViewModel> lstOriginal = objDataTableResult.rows;
            List<dynamic> lstRows = new List<dynamic>();

            int length  = (int)data.length;
            int page    = (int)data.page;
            int index   = ((page - 1) * length) + 1;

            lstOriginal.ForEach(item =>
            {
                lstRows.Add(new
                {
                    index                           = index,
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
                    IsCancelado                     = item.IdInspeccionEstatus == "ea52bdfd-8af6-4f5a-b182-2b99e554eb35",
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
                index++;
            });

            var objReturn = new
            {
                rows    = lstRows,
                count   = objDataTableResult.count,
                length  = objDataTableResult.length,
                pages   = objDataTableResult.pages,
                page    = objDataTableResult.page,
            };

            return objReturn;
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
                {"IdUnidadTipo",        (strValue) => item => item.IdUnidadTipo         == strValue},
                {"IdInspeccionEstatus", (strValue) => item => item.IdInspeccionEstatus  == strValue},
                {"IdCreatedUser",       (strValue) => item => item.IdCreatedUser        == strValue},
                {"IdUpdatedUser",       (strValue) => item => item.IdUpdatedUser        == strValue},
            };

            // FILTROS MULTIPLE
            var filtersMultiple = new Dictionary<string, Func<string, Expression<Func<Inspeccion, bool>>>> { };

            // FILTROS FECHAS
            DateTime? dateFrom  = SourceExpression<Inspeccion>.Date((string)data.dateFrom);
            DateTime? dateTo    = SourceExpression<Inspeccion>.Date((string)data.dateTo);

            var dates = new Dictionary<string, DateExpression<Inspeccion>>()
            {
                { "FechaProgramada",            new DateExpression<Inspeccion>{ dateFrom = item => item.FechaProgramada.Date                >= dateFrom, dateTo = item => item.FechaProgramada.Date                 <= dateTo } },                
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
                case "folio"                    : sortExpression = (x => x.Folio);                  break;
                case "inspeccionEstatusName"    : sortExpression = (x => x.InspeccionEstatusName);  break;
                case "fechaProgramada"          : sortExpression = (x => x.FechaProgramada);        break;
                case "createdFechaNatural"      : sortExpression = (x => x.CreatedFecha);           break;
                default                         : sortExpression = (x => x.CreatedFecha);           break;
            }

            // COMPLETE
            IQueryable<Inspeccion> rows = _context.Inspecciones.AsNoTracking();

            rows = (orderDirection == "asc") ? rows.OrderBy(sortExpression) : rows.OrderByDescending(sortExpression);

            string fields = "IdInspeccion,RequerimientoFolio,Folio,FechaProgramada,FechaInspeccionInicial,UserInspeccionInicialName,FechaInspeccionFinal,UserInspeccionFinalName,IdInspeccionEstatus,InspeccionEstatusName,InspeccionTipoCodigo,InspeccionTipoName,BaseName,IdUnidad,UnidadNumeroEconomico,IsUnidadTemporal,UnidadTipoName,UnidadMarcaName,UnidadPlacaTipoName,Placa,NumeroSerie,Modelo,AnioEquipo,Capacidad,UnidadCapacidadMedidaName,Evaluado,FechaEvaluacion,Locacion,TipoPlataforma,Odometro,Horometro,Observaciones,FirmaOperador,FirmaVerificador,CreatedUserName,CreatedFecha,UpdatedUserName,UpdatedFecha";

            lstItems = rows
                        .Where(x => !x.Deleted)
                        .Where(ExpFullWhere)
                        .Select(Globals.BuildSelector<Inspeccion, Inspeccion>(fields))
                        .ProjectTo<InspeccionViewModel>(_mapper.ConfigurationProvider);

            return lstItems;
        }

        public Task Delete(dynamic data, ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }

        public async Task Finish(dynamic data, ClaimsPrincipal user)
        {
            var objUser = Globals.GetUser(user);
            var objTransaction = _context.Database.BeginTransaction();

            string idInspeccion = Globals.ParseGuid(data.idInspeccion);

            // ENCONTRAR INSPECCION PARA FINALIZAR
            Inspeccion objModel = await Find(idInspeccion);

            if (objModel == null) { throw new ArgumentException("No se encontró la inspección."); }

            if (objModel.FechaInspeccionFinal != null || objModel.IdInspeccionEstatus == "ea52bdfd-8af6-4f5a-b182-2b99e554eb34" || objModel.IdInspeccionEstatus == "ea52bdfd-8af6-4f5a-b182-2b99e554eb35")
            {
                throw new ArgumentException("La inspección ya fue finalizada o cancelada anteriormente.");
            }

            // FIRMA VERIFICADOR
            string verificadorFileBase64        = Globals.ToString(data.firmaVerificador);
            string verificadorFileExtension     = "." + Globals.ToString(data.fileExtensionVerificador);
            string verificadorFilePath          = FileManager.GetNamePath(verificadorFileExtension);

            // FIRMA OPERADOR
            string operadorFileBase64           = Globals.ToString(data.firmaOperador);
            string operadorFileExtension        = "." + Globals.ToString(data.fileExtensionOperador);
            string operadorFilePath             = FileManager.GetNamePath(operadorFileExtension);

            // FINALIZAR INSPECCION
            objModel.FechaInspeccionFinal          = Globals.DateTime(data.fechaInspeccionFinal);
            objModel.FechaInspeccionFinalUpdate    = Globals.DateTime(data.fechaInspeccionFinal);
            objModel.IdUserInspeccionFinal         = objUser.Id;
            objModel.UserInspeccionFinalName       = objUser.Nombre;

            objModel.IdInspeccionEstatus    = "ea52bdfd-8af6-4f5a-b182-2b99e554eb34";
            objModel.InspeccionEstatusName  = "FINALIZADO";
            objModel.FirmaVerificador       = verificadorFilePath;
            objModel.FirmaOperador          = operadorFilePath;
            objModel.Observaciones          = Globals.ToString(data.observaciones);
            objModel.SetUpdated(objUser);

            _context.Inspecciones.Update(objModel);
            await _context.SaveChangesAsync();

            string verificadorDir   = _root.ContentRootPath + "\\Ficheros\\Inspecciones\\FirmasVerificador\\";
            string operadorDir      = _root.ContentRootPath + "\\Ficheros\\Inspecciones\\FirmasOperador\\";

            if (!FileManager.ValidateExtension(verificadorFileExtension)) { throw new AppException(ExceptionMessage.CAST_002); }
            if (!FileManager.ValidateExtension(operadorFileExtension)) { throw new AppException(ExceptionMessage.CAST_002); }

            FileManager.ValidateDirectory(verificadorDir);
            FileManager.ValidateDirectory(operadorDir);

            await FileManager.SaveFileBase64(verificadorFileBase64, verificadorDir + objModel.FirmaVerificador);
            await FileManager.SaveFileBase64(operadorFileBase64, operadorDir + objModel.FirmaOperador);

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