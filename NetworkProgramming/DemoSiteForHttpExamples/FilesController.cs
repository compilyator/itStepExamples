using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace DemoSiteForHttpExamples
{
    [RoutePrefix("files")]
    public class FilesController : Controller
    {
        private readonly string _imagesPath;
        private readonly Random _random;
        public FilesController()
        {
            _imagesPath = HostingEnvironment.MapPath("~/Images");
            _random = new Random();
        }

        [Route("count")]
        public ActionResult Count()
        {
            var files = Directory.GetFiles(_imagesPath);
            return Content(files.Length.ToString());
        }

        [Route("names")]
        public ActionResult Names()
        {
            var files = Directory.GetFiles(_imagesPath);
            var builder = new StringBuilder();
            foreach (var file in Enumerable.Range(1, files.Count()))
            {
                builder.AppendLine(Url.Action("Image", "Files", new {number = file}, "http"));
            }
            return Content(builder.ToString());
        }

        [Route("image/{number}")]
        public ActionResult Image(int number)
        {
            var fileName = $"{number:D2}.jpg";
            var fullFileName = $"{_imagesPath}\\{fileName}";
            Thread.Sleep(_random.Next(1000));
            if (!System.IO.File.Exists(fullFileName))
            {
                return HttpNotFound();
            }
            return File(fullFileName, MimeMapping.GetMimeMapping(Path.GetFileName(fileName)), fileName);
        }
    }
}