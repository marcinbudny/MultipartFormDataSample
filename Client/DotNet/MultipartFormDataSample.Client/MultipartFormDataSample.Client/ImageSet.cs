using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MultipartFormDataSample.Client
{
    public class ImageSet
    {
        public string Name { get; set; }

        [JsonIgnore]
        public List<Image> Images { get; set; }
    }

    public class Image
    {
        public string FileName { get; set; }

        public string MimeType { get; set; }

        public byte[] ImageData { get; set; }
    }
}
