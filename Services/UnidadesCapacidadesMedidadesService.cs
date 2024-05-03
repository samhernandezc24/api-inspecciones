using API.Inspecciones.Persistence;
using Microsoft.EntityFrameworkCore;

namespace API.Inspecciones.Services
{
    public class UnidadesCapacidadesMedidadesService
    {
        private readonly Context _context;

        public UnidadesCapacidadesMedidadesService(Context context)
        {
            _context = context;
        }

        public async Task<List<dynamic>> List()
        {
            return await _context.UnidadesCapacidadesMedidas
                            .AsNoTracking()
                            .OrderBy(x => x.Name)
                            .Where(x => !x.Deleted)
                            .Select(x => new
                            {
                                IdUnidadCapacidadMedida = x.IdUnidadCapacidadMedida,
                                Name                    = x.Name,
                            })
                            .ToListAsync<dynamic>();
        }
    }
}
