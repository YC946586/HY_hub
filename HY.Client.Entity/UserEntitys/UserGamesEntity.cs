using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.Client.Entity.UserEntitys
{
    public class UserGamesEntity
    {
        public int id { get; set; }
        public int gameId { get; set; }
        public int userId { get; set; }
        public int isInOften { get; set; }
        public int downLoadCount { get; set; }
        public object downMacAdd { get; set; }
        public int createTime { get; set; }
        public int updateTime { get; set; }
        public string title { get; set; }
        public string fileDir { get; set; }
        public string pict { get; set; }
        public object logo { get; set; }
        public float price { get; set; }
        public string cateName { get; set; }
        public string startFileName { get; set; }
        public string setUpFile { get; set; }
        public int fileSize { get; set; }
        public object version { get; set; }
    }

     

}
