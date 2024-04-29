using API.Inspecciones.Models;
using API.Inspecciones.Persistence;
using API.Inspecciones.ViewModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

        public InspeccionesService(Context context, IMapper mapper)
        {
            _context    = context;
            _mapper     = mapper;
        }

        public async Task Cancel(dynamic data, ClaimsPrincipal user)
        {
            var objTransaction = _context.Database.BeginTransaction();

            string idInspeccion = Globals.ParseGuid(data.idInspeccion);

            // CANCELAR INSPECCIÓN
            Inspeccion objModel = await Find(idInspeccion);

            if (objModel.IdInspeccionEstatus == "ea52bdfd-8af6-4f5a-b182-2b99e554eb35") { throw new ArgumentException("La inspección ya fue cancelada anteriormente."); }

            objModel.IdInspeccionEstatus    = "ea52bdfd-8af6-4f5a-b182-2b99e554eb35";
            objModel.InspeccionEstatusName  = "CANCELADO";
            objModel.SetUpdated(Globals.GetUser(user));

            await _context.SaveChangesAsync();
            objTransaction.Commit();
        }

        public async Task Create(dynamic data, ClaimsPrincipal user)
        {
            var objTransaction = _context.Database.BeginTransaction();

            // GUARDAR INSPECCIÓN
            Inspeccion objModel = new Inspeccion();

            objModel.IdInspeccion                   = Guid.NewGuid().ToString();
            objModel.Fecha                          = Globals.DateTime(data.fecha);
            objModel.IdBase                         = Globals.ParseGuid(data.idBase);
            objModel.BaseName                       = Globals.ToUpper(data.baseName);
            objModel.IdInspeccionEstatus            = "ea52bdfd-8af6-4f5a-b182-2b99e554eb31";
            objModel.InspeccionEstatusName          = "POR EVALUAR";
            objModel.IdInspeccionTipo               = Globals.ParseGuid(data.idInspeccionTipo);
            objModel.InspeccionTipoCodigo           = Globals.ToUpper(data.inspeccionTipoCodigo);
            objModel.InspeccionTipoName             = Globals.ToUpper(data.inspeccionTipoName);
            objModel.FechaInspeccionInicial         = DateTime.Now;
            objModel.FechaInspeccionInicialUpdate   = DateTime.Now;
            objModel.IdUserInspeccionInicial        = Globals.GetUser(user).Id;
            objModel.UserInspeccionInicialName      = Globals.GetUser(user).Nombre;
            objModel.FechaInspeccionFinal           = Globals.DateTime(data.fechaInspeccionFinal);
            objModel.FechaInspeccionFinalUpdate     = Globals.DateTime(data.fechaInspeccionFinal);
            objModel.IdUserInspeccionFinal          = Globals.GetUser(user).Id;
            objModel.UserInspeccionFinalName        = Globals.GetUser(user).Nombre;
            objModel.IdRequerimiento                = Globals.ParseGuid(data.idRequerimiento);
            objModel.RequerimientoFolio             = Globals.ToUpper(data.requerimientoFolio);
            objModel.IdUnidad                       = Globals.ParseGuid(data.idUnidad);
            objModel.UnidadNumeroEconomico          = Globals.ToUpper(data.unidadNumeroEconomico);
            objModel.IsUnidadTemporal               = Globals.ParseBool(data.isUnidadTemporal);
            objModel.IdUnidadTipo                   = Globals.ParseGuid(data.idUnidadTipo);
            objModel.UnidadTipoName                 = Globals.ParseGuid(data.unidadTipoName);
            objModel.IdUnidadMarca                  = Globals.ParseGuid(data.idUnidadMarca);
            objModel.UnidadMarcaName                = Globals.ToUpper(data.unidadMarcaName);
            objModel.IdUnidadPlacaTipo              = Globals.ParseGuid(data.idUnidadPlacaTipo);
            objModel.UnidadPlacaTipoName            = Globals.ToUpper(data.unidadPlacaTipoName);
            objModel.Placa                          = Globals.ToUpper(data.placa);
            objModel.NumeroSerie                    = Globals.ToUpper(data.numeroSerie);
            objModel.Modelo                         = Globals.ToUpper(data.modelo);
            objModel.AnioEquipo                     = Globals.ToString(data.anioEquipo);
            objModel.Locacion                       = Globals.ToUpper(data.locacion);
            objModel.TipoPlataforma                 = Globals.ToUpper(data.tipoPlataforma);
            objModel.Capacidad                      = Globals.ParseDecimal(data.capacidad) ?? 0;
            objModel.Odometro                       = Globals.ParseInt(data.horometro) ?? 0;
            objModel.Horometro                      = Globals.ParseInt(data.horometro) ?? 0;
            objModel.Observaciones                  = Globals.ToUpper(data.observaciones);
            objModel.FirmaOperador                  = Globals.ToUpper(data.firmaOperador);
            objModel.FirmaVerificador               = Globals.ToUpper(data.firmaVerificador);

            NextFolio(ref objModel);
            objModel.SetCreated(Globals.GetUser(user));

            _context.Inspecciones.Add(objModel);

            await _context.SaveChangesAsync();
            objTransaction.Commit();
        }

        public async Task CreateFromRequerimientos(dynamic data, ClaimsPrincipal user)
        {
            // GUARDAR INSPECCIÓN DESDE REQUERIMIENTOS
            Inspeccion objModel = new Inspeccion();

            objModel.IdInspeccion                   = Guid.NewGuid().ToString();
            objModel.Fecha                          = Globals.DateTime(data.fecha);
            objModel.IdBase                         = Globals.ParseGuid(data.idBase);
            objModel.BaseName                       = Globals.ToUpper(data.baseName);
            objModel.IdInspeccionEstatus            = "ea52bdfd-8af6-4f5a-b182-2b99e554eb31";
            objModel.InspeccionEstatusName          = "POR EVALUAR";
            objModel.IdInspeccionTipo               = Globals.ParseGuid(data.idInspeccionTipo);
            objModel.InspeccionTipoCodigo           = Globals.ToUpper(data.inspeccionTipoCodigo);
            objModel.InspeccionTipoName             = Globals.ToUpper(data.inspeccionTipoName);
            objModel.FechaInspeccionInicial         = DateTime.Now;
            objModel.FechaInspeccionInicialUpdate   = DateTime.Now;
            objModel.IdUserInspeccionInicial        = Globals.GetUser(user).Id;
            objModel.UserInspeccionInicialName      = Globals.GetUser(user).Nombre;
            objModel.FechaInspeccionFinal           = Globals.DateTime(data.fechaInspeccionFinal);
            objModel.FechaInspeccionFinalUpdate     = Globals.DateTime(data.fechaInspeccionFinal);
            objModel.IdUserInspeccionFinal          = Globals.GetUser(user).Id;
            objModel.UserInspeccionFinalName        = Globals.GetUser(user).Nombre;
            objModel.IdRequerimiento                = Globals.ParseGuid(data.idRequerimiento);
            objModel.RequerimientoFolio             = Globals.ToUpper(data.requerimientoFolio);
            objModel.IdUnidad                       = Globals.ParseGuid(data.idUnidad);
            objModel.UnidadNumeroEconomico          = Globals.ToUpper(data.unidadNumeroEconomico);
            objModel.IsUnidadTemporal               = Globals.ParseBool(data.isUnidadTemporal);
            objModel.IdUnidadMarca                  = Globals.ParseGuid(data.idUnidadMarca);
            objModel.UnidadMarcaName                = Globals.ToUpper(data.unidadMarcaName);
            objModel.IdUnidadPlacaTipo              = Globals.ParseGuid(data.idUnidadPlacaTipo);
            objModel.UnidadPlacaTipoName            = Globals.ToUpper(data.unidadPlacaTipoName);
            objModel.Placa                          = Globals.ToUpper(data.placa);
            objModel.NumeroSerie                    = Globals.ToUpper(data.numeroSerie);
            objModel.Modelo                         = Globals.ToUpper(data.modelo);
            objModel.AnioEquipo                     = Globals.ToString(data.anioEquipo);
            objModel.Locacion                       = Globals.ToUpper(data.locacion);
            objModel.TipoPlataforma                 = Globals.ToUpper(data.tipoPlataforma);
            objModel.Capacidad                      = Globals.ParseDecimal(data.capacidad) ?? 0;
            objModel.Odometro                       = Globals.ParseInt(data.horometro) ?? 0;
            objModel.Horometro                      = Globals.ParseInt(data.horometro) ?? 0;
            objModel.Observaciones                  = Globals.ToUpper(data.observaciones);
            objModel.FirmaOperador                  = Globals.ToUpper(data.firmaOperador);
            objModel.FirmaVerificador               = Globals.ToUpper(data.firmaVerificador);

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

            DataSourceBuilder<InspeccionViewModel> objDataSourceBuilder = new DataSourceBuilder<InspeccionViewModel>(data, lstItems);

            var objDataTableResult = await objDataSourceBuilder.build();

            // CONSTRUCCIÓN RETORNO DE DATOS
            List<InspeccionViewModel> lstOriginal = objDataTableResult.rows;
            List<dynamic> lstRows = new List<dynamic>();

            int length  = (int)data.length;
            int page    = (int)data.page;
            int index   = ((page - 1) * length) + 1;

            lstOriginal.ForEach(item =>
            {
                lstRows.Add(new
                {
                    index                   = index,
                    IdInspeccion            = item.IdInspeccion,
                    Folio                   = item.Folio,
                    FechaNatural            = item.FechaNatural,
                    IdBase                  = item.IdBase,
                    BaseName                = item.BaseName,
                    IdInspeccionEstatus     = item.IdInspeccionEstatus,
                    InspeccionEstatusName   = item.InspeccionEstatusName,
                    IsValid                 = item.IsValid,
                    IdRequerimiento         = item.IdRequerimiento,
                    RequerimientoFolio      = item.RequerimientoFolio,
                    HasRequerimiento        = item.HasRequerimiento,
                    IdUnidad                = item.IdUnidad,
                    UnidadNumeroEconomico   = item.UnidadNumeroEconomico,
                    IsUnidadTemporal        = item.IsUnidadTemporal,
                    IdUnidadTipo            = item.IdUnidadTipo,
                    UnidadTipoName          = item.UnidadTipoName,
                    IdUnidadMarca           = item.IdUnidadMarca,
                    UnidadMarcaName         = item.UnidadMarcaName,
                    IdUnidadPlacaTipo       = item.IdUnidadPlacaTipo,
                    UnidadPlacaTipoName     = item.UnidadPlacaTipoName,
                    Placa                   = item.Placa,
                    NumeroSerie             = item.NumeroSerie,
                    Modelo                  = item.Modelo,
                    Locacion                = item.Locacion,
                    AnioEquipo              = item.AnioEquipo,
                    CreatedUserName         = item.CreatedUserName,
                    CreatedFechaNatural     = item.CreatedFechaNatural,
                    UpdatedUserName         = item.UpdatedUserName,
                    UpdatedFechaNatural     = item.UpdatedFechaNatural,
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

            // APLICAR FILTROS DINÁMICOS
            // FILTROS
            var filters = new Dictionary<string, Func<string, Expression<Func<Inspeccion, bool>>>>
            {
                {"HasRequerimiento",    (strValue) => item => !string.IsNullOrEmpty(item.RequerimientoFolio) == Globals.ParseBool(strValue) },
                {"IdCreatedUser",       (strValue) => item => item.IdCreatedUser    == strValue},
                {"IdUpdatedUser",       (strValue) => item => item.IdUpdatedUser    == strValue},
            };

            // FILTROS MULTIPLE
            var filtersMultiple = new Dictionary<string, Func<string, Expression<Func<Inspeccion, bool>>>>
            {
                {"IdInspeccionEstatus",     (strValue) => item => item.IdInspeccionEstatus  == strValue},
                {"IdInspeccionTipo",        (strValue) => item => item.IdInspeccionTipo     == strValue},
                {"IdUnidadTipo",            (strValue) => item => item.IdUnidadTipo         == strValue},
            };

            // FILTROS FECHAS
            DateTime? dateFrom  = SourceExpression<Inspeccion>.Date((string)data.dateFrom);
            DateTime? dateTo    = SourceExpression<Inspeccion>.Date((string)data.dateTo);

            var dates = new Dictionary<string, DateExpression<Inspeccion>>()
            {
                { "Fecha",          new DateExpression<Inspeccion>{ dateFrom = item => item.Fecha.Date          >= dateFrom, dateTo = item => item.Fecha.Date           <= dateTo } },
                { "CreatedFecha",   new DateExpression<Inspeccion>{ dateFrom = item => item.CreatedFecha.Date   >= dateFrom, dateTo = item => item.CreatedFecha.Date    <= dateTo } },
                { "UpdatedFecha",   new DateExpression<Inspeccion>{ dateFrom = item => item.UpdatedFecha.Date   >= dateFrom, dateTo = item => item.UpdatedFecha.Date    <= dateTo } }
            };

            Expression<Func<Inspeccion, bool>> ExpFullWhere = SourceExpression<Inspeccion>.GetExpression(data, filters, dates, filtersMultiple);

            // ORDER BY
            var orderColumn     = Globals.ToString(data.sort.column);
            var orderDirection  = Globals.ToString(data.sort.direction);

            Expression<Func<Inspeccion, object>> sortExpression;

            switch (orderColumn)
            {
                case "folio"                : sortExpression = (x => x.Folio);          break;
                case "fecha"                : sortExpression = (x => x.Fecha);          break;
                default                     : sortExpression = (x => x.CreatedFecha);   break;
            }

            // COMPLETE
            IQueryable<Inspeccion> lstRows = _context.Inspecciones.AsNoTracking();

            lstRows = (orderDirection == "asc") ? lstRows.OrderBy(sortExpression) : lstRows.OrderByDescending(sortExpression);

            string strFields = "IdInspeccion,Folio,Fecha,IdBase,BaseName,IdInspeccionEstatus,InspeccionEstatusName,IdRequerimiento,RequerimientoFolio,IdUnidad,UnidadNumeroEconomico,IsUnidadTemporal,IdUnidadTipo,UnidadTipoName,IdUnidadMarca,UnidadMarcaName,IdUnidadPlacaTipo,UnidadPlacaTipoName,Placa,NumeroSerie,Modelo,Locacion,AnioEquipo,CreatedUserName,CreatedFecha,UpdatedUserName,UpdatedFecha";

            lstItems = lstRows
                        .Where(x => !x.Deleted)
                        .Where(ExpFullWhere)
                        .Select(Globals.BuildSelector<Inspeccion, Inspeccion>(strFields))
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

            // CANCELAR INSPECCIÓN
            Inspeccion objModel = await Find(idInspeccion);

            if (objModel.IdInspeccionEstatus == "ea52bdfd-8af6-4f5a-b182-2b99e554eb34") { throw new ArgumentException("La inspección ya fue finalizada anteriormente."); }
            if (objModel.IdInspeccionEstatus == "ea52bdfd-8af6-4f5a-b182-2b99e554eb35") { throw new ArgumentException("La inspección ya fue cancelada anteriormente."); }

            objModel.IdInspeccionEstatus    = "ea52bdfd-8af6-4f5a-b182-2b99e554eb34";
            objModel.InspeccionEstatusName  = "FINALIZADO";
            objModel.SetUpdated(Globals.GetUser(user));

            await _context.SaveChangesAsync();
            objTransaction.Commit();
        }

        public async Task<Inspeccion> Find(string id)
        {
            return await _context.Inspecciones.FindAsync(id);
        }

        public async Task<Inspeccion> FindSelectorById(string id, string fields)
        {
            return await _context.Inspecciones.AsNoTracking().Where(x => x.IdInspeccion == id)
                        .Select(Globals.BuildSelector<Inspeccion, Inspeccion>(fields)).FirstOrDefaultAsync();
        }

        public async Task<List<dynamic>> FindLastByIds(List<string> lstIds)
        {
            var lstResult = await _context.Inspecciones
                                .AsNoTracking()
                                .Where(x =>  lstIds.Contains(x.IdUnidad) && !x.Deleted)
                                .OrderByDescending(x => x.Fecha)
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

        public Task<List<dynamic>> List()
        {
            throw new NotImplementedException();
        }

        public async Task<List<dynamic>> ListSelector(string id, string fields)
        {
            return await _context.Inspecciones
                .AsNoTracking()
                .Where(x => x.IdInspeccion == id && !x.Deleted)
                .OrderBy(x => x.Fecha)
                .Select(Globals.BuildSelector<Inspeccion, Inspeccion>(fields))
                .ToListAsync<dynamic>();
        }

        public async Task<List<dynamic>> Predictive(dynamic data)
        {
            // INCLUDES
            string fields = "IdInspeccion,Folio,Fecha,IdBase,BaseName,IdInspeccionEstatus,InspeccionEstatusName,IdRequerimiento,RequerimientoFolio,IdUnidad,UnidadNumeroEconomico,IsUnidadTemporal,IdUnidadTipo,UnidadTipoName,IdUnidadMarca,UnidadMarcaName,IdUnidadPlacaTipo,UnidadPlacaTipoName,Placa,NumeroSerie,Modelo,Locacion,AnioEquipo,CreatedUserName,CreatedFecha,UpdatedUserName,UpdatedFecha";

            // QUERY
            var lstItems = _context.Inspecciones
                            .AsNoTracking()
                            .Where(x => !x.Deleted)
                            .OrderBy(w => w.Folio)
                            .Select(Globals.BuildSelector<Inspeccion, Inspeccion>(fields));

            // INITIALIZATION
            DataSourceBuilder<Inspeccion> objDataSourceBuilder = new DataSourceBuilder<Inspeccion>();
            objDataSourceBuilder.Source     = lstItems;
            objDataSourceBuilder.Arguments  = data;

            // SEARCH FILTERS
            Func<Expression<Func<Inspeccion, bool>>, string, string, Expression<Func<Inspeccion, bool>>> argSwitchFilters = (argExpression, argField, search) => { return argExpression; };

            lstItems = objDataSourceBuilder.SearchFilters(argSwitchFilters);

            // TAKE
            lstItems = objDataSourceBuilder.Take();

            // DATA MAPPING
            var lstOriginal     = await lstItems.ToListAsync();
            var lstRows         = new List<dynamic>();

            lstOriginal.ForEach((item) =>
            {
                lstRows.Add(new
                {
                    IdInspeccion            = item.IdInspeccion,
                    Folio                   = item.Folio,
                    Fecha                   = item.Fecha,
                    IdBase                  = item.IdBase,
                    BaseName                = item.BaseName,
                    IdInspeccionEstatus     = item.IdInspeccionEstatus,
                    InspeccionEstatusName   = item.InspeccionEstatusName,
                    IsValid                 = item.IsValid,
                    IdRequerimiento         = item.IdRequerimiento,
                    RequerimientoFolio      = item.RequerimientoFolio,
                    HasRequerimiento        = item.HasRequerimiento,
                    IdUnidad                = item.IdUnidad,
                    UnidadNumeroEconomico   = item.UnidadNumeroEconomico,
                    IsUnidadTemporal        = item.IsUnidadTemporal,
                    IdUnidadTipo            = item.IdUnidadTipo,
                    UnidadTipoName          = item.UnidadTipoName,
                    IdUnidadMarca           = item.IdUnidadMarca,
                    UnidadMarcaName         = item.UnidadMarcaName,
                    IdUnidadPlacaTipo       = item.IdUnidadPlacaTipo,
                    UnidadPlacaTipoName     = item.UnidadPlacaTipoName,
                    Placa                   = item.Placa,
                    NumeroSerie             = item.NumeroSerie,
                    Modelo                  = item.Modelo,
                    Locacion                = item.Locacion,
                    AnioEquipo              = item.AnioEquipo,
                    CreatedUserName         = item.CreatedUserName,
                    CreatedFechaNatural     = item.CreatedFechaNatural,
                    UpdatedUserName         = item.UpdatedUserName,
                    UpdatedFechaNatural     = item.UpdatedFechaNatural,
                });
            });

            return lstRows;
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
