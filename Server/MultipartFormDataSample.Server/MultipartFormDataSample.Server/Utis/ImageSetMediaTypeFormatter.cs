using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using MultipartFormDataSample.Server.Models;

namespace MultipartFormDataSample.Server.Utis
{
    public class ImageSetMediaTypeFormatter : MediaTypeFormatter
    {
        public ImageSetMediaTypeFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("multipart/form-data"));
        }
        
        public override bool CanReadType(Type type)
        {
            return type == typeof (ImageSet);
        }

        public override bool CanWriteType(Type type)
        {
            return false;
        }

        public async override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            var provider = await content.ReadAsMultipartAsync();

            var modelContent = provider.Contents
                .FirstOrDefault(c => c.Headers.ContentDisposition.Name == "imageset");
            
            var imageSet = await modelContent.ReadAsAsync<ImageSet>();

            var fileContents = provider.Contents
                .Where(c => c.Headers.ContentDisposition.Name == "images")
                .ToList();

            imageSet.Images = new List<Image>();
            foreach (var fileContent in fileContents)
            {
                imageSet.Images.Add(new Image
                {
                    ImageData = await fileContent.ReadAsByteArrayAsync(),
                    MimeType = fileContent.Headers.ContentType.MediaType,
                    FileName = fileContent.Headers.ContentDisposition.FileName
                });
            }

            return imageSet;

        }
    }
}