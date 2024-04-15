using API.Inspecciones.Models;
using API.Inspecciones.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Workcube.Interfaces;
using Workcube.Libraries;

namespace API.Inspecciones.Services
{
    public class InspeccionesService : IGlobal<Inspeccion>
    {
        private readonly Context _context;

        public InspeccionesService(Context context)
        {
            _context = context;            
        }

        public async Task Cancel(dynamic data, ClaimsPrincipal user)
        {
            var objTransaction = _context.Database.BeginTransaction();

            string idInspeccion = Globals.ParseGuid(data.idInspeccion);

            // CANCELAR INSPECCIÓN
            Inspeccion objModel = await Find(idInspeccion);

            if (objModel.IdInspeccionEstatus == "ea52bdfd-8af6-4f5a-b182-2b99e554eb33") { throw new ArgumentException("La inspección ya fue cancelada anteriormente."); }

            objModel.IdInspeccionEstatus    = "ea52bdfd-8af6-4f5a-b182-2b99e554eb33";
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
            objModel.InspeccionEstatusName          = "EVALUACIÓN";
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
            objModel.AnioEquipo                     = Globals.ToString(data.anioEquipo);
            objModel.Locacion                       = Globals.ToUpper(data.locacion);
            objModel.TipoPlataforma                 = Globals.ToUpper(data.tipoPlataforma);
            objModel.Capacidad                      = Globals.ParseDecimal(data.capacidad);
            objModel.Odometro                       = Globals.ParseInt(data.horometro);
            objModel.Horometro                      = Globals.ParseInt(data.horometro);
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
            objModel.InspeccionEstatusName          = "EVALUACIÓN";
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
            objModel.AnioEquipo                     = Globals.ToString(data.anioEquipo);
            objModel.Locacion                       = Globals.ToUpper(data.locacion);
            objModel.TipoPlataforma                 = Globals.ToUpper(data.tipoPlataforma);
            objModel.Capacidad                      = Globals.ParseDecimal(data.capacidad);
            objModel.Odometro                       = Globals.ParseInt(data.horometro);
            objModel.Horometro                      = Globals.ParseInt(data.horometro);
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

        public Task<dynamic> DataSource(dynamic data, ClaimsPrincipal user)
        {
            throw new NotImplementedException();
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

            if (objModel.IdInspeccionEstatus == "ea52bdfd-8af6-4f5a-b182-2b99e554eb32") { throw new ArgumentException("La inspección ya fue finalizada anteriormente."); }
            if (objModel.IdInspeccionEstatus == "ea52bdfd-8af6-4f5a-b182-2b99e554eb33") { throw new ArgumentException("La inspección ya fue cancelada anteriormente."); }

            objModel.IdInspeccionEstatus    = "ea52bdfd-8af6-4f5a-b182-2b99e554eb32";
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
