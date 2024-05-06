using API.Inspecciones.Persistence;
using Microsoft.EntityFrameworkCore;

namespace API.Inspecciones.Services
{
    public class InspeccionesEstatusService
    {
        private readonly Context _context;

        public InspeccionesEstatusService(Context context)
        {
            _context = context;
        }

        public async Task<List<dynamic>> List()
        {
            return await _context.InspeccionesEstatus
                            .AsNoTracking()
                            .OrderBy(x => x.Orden)
                            .Where(x => !x.Deleted)
                            .Select(x => new
                            {
                                IdInspeccionEstatus     = x.IdInspeccionEstatus,
                                Name                    = x.Name,
                            })
                            .ToListAsync<dynamic>();
        }
    }
}
