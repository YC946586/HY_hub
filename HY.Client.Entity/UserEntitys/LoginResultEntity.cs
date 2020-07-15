using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.Client.Entity.UserEntitys
{
    [Serializable]
    public class LoginResultEntity
    {
        public int id { get; set; }
        public string phone { get; set; }
        public int state { get; set; }
        public int vipType { get; set; }
        public float balance { get; set; }
        public int freeCount { get; set; }
        public object vipValidTo { get; set; }
        public string token { get; set; }

    }
}
