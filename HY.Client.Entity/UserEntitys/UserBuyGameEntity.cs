using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.Client.Entity.UserEntitys
{
    public class UserBuyGameEntity
    {
        public int id { get; set; }
        public string phone { get; set; }
        public string passWord { get; set; }
        public string salt { get; set; }
        public int state { get; set; }
        public int loginTime { get; set; }
        public int loginNum { get; set; }
        public int vipType { get; set; }
        public float balance { get; set; }
        public int freeCount { get; set; }
        public int vipValidTo { get; set; }
        public string loginMacAdd { get; set; }
        public int createTime { get; set; }
        public int updateTime { get; set; }

        public string vipInfo { get; set; }
    }

  

}
