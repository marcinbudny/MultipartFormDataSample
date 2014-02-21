using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using MultipartFormDataSample.Server.Models;

namespace MultipartFormDataSample.Server.Controllers
{
    public class SendApiController : ApiController
    {
        [Route("api/send")]
        public async Task<HttpResponseMessage> SendWithJson()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = await Request.Content.ReadAsMultipartAsync();

            var modelContent = provider.Contents
                .FirstOrDefault(c => c.Headers.ContentDisposition.Name == "model");
            var model = await modelContent.ReadAsAsync<SampleModel>();

            var fileContents = provider.Contents
                .Where(c => c.Headers.ContentDisposition.Name == "files")
                .ToList();

            fileContents.ForEach(async fc =>
            {
                var fileBytes = await fc.ReadAsByteArrayAsync();
                var fileType = fc.Headers.ContentType.MediaType;
                var fileName = fc.Headers.ContentDisposition.FileName;

                Trace.WriteLine(string.Format("Got image {0} of type {1} and size {2} bytes", fileName, fileType, fileBytes.Length));
            });
            
            return Request.CreateResponse(HttpStatusCode.OK);

        }
    }
}
