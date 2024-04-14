using System.Security.Claims;
using API.Inspecciones.Models;
using API.Inspecciones.Persistence;
using Workcube.Libraries;

namespace API.Inspecciones.Services
{
    public class InspeccionesFicherosService
    {
        private readonly Context _context;
        private readonly IWebHostEnvironment _root;

        public InspeccionesFicherosService(Context context, IWebHostEnvironment root)
        {
            _context    = context;
            _root       = root;
        }

        public async Task Create(dynamic data, ClaimsPrincipal user)
        {
            string fileBase64       = Globals.ToString(data.fileBase64);
            string fileExtension    = "." + Globals.ToString(data.fileExtension);
            string filePath         = FileManager.GetNamePath(fileExtension);

            var objTransaction = _context.Database.BeginTransaction();

            // GUARDAR FOTOGRAFÍAS DE INSPECCIÓN
            InspeccionFichero objModel = new InspeccionFichero();
            objModel.IdInspeccionFichero    = Guid.NewGuid().ToString();
            objModel.IdInspeccion           = Globals.ParseGuid(data.idInspeccion);
            objModel.InspeccionFolio        = Globals.ToUpper(data.inspeccionFolio);
            objModel.Path                   = filePath;
            objModel.SetCreated(Globals.GetUser(user));

            _context.InspeccionesFicheros.Add(objModel);
            await _context.SaveChangesAsync();

            string directory = _root.ContentRootPath + "\\Ficheros\\InspeccionesFicheros\\";

            if (!FileManager.ValidateExtension(fileExtension)) { throw new AppException(ExceptionMessage.CAST_002); }

            FileManager.ValidateDirectory(directory);

            await FileManager.SaveFileBase64(fileBase64, directory + objModel.Path);

            objTransaction.Commit();
        }
    }
}
