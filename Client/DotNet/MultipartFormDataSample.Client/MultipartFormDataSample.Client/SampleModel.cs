using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipartFormDataSample.Client
{
    public class SampleModel
    {
        public string ModelName { get; set; }

        public List<SampleItem> Items { get; set; }
    }

    public class SampleItem
    {
        public string ItemName { get; set; }

        public int ItemCount { get; set; }
    }
}
