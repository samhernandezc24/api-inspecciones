using System.Linq.Expressions;
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

            string unidadNumeroEconomico = Globals.ToUpper(data.numeroEconomico);

            bool findUnidad = await _context.Unidades.AnyAsync(x => x.NumeroEconomico.ToUpper() == unidadNumeroEconomico && !x.Deleted);
            if (findUnidad) { throw new ArgumentException("Ya existe una unidad con ese mismo número económico."); }

            // GUARDAR UNIDAD TEMPORAL
            Unidad objModel = new Unidad();
            objModel.IdUnidad           = Guid.NewGuid().ToString();
            objModel.NumeroEconomico    = unidadNumeroEconomico;
            objModel.NumeroSerie        = Globals.ToUpper(data.numeroSerie);
            objModel.Descripcion        = Globals.ToUpper(data.descripcion);
            objModel.Modelo             = Globals.ToUpper(data.modelo);
            objModel.AnioEquipo         = Globals.ToUpper(data.anioEquipo);
            objModel.IdBase             = Globals.ParseGuid(data.idBase);
            objModel.BaseName           = Globals.ToUpper(data.baseName);
            objModel.IdUnidadMarca      = Globals.ParseGuid(data.idUnidadMarca);
            objModel.UnidadMarcaName    = Globals.ToUpper(data.unidadMarcaName);
            objModel.IdUnidadTipo       = Globals.ParseGuid(data.idUnidadTipo);
            objModel.UnidadTipoName     = Globals.ToUpper(data.unidadTipoName);
            objModel.Capacidad          = Globals.ParseDecimal(data.capacidad) ?? 0;
            objModel.Horometro          = Globals.ParseInt(data.horometro) ?? 0;
            objModel.Odometro           = Globals.ParseInt(data.odometro) ?? 0;
            objModel.SetCreated(Globals.GetUser(user));

            _context.Unidades.Add(objModel);

            await _context.SaveChangesAsync();
            objTransaction.Commit();
        }

        public async Task<dynamic> DataSource(dynamic data, ClaimsPrincipal user)
        {
            IQueryable<UnidadViewModel> lstItems = DataSourceExpression(data);

            DataSourceBuilder<UnidadViewModel> objDataTableBuilder = new DataSourceBuilder<UnidadViewModel>(data, lstItems);

            dynamic objDataTableResult = await objDataTableBuilder.build();

            List<UnidadViewModel> lstOriginal = objDataTableResult.rows;
            List<dynamic> lstRows = new List<dynamic>();

            int length  = (int)data.length;
            int page    = (int)data.page;
            int index   = ((page - 1) * length) + 1;

            lstOriginal.ForEach(item =>
            {
                lstRows.Add(new
                {
                    index               = index,
                    IdUnidad            = item.IdUnidad,
                    NumeroEconomico     = item.NumeroEconomico,
                    NumeroSerie         = item.NumeroSerie,
                    Descripcion         = item.Descripcion,
                    Modelo              = item.Modelo,
                    AnioEquipo          = item.AnioEquipo,
                    UnidadMarcaName     = item.UnidadMarcaName,
                    UnidadTipoName      = item.UnidadTipoName,
                    Capacidad           = item.Capacidad,
                    CreatedUserName     = item.CreatedUserName,
                    CreatedFecha        = item.CreatedFechaNatural,
                    UpdatedUserName     = item.UpdatedUserName,
                    UpdatedFecha        = item.UpdatedFechaNatural,
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

            // APLICAR FILTROS DINÁMICOS
            // FILTROS
            var filters = new Dictionary<string, Func<string, Expression<Func<Unidad, bool>>>>
            {
                {"IdCreatedUser",   (strValue) => item => item.IdCreatedUser    == strValue},
                {"IdUpdatedUser",   (strValue) => item => item.IdUpdatedUser    == strValue},
            };

            // FILTROS MULTIPLE
            var filtersMultiple = new Dictionary<string, Func<string, Expression<Func<Unidad, bool>>>>
            {
                {"IdUnidadMarca",   (strValue) => item => item.IdUnidadMarca    == strValue },
                {"IdUnidadTipo",    (strValue) => item => item.IdUnidadTipo     == strValue },
            };

            // FILTROS FECHAS
            DateTime? dateFrom  = SourceExpression<Unidad>.Date((string)data.dateFrom);
            DateTime? dateTo    = SourceExpression<Unidad>.Date((string)data.dateTo);

            var dates = new Dictionary<string, DateExpression<Unidad>>()
            {
                { "CreatedFecha",   new DateExpression<Unidad>{ dateFrom = item => item.CreatedFecha.Date  >= dateFrom, dateTo = item => item.CreatedFecha.Date    <= dateTo } },
                { "UpdatedFecha",   new DateExpression<Unidad>{ dateFrom = item => item.UpdatedFecha.Date  >= dateFrom, dateTo = item => item.UpdatedFecha.Date    <= dateTo } }
            };

            Expression<Func<Unidad, bool>> ExpFullWhere = SourceExpression<Unidad>.GetExpression(data, filters, dates, filtersMultiple);

            // ORDER BY
            var orderColumn     = Globals.ToString(data.sort.column);
            var orderDirection  = Globals.ToString(data.sort.direction);

            Expression<Func<Unidad, object>> sortExpression;
            switch (orderColumn)
            {
                case "numeroEconomico"      : sortExpression = (x => x.NumeroEconomico);        break;
                case "numeroSerie"          : sortExpression = (x => x.NumeroSerie);            break;
                case "descripcion"          : sortExpression = (x => x.Descripcion);            break;
                case "modelo"               : sortExpression = (x => x.Modelo);                 break;
                case "anioEquipo"           : sortExpression = (x => x.AnioEquipo);             break;
                case "unidadMarcaName"      : sortExpression = (x => x.UnidadMarcaName);        break;
                case "unidadTipoName"       : sortExpression = (x => x.UnidadTipoName);         break;
                case "capacidad"            : sortExpression = (x => x.Capacidad);              break;
                case "createdUserName"      : sortExpression = (x => x.CreatedUserName);        break;
                case "createdFechaNatural"  : sortExpression = (x => x.CreatedFechaNatural);    break;
                case "updatedUserName"      : sortExpression = (x => x.UpdatedUserName);        break;
                case "updatedFechaNatural"  : sortExpression = (x => x.UpdatedFechaNatural);    break;
                default                     : sortExpression = (x => x.CreatedFecha);           break;
            }

            // MAPEAR DATOS
            List<string> columns = new List<string>();

            columns = Globals.GetArrayColumns(data);

            columns.Add("IdUnidad");
            columns.Add("NumeroEconomico");
            columns.Add("NumeroSerie");
            columns.Add("Descripcion");
            columns.Add("Modelo");
            columns.Add("AnioEquipo");
            columns.Add("UnidadMarcaName");
            columns.Add("UnidadTipoName");
            columns.Add("Capacidad");
            columns.Add("CreatedUserName");
            columns.Add("CreatedFecha");
            columns.Add("UpdatedUserName");
            columns.Add("UpdatedFecha");

            string strColumns = Globals.GetStringColumns(columns);

            // COMPLETE
            IQueryable<Unidad> lstRows = _context.Unidades.AsNoTracking();

            lstRows = (orderDirection == "asc") ? lstRows.OrderBy(sortExpression) : lstRows.OrderByDescending(sortExpression);

            lstItems = lstRows
                .Where(x => !x.Deleted)
                .Where(ExpFullWhere)
                .Select(Globals.BuildSelector<Unidad, Unidad>(strColumns))
                .ProjectTo<UnidadViewModel>(_mapper.ConfigurationProvider);

            return lstItems;
        }

        public Task Delete(dynamic data, ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }

        public async Task<Unidad> Find(string id)
        {
            return await _context.Unidades.FindAsync(id);
        }

        public async Task<Unidad> FindSelectorById(string id, string fields)
        {
            return await _context.Unidades.AsNoTracking().Where(x => x.IdUnidad == id)
                        .Select(Globals.BuildSelector<Unidad, Unidad>(fields)).FirstOrDefaultAsync();
        }

        public async Task<List<dynamic>> List()
        {
            return await _context.Unidades
                .AsNoTracking()
                .Where(x => !x.Deleted)
                .OrderBy(x => x.NumeroEconomico)
                .Select(x => new
                {
                    IdUnidad            = x.IdUnidad,
                    NumeroEconomico     = x.NumeroEconomico,
                    NumeroSerie         = x.NumeroSerie,
                    Modelo              = x.Modelo,
                    AnioEquipo          = x.AnioEquipo,
                    IdUnidadMarca       = x.IdUnidadMarca,
                    UnidadMarcaName     = x.UnidadMarcaName,
                    IdUnidadTipo        = x.IdUnidadTipo,
                    UnidadTipoName      = x.UnidadTipoName,
                    Capacidad           = x.Capacidad,
                })
                .ToListAsync<dynamic>();
        }

        public async Task<List<dynamic>> ListUsuarios()
        {
            var lstUsuarios = await _context.Unidades
                .AsNoTracking()
                .Select(x => new
                {
                    IdCreatedUser       = x.IdCreatedUser,
                    CreatedUserName     = x.CreatedUserName,
                    IdUpdatedUser       = x.IdUpdatedUser,
                    UpdatedUserName     = x.UpdatedUserName,
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
            string fields = "IdUnidad,NumeroEconomico,NumeroSerie,Descripcion,Modelo,AnioEquipo,IdUnidadMarca,UnidadMarcaName,IdUnidadTipo,UnidadTipoName";

            // QUERY
            var lstItems = _context.Unidades
                .AsNoTracking()
                .OrderBy(w => w.NumeroEconomico)
                .Where(x => !x.Deleted)
                .Select(Globals.BuildSelector<Unidad, Unidad>(fields));

            // INITIALIZATION
            DataSourceBuilder<Unidad> objDataSourceBuilder = new DataSourceBuilder<Unidad>();
            objDataSourceBuilder.Source     = lstItems;
            objDataSourceBuilder.Arguments  = data;

            // SEARCH FILTERS
            Func<Expression<Func<Unidad, bool>>, string, string, Expression<Func<Unidad, bool>>> argSwitchFilters = (argExpression, argField, search) =>
            {
                return argExpression;
            };

            objDataSourceBuilder.SearchFilters(argSwitchFilters);

            // TAKE
            lstItems = objDataSourceBuilder.Take();

            // DATA MAPPING
            var lstOriginal = await lstItems.ToListAsync();
            var lstRows = new List<dynamic>();

            if (lstOriginal != null)
            {
                lstOriginal.ForEach(item =>
                {
                    lstRows.Add(new
                    {
                        IdUnidad            = item.IdUnidad,
                        NumeroEconomico     = item.NumeroEconomico,
                        NumeroSerie         = item.NumeroSerie,
                        Descripcion         = item.Descripcion,
                        Modelo              = item.Modelo,
                        AnioEquipo          = item.AnioEquipo,
                        IdUnidadMarca       = item.IdUnidadMarca,
                        UnidadMarcaName     = item.UnidadMarcaName,
                        IdUnidadTipo        = item.IdUnidadTipo,
                        UnidadTipoName      = item.UnidadTipoName,
                        IsUnidadTemporal    = true,
                    });
                });
            }

            return lstRows;
        }

        public Task Update(dynamic data, ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }
    }
}
