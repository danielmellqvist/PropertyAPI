using Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Initializer
{
    public class JsonReader
    {
        public static List<object> DeserializeObject(string FilePath, object obj, List<object> objs)
        {
            string json = File.ReadAllText(FilePath);
            objs = JsonConvert.DeserializeObject<List<object>>(json);

            return objs;

        }



    }

}
