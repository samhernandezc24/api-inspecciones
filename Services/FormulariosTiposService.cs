﻿using API.Inspecciones.Persistence;
using System.Data.Entity;

namespace API.Inspecciones.Services
{
    public class FormulariosTiposService
    {
        private readonly Context _context;

        public FormulariosTiposService(Context context)
        {
            _context = context;
        }
        
        public async Task<List<dynamic>> List()
        {
            return await _context.FormulariosTipos
                                 .AsNoTracking() 
                                 .OrderBy(x => x.Orden)
                                 .Where(x => !x.Deleted)
                                 .Select(x => new
                                 {
                                     IdFormularioTipo   = x.IdFormularioTipo,
                                     Name               = x.Name,
                                     Descripcion        = x.Descripcion
                                 })
                                 .ToListAsync<dynamic>();
        }
    }
}
