namespace DemoSiteForHttpExamples
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Web;
    using System.Web.Hosting;
    using System.Web.Mvc;

    [RoutePrefix("files")]
    public class FilesController : Controller
    {
        private readonly string _imagesPath;

        private readonly Random _random;

        public FilesController()
        {
            this._imagesPath = HostingEnvironment.MapPath("~/Images");
            this._random = new Random();
        }

        [Route("count")]
        public ActionResult Count()
        {
            var files = Directory.GetFiles(this._imagesPath);
            return this.Content(files.Length.ToString());
        }

        [Route("image/{number}")]
        public ActionResult Image(int number)
        {
            var fileName = $"{number:D2}.jpg";
            var fullFileName = $"{this._imagesPath}\\{fileName}";
            Thread.Sleep(this._random.Next(1000));
            if (!System.IO.File.Exists(fullFileName))
            {
                return this.HttpNotFound();
            }

            return this.File(fullFileName, MimeMapping.GetMimeMapping(Path.GetFileName(fileName)), fileName);
        }

        [Route("names")]
        public ActionResult Names()
        {
            var files = Directory.GetFiles(this._imagesPath);
            var builder = new StringBuilder();
            foreach (var file in Enumerable.Range(1, files.Count()))
            {
                builder.AppendLine(this.Url.Action("Image", "Files", new { number = file }, "http"));
            }

            return this.Content(builder.ToString());
        }
    }
}