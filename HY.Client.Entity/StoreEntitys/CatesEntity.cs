using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.Client.Entity.StoreEntitys
{
    public class CatesEntity
    {
        public string id { get; set; }
        public string name { get; set; }
        public object shortName { get; set; }
        public int isActive { get; set; }
        public int displayOrder { get; set; }
        public object createTime { get; set; }
        public object updateTime { get; set; }
    }
}

