using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MultipartFormDataSample.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var content = new MultipartFormDataContent();

            foreach (var path in Directory.EnumerateFiles("../../../../../SampleImages"))
            {
                content.Add(new StreamContent(File.Open(path, FileMode.Open)), "files", Path.GetFileName(path));                
            }

            var model = new SampleModel
            {
                ModelName = "Model",
                Items = new List<SampleItem>
                {
                    new SampleItem {ItemName = "Item1", ItemCount = 1},
                    new SampleItem {ItemName = "Item2", ItemCount = 2}
                }
            };

            var json = JsonConvert.SerializeObject(model);
            content.Add(new StringContent(json, Encoding.UTF8, "application/json"), "model");

            var client = new HttpClient();
            var result = client.PostAsync("http://localhost:53908/Home/SendWithJson", content);
            
            Debug.WriteLine(result.Result.ToString());
        }
    }
}
