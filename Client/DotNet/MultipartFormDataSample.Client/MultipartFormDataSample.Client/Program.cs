using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MultipartFormDataSample.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var imageSet = new ImageSet()
            {
                Name = "Model",
                Images = Directory
                    .EnumerateFiles("../../../../../SampleImages")
                    .Where(file => new[] {".jpg", ".png"}.Contains(Path.GetExtension(file)))
                    .Select(file => new Image
                    {
                        FileName = Path.GetFileName(file),
                        MimeType = MimeMapping.GetMimeMapping(file),
                        ImageData = File.ReadAllBytes(file)
                    })
                    .ToList()
            };

            SendImageSet(imageSet);
        }

        private static void SendImageSet(ImageSet imageSet)
        {
            var multipartContent = new MultipartFormDataContent();

            var imageSetJson = JsonConvert.SerializeObject(imageSet, 
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

            multipartContent.Add(new StringContent(imageSetJson, Encoding.UTF8, "application/json"), "imageset");

            int counter = 0;
            foreach (var image in imageSet.Images)
            {
                var imageContent = new ByteArrayContent(image.ImageData);
                imageContent.Headers.ContentType = new MediaTypeHeaderValue(image.MimeType);
                multipartContent.Add(imageContent, "image" + counter++, image.FileName);
            }

            var response = new HttpClient()
                .PostAsync("http://localhost:53908/api/send", multipartContent)
                .Result;

            var responseContent = response.Content.ReadAsStringAsync().Result;
            Trace.Write(responseContent);
        }

    }
}
