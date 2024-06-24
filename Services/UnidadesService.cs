using System.Linq.Expressions;
using System.Numerics;
using System.Security.Claims;
using API.Inspecciones.Models;
using API.Inspecciones.Persistence;
using API.Inspecciones.ViewModels;
using API.Inspecciones.ViewModels.Reports;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Workcube.Interfaces;
using Workcube.Libraries;
using Workcube.ViewModels;

namespace API.Inspecciones.Services
{
    public class UnidadesService : IGlobal<Unidad>
    {
        private readonly Context _context;
        private readonly IMapper _mapper;

        public UnidadesService(Context context, IMapper mapper) 
        {
            _context    = context;
            _mapper     = mapper;
        }

        public async Task Create(dynamic data, ClaimsPrincipal user)
        {
            var objTransaction = _context.Database.BeginTransaction();

            string numeroEconomico = Globals.ToUpper(data.numeroEconomico);

            bool findUnidad = await _context.Unidades.AnyAsync(x => x.NumeroEconomico.ToUpper() == numeroEconomico && !x.Deleted);

            if (findUnidad) { throw new ArgumentException("Ya existe una unidad con ese número ecónomico."); }

            // GUARDAR UNIDAD
            Unidad objModel = new Unidad();

            objModel.IdUnidad                   = Guid.NewGuid().ToString();
            objModel.NumeroEconomico            = numeroEconomico;
            objModel.IdBase                     = Globals.ParseGuid(data.idBase);
            objModel.BaseName                   = Globals.ToUpper(data.baseName);
            objModel.IdUnidadTipo               = Globals.ParseGuid(data.idUnidadTipo);
            objModel.UnidadTipoName             = Globals.ToUpper(data.unidadTipoName);
            objModel.IdUnidadMarca              = Globals.ParseGuid(data.idUnidadMarca);
            objModel.UnidadMarcaName            = Globals.ToUpper(data.unidadMarcaName);
            objModel.IdUnidadPlacaTipo          = Globals.ParseGuid(data.idUnidadPlacaTipo);
            objModel.UnidadPlacaTipoName        = Globals.ToUpper(data.unidadPlacaTipoName);
            objModel.Placa                      = Globals.ToUpper(data.placa);
            objModel.NumeroSerie                = Globals.ToUpper(data.numeroSerie);
            objModel.Modelo                     = Globals.ToUpper(data.modelo);
            objModel.AnioEquipo                 = Globals.ToUpper(data.anioEquipo);
            objModel.Descripcion                = Globals.ToUpper(data.descripcion);
            objModel.Capacidad                  = Globals.ParseDecimal(data.capacidad);
            objModel.IdUnidadCapacidadMedida    = Globals.ParseGuid(data.idUnidadCapacidadMedida);
            objModel.UnidadCapacidadMedidaName  = Globals.ToUpper(data.unidadCapacidadMedidaName);
            objModel.Horometro                  = Globals.ParseInt(data.horometro)  ?? 0;
            objModel.Odometro                   = Globals.ParseInt(data.odometro)   ?? 0;
            objModel.SetCreated(Globals.GetUser(user));

            _context.Unidades.Add(objModel);
            await _context.SaveChangesAsync();
            objTransaction.Commit();
        }

        public async Task<dynamic> DataSource(dynamic data, ClaimsPrincipal user)
        {
            IQueryable<UnidadViewModel> lstItems = DataSourceExpression(data);
            DataSourceBuilder<UnidadViewModel> objDataTableBuilder = new DataSourceBuilder<UnidadViewModel>(data, lstItems);
            var objDataTableResult = await objDataTableBuilder.build();

            // CONSTRUCCION RETORNO DE DATOS
            List<UnidadViewModel> lstOriginal = objDataTableResult.rows;
            List<dynamic> lstRows = new List<dynamic>();

            int length  = (int)data.length;
            int page    = (int)data.page;
            int index   = ((page - 1) * length) + 1;

            lstOriginal.ForEach(item =>
            {
                lstRows.Add(new
                {
                    index                           = index,
                    IdUnidad                        = item.IdUnidad,
                    NumeroEconomico                 = item.NumeroEconomico,
                    IdBase                          = item.IdBase,
                    BaseName                        = item.BaseName,
                    IdUnidadTipo                    = item.IdUnidadTipo,
                    UnidadTipoName                  = item.UnidadTipoName,
                    IdUnidadMarca                   = item.IdUnidadMarca,
                    UnidadMarcaName                 = item.UnidadMarcaName,
                    IdUnidadPlacaTipo               = item.IdUnidadPlacaTipo,
                    UnidadPlacaTipoName             = item.UnidadPlacaTipoName,
                    Placa                           = item.Placa,
                    NumeroSerie                     = item.NumeroSerie,
                    Modelo                          = item.Modelo,
                    AnioEquipo                      = item.AnioEquipo,
                    Descripcion                     = item.Descripcion,
                    Capacidad                       = item.Capacidad,
                    IdUnidadCapacidadMedida         = item.IdUnidadCapacidadMedida,
                    UnidadCapacidadMedidaName       = item.UnidadCapacidadMedidaName,
                    Odometro                        = item.Odometro,
                    Horometro                       = item.Horometro,
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

        public IQueryable<UnidadViewModel> DataSourceExpression(dynamic data)
        {
            // INCLUDES
            IQueryable<UnidadViewModel> lstItems;

            // APLICAR FILTROS DINAMICOS
            // FILTROS
            var filters = new Dictionary<string, Func<string, Expression<Func<Unidad, bool>>>>
            {
                {"IdUnidadMarca",       (strValue) => item => item.IdUnidadMarca    == strValue},
                {"IdUnidadTipo",        (strValue) => item => item.IdUnidadTipo     == strValue},
                {"IdCreatedUser",       (strValue) => item => item.IdCreatedUser    == strValue},
                {"IdUpdatedUser",       (strValue) => item => item.IdUpdatedUser    == strValue},
            };

            // FILTROS MULTIPLE
            var filtersMultiple = new Dictionary<string, Func<string, Expression<Func<Unidad, bool>>>> { };

            // FILTROS FECHAS
            DateTime? dateFrom  = SourceExpression<Unidad>.Date((string)data.dateFrom);
            DateTime? dateTo    = SourceExpression<Unidad>.Date((string)data.dateTo);

            var dates = new Dictionary<string, DateExpression<Unidad>>()
            {             
                { "CreatedFecha",   new DateExpression<Unidad>{ dateFrom = item => item.CreatedFecha.Date   >= dateFrom, dateTo = item => item.CreatedFecha.Date    <= dateTo } },
                { "UpdatedFecha",   new DateExpression<Unidad>{ dateFrom = item => item.UpdatedFecha.Date   >= dateFrom, dateTo = item => item.UpdatedFecha.Date    <= dateTo } }
            };

            Expression<Func<Unidad, bool>> ExpFullWhere = SourceExpression<Unidad>.GetExpression(data, filters, dates, filtersMultiple);

            // COMPLETE
            IQueryable<Unidad> rows = _context.Unidades.AsNoTracking();

            string fields = "IdUnidad,NumeroEconomico,IdBase,BaseName,IdUnidadTipo,UnidadTipoName,IdUnidadMarca,UnidadMarcaName,IdUnidadPlacaTipo,UnidadPlacaTipoName,Placa,NumeroSerie,Modelo,AnioEquipo,Descripcion,Capacidad,IdUnidadCapacidadMedida,UnidadCapacidadMedidaName,Odometro,Horometro,CreatedUserName,CreatedFecha,UpdatedUserName,UpdatedFecha";

            lstItems = rows
                         .Where(x => !x.Deleted)
                         .Where(ExpFullWhere)
                         .Select(Globals.BuildSelector<Unidad, Unidad>(fields))
                         .ProjectTo<UnidadViewModel>(_mapper.ConfigurationProvider);

            return lstItems;
        } 

        public async Task Delete(dynamic data, ClaimsPrincipal user)
        {
            var objTransaction = _context.Database.BeginTransaction();

            string idUnidad = Globals.ParseGuid(data.idUnidad);

            // ENCONTRAR UNIDAD A ELIMINAR
            Unidad objModel = await Find(idUnidad);

            if (objModel == null) { throw new ArgumentException("No se encontró la unidad."); }
            if (objModel.Deleted) { throw new ArgumentException("La unidad ya fue eliminada anteriormente."); }

            // ELIMINAR UNIDAD
            objModel.Deleted = true;
            objModel.SetUpdated(Globals.GetUser(user));

            _context.Unidades.Update(objModel);
            await _context.SaveChangesAsync();
            objTransaction.Commit();
        }

        public async Task<Unidad> Find(string id)
        {
            return await _context.Unidades.FindAsync(id);
        }

        public async Task<Unidad> FindSelectorById(string id, string fields)
        {
            return await _context.Unidades.Where(x => x.IdUnidad == id).Select(Globals.BuildSelector<Unidad, Unidad>(fields)).FirstOrDefaultAsync();
        }

        public async Task<List<dynamic>> List()
        {
            return await _context.Unidades
                            .AsNoTracking()
                            .Where(x => !x.Deleted)
                            .OrderBy(x => x.NumeroEconomico)
                            .Select(x => new
                            {
                                IdUnidad                    = x.IdUnidad,
                                NumeroEconomico             = x.NumeroEconomico,
                                IdBase                      = x.IdBase,
                                BaseName                    = x.BaseName,
                                IdUnidadTipo                = x.IdUnidadTipo,
                                UnidadTipoName              = x.UnidadTipoName,
                                IdUnidadMarca               = x.IdUnidadMarca,
                                UnidadMarcaName             = x.UnidadMarcaName,
                                IdUnidadPlacaTipo           = x.IdUnidadPlacaTipo,
                                UnidadPlacaTipoName         = x.UnidadPlacaTipoName,
                                Placa                       = x.Placa,
                                NumeroSerie                 = x.NumeroSerie,
                                Modelo                      = x.Modelo,
                                AnioEquipo                  = x.AnioEquipo,
                                Capacidad                   = x.Capacidad.ToString(),
                                IdUnidadCapacidadMedida     = x.IdUnidadCapacidadMedida,
                                UnidadCapacidadMedidaName   = x.UnidadCapacidadMedidaName,
                                Odometro                    = x.Odometro.ToString(),
                                Horometro                   = x.Horometro.ToString(),
                                value                       = string.Format("No. Económico: {0}", x.NumeroEconomico),
                            })
                            .ToListAsync<dynamic>();
        }

        public async Task<List<dynamic>> ListUsuarios()
        {
            var lstUsuarios = await _context.Unidades
                                        .AsNoTracking()
                                        .Select(x => new
                                        {
                                            IdCreatedUser   = x.IdCreatedUser,
                                            CreatedUserName = x.CreatedUserName,
                                            IdUpdatedUser   = x.IdUpdatedUser,
                                            UpdatedUserName = x.UpdatedUserName,
                                        })
                                        .Distinct()
                                        .ToListAsync<dynamic>();

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

        public async Task<byte[]> Reporte(dynamic data)
        {
            IQueryable<UnidadViewModel> lstDataSource = DataSourceExpression(data);

            List<ModelExcelColumn> columnsDataTable = new List<ModelExcelColumn>();
            columnsDataTable.Add(new ModelExcelColumn());

            var lstRows = _mapper.Map<List<RepUnidad>>(lstDataSource);

            return await ExcelManager<RepUnidad>.GetFile(columnsDataTable, lstRows);
        }

        public async Task<List<dynamic>> Predictive(dynamic data)
        {
            // INCLUDES
            string fields = "IdUnidad,NumeroEconomico,NumeroSerie,IdBase,BaseName,IdUnidadTipo,UnidadTipoName,IdUnidadMarca,UnidadMarcaName,IdUnidadPlacaTipo,UnidadPlacaTipoName,Placa, Modelo,AnioEquipo,Capacidad,IdUnidadCapacidadMedida,UnidadCapacidadMedidaName,Odometro,Horometro";

            // QUERY
            var lstItems = _context.Unidades
                                    .AsNoTracking()
                                    .Where(x => !x.Deleted)
                                    .OrderBy(x => x.NumeroEconomico)
                                    .Select(Globals.BuildSelector<Unidad, Unidad>(fields));

            // INITIALIZATION
            DataSourceBuilder<Unidad> objDataSourceBuilder = new DataSourceBuilder<Unidad>();
            objDataSourceBuilder.Source     = lstItems;
            objDataSourceBuilder.Arguments  = data;

            // SEARCH FILTERS
            Func<Expression<Func<Unidad, bool>>, string, string, Expression<Func<Unidad, bool>>> argSwitchFilters = (argExpression, argField, search) => { return argExpression; };

            lstItems = objDataSourceBuilder.SearchFilters(argSwitchFilters);

            // TAKE
            lstItems = objDataSourceBuilder.Take();

            // DATA MAPPING
            var lstOriginal = await lstItems.ToListAsync();
            var lstRows     = new List<dynamic>();

            lstOriginal.ForEach((item) =>
            {
                lstRows.Add(new
                {
                    IdUnidad                    = item.IdUnidad,
                    NumeroEconomico             = item.NumeroEconomico,
                    NumeroSerie                 = item.NumeroSerie,
                    IdBase                      = item.IdBase,
                    BaseName                    = item.BaseName,
                    IdUnidadTipo                = item.IdUnidadTipo,
                    UnidadTipoName              = item.UnidadTipoName,
                    IdUnidadMarca               = item.IdUnidadMarca,
                    UnidadMarcaName             = item.UnidadMarcaName,
                    IdUnidadPlacaTipo           = item.IdUnidadPlacaTipo,
                    UnidadPlacaTipoName         = item.UnidadPlacaTipoName,
                    Placa                       = item.Placa,
                    Modelo                      = item.Modelo,
                    AnioEquipo                  = item.AnioEquipo,
                    Capacidad                   = item.Capacidad.ToString(),
                    IdUnidadCapacidadMedida     = item.IdUnidadCapacidadMedida,
                    UnidadCapacidadMedidaName   = item.UnidadCapacidadMedidaName,
                    Odometro                    = item.Odometro.ToString(),
                    Horometro                   = item.Horometro.ToString(),
                });
            });

            return lstRows;
        }

        public async Task Update(dynamic data, ClaimsPrincipal user)
        {
            var objTransaction = _context.Database.BeginTransaction();

            string idUnidad         = Globals.ParseGuid(data.idUnidad);
            string numeroEconomico  = Globals.ToUpper(data.numeroEconomico);

            // ENCONTRAR UNIDAD A ACTUALIZAR
            Unidad objModel = await Find(idUnidad);

            if (objModel == null) { throw new ArgumentException("No se encontró la unidad."); }
            if (objModel.Deleted) { throw new ArgumentException("La unidad ha sido eliminada."); }

            bool editNumeroEconomico = !string.Equals(objModel.NumeroEconomico, numeroEconomico, StringComparison.OrdinalIgnoreCase);

            if (editNumeroEconomico)
            {
                bool findUnidad = await _context.Unidades.AnyAsync(x => x.NumeroEconomico.ToUpper() == numeroEconomico && x.IdUnidad != idUnidad && !x.Deleted);

                if (findUnidad) { throw new ArgumentException("Ya existe una unidad con ese número ecónomico."); }
            }                            

            // ACTUALIZAR UNIDAD
            objModel.NumeroEconomico            = numeroEconomico;
            objModel.IdBase                     = Globals.ParseGuid(data.idBase);
            objModel.BaseName                   = Globals.ToUpper(data.baseName);
            objModel.IdUnidadTipo               = Globals.ParseGuid(data.idUnidadTipo);
            objModel.UnidadTipoName             = Globals.ToUpper(data.unidadTipoName);
            objModel.IdUnidadMarca              = Globals.ParseGuid(data.idUnidadMarca);
            objModel.UnidadMarcaName            = Globals.ToUpper(data.unidadMarcaName);
            objModel.IdUnidadPlacaTipo          = Globals.ParseGuid(data.idUnidadPlacaTipo);
            objModel.UnidadPlacaTipoName        = Globals.ToUpper(data.unidadPlacaTipoName);
            objModel.Placa                      = Globals.ToUpper(data.placa);
            objModel.NumeroSerie                = Globals.ToUpper(data.numeroSerie);
            objModel.Modelo                     = Globals.ToUpper(data.modelo);
            objModel.AnioEquipo                 = Globals.ToUpper(data.anioEquipo);
            objModel.Descripcion                = Globals.ToUpper(data.descripcion);
            objModel.Capacidad                  = Globals.ParseDecimal(data.capacidad);
            objModel.IdUnidadCapacidadMedida    = Globals.ParseGuid(data.idUnidadCapacidadMedida);
            objModel.UnidadCapacidadMedidaName  = Globals.ToUpper(data.unidadCapacidadMedidaName);
            objModel.Horometro                  = Globals.ParseInt(data.horometro)  ?? 0;
            objModel.Odometro                   = Globals.ParseInt(data.odometro)   ?? 0;
            objModel.SetUpdated(Globals.GetUser(user));

            _context.Unidades.Update(objModel);
            await _context.SaveChangesAsync();
            objTransaction.Commit();
        }
    }
}