using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using MultipartFormDataSample.Server.Models;

namespace MultipartFormDataSample.Server.Controllers
{
    public class SendApiController : ApiController
    {
        [Route("api/send")]
        public string UploadImageSet(ImageSet model)
        {
            var sb = new StringBuilder();
            
            sb.AppendFormat("Received image set {0}: ", model.Name);
            model.Images.ForEach(i =>
                sb.AppendFormat("Got image {0} of type {1} and size {2} bytes,", i.FileName, i.MimeType,
                    i.ImageData.Length)
                );

            var result = sb.ToString();
            Trace.Write(result);

            return result;

        }
    }
}
