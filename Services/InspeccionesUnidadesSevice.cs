using API.Inspecciones.Interfaces;
using API.Inspecciones.Models;
using API.Inspecciones.Persistence;
using System.Data.Entity;
using System.Security.Claims;
using Workcube.Libraries;

namespace API.Inspecciones.Services
{
    public class InspeccionesUnidadesService : IInspeccionesUnidadesService
    {
        private readonly Context _context;

        public InspeccionesUnidadesService(Context context)
        {
            _context = context;
        }

        public async Task Create(dynamic data, ClaimsPrincipal user)
        {
            if (!await HttpReq.GetPrivilegio("INSPECCIONES_UNIDADES_CREATE", user)) { throw new AppException(ExceptionMessage.SESSION_003); };

            var objTransaction  = _context.Database.BeginTransaction();

            // GUARDAR INSPECCION UNIDAD
            InspeccionUnidad objModel = new InspeccionUnidad();

            objModel.IdInspeccionUnidad     = Guid.NewGuid().ToString();
            objModel.IdBase                 = Globals.ParseGuid(data.idBase);
            objModel.IdBase                 = Globals.ToString(data.baseName);
            objModel.IdUnidad               = Globals.ParseGuid(data.idUnidad);
            objModel.UnidadNumeroEconomico  = Globals.ToString(data.unidadNumeroEconomico);
            objModel.Folio                  = Globals.ToString(data.folio);
            objModel.IsUnidadTemporal       = Globals.ParseBool(data.isUnidadTemporal);
            objModel.Fecha                  = Globals.DateTime(data.fecha);
            objModel.IdInspeccion           = Globals.ParseGuid(data.idInspeccion);
            objModel.InspeccionFolio        = Globals.ToString(data.inspeccionFolio);
            objModel.InspeccionName         = Globals.ToString(data.inspeccionName);
            objModel.IdRequerimiento        = Globals.ParseGuid(data.idRequerimiento);
            objModel.RequerimientoFolio     = Globals.ToString(data.requerimientoFolio);
            objModel.InspeccionName         = Globals.ToString(data.inspeccionName);
            objModel.TipoPlataforma         = Globals.ToString(data.tipoPlataforma);
            objModel.NumeroSerie            = Globals.ToString(data.numeroSerie);
            objModel.Marca                  = Globals.ToString(data.marca);
            objModel.Modelo                 = Globals.ToString(data.modelo);
            objModel.Horometro              = Globals.ParseInt(data.horometro);
            objModel.Odometro               = Globals.ParseInt(data.odometro);
            objModel.Locacion               = Globals.ToString(data.locacion);
            objModel.Capacidad              = Globals.ParseInt(data.capacidad);
            objModel.Observaciones          = Globals.ToString(data.observaciones);
            objModel.FirmaOperador          = Globals.ToString(data.firmaOperador);
            objModel.FirmaVerificador       = Globals.ToString(data.firmaVerificador);
            objModel.SetCreated(Globals.GetUser(user));

            _context.InspeccionesUnidades.Add(objModel);
            await _context.SaveChangesAsync();
            objTransaction.Commit();
        }

        public Task<dynamic> DataSource(dynamic data, ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }

        public Task Delete(dynamic data, ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }

        public async Task<InspeccionUnidad> Find(string id)
        {
            return await _context.InspeccionesUnidades.FindAsync(id);
        }

        public async Task<InspeccionUnidad> FindSelectorById(string id, string fields)
        {
            return await _context.InspeccionesUnidades.AsNoTracking().Where(x => x.IdInspeccionUnidad == id)
                            .Select(Globals.BuildSelector<InspeccionUnidad, InspeccionUnidad>(fields)).FirstOrDefaultAsync();
        }

        public async Task<List<dynamic>> FindLastInspeccionByIds(List<string> lstIds)
        {
            var lstResult = await _context.InspeccionesUnidades
                                .AsNoTracking()
                                .Where(x => lstIds.Contains(x.IdUnidad) && !x.Deleted)
                                .OrderByDescending(x => x.Fecha.Date)
                                .ToListAsync();

            var lstGroup = lstResult.GroupBy(x => x.IdUnidad, x => x, (key, data) => new
            {
                key     = key,
                data    = data,
            }).Select(x => new
            {
                IdUnidad    = x.key,
                Inspeccion  = x.data.FirstOrDefault(),
                IsValid     = x.data.FirstOrDefault().IsValid,
            }).ToList<dynamic>();

            return lstGroup;
        }

        public Task<List<dynamic>> List()
        {
            throw new NotImplementedException();
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
