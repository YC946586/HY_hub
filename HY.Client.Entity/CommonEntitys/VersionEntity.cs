using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.Client.Entity.CommonEntitys
{
    public class Attach
    {
        public int id { get; set; }
        public string fileUrl { get; set; }
    }
    public class VersionEntity
    {
        public int id { get; set; }
        public string version { get; set; }
        public string comment { get; set; }
        public int createTime { get; set; }
        public Attach[] attaches { get; set; }
    }

   

}
