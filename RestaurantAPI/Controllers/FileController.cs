using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Controllers
{
    [Route("file")]
    [Authorize]
    public class FileController : ControllerBase
    {
        public ActionResult GetFile([FromQuery]string fileName)
        {
            var filePath = $"{Directory.GetCurrentDirectory()}/PrivateFiles/{fileName}";

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var contentProvider = new FileExtensionContentTypeProvider();
            contentProvider.TryGetContentType(fileName, out string contentType);

            return File(System.IO.File.ReadAllBytes(filePath), contentType, fileName);
        }
    }
}
