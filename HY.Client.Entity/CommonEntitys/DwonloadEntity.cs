using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.Client.Entity.CommonEntitys
{
   public  class DwonloadEntity
    {
        public int id { get; set; }
        public int gameId { get; set; }
        public string url { get; set; }
        public string name { get; set; }
        public long size { get; set; }
        public int createTime { get; set; }

        public long SurplusSize { get; set; }
    }

  

}
