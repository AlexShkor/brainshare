using System;
using System.Net.Mime;
using System.Web.Mvc;

namespace Brainshare.Infrastructure.Platform.Mvc
{
    public class ViewInBrowserFileResult : FileContentResult
    {
        public ViewInBrowserFileResult(byte[] fileContents, string contentType, string fileName, bool inline = true)
            : base(fileContents, contentType)
        {
            FileDownloadName = fileName;
            Inline = inline;
        }

        public bool Inline { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            var response = context.HttpContext.Response;
            response.ContentType = ContentType;
            if (!string.IsNullOrEmpty(FileDownloadName))
            {
                var str = new ContentDisposition { FileName = FileDownloadName, Inline = Inline }.ToString();
                context.HttpContext.Response.AddHeader("Content-Disposition", str);
            }
            WriteFile(response);
        }
    }
}
