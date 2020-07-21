using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.Client.Entity.HomeEntitys
{


    public class GetHomeResultEntity
    {
        public Hotgame[] hotGames { get; set; }
        public Recommendgame[] recommendGames { get; set; }
    }

    public class Hotgame
    {
        public int id { get; set; }
        public string title { get; set; }
        public string pict { get; set; }
        public object logo { get; set; }
        public float price { get; set; }
        public string cateName { get; set; }
        public int displayOrder { get; set; }
        public string fileDir { get; set; }
        public int Sort { get; set; }
        public bool isPurchased { get; set; }
        public string Content { get; set; } = "获取游戏";
    }

    public class Recommendgame
    {
        public int id { get; set; }
        public string title { get; set; }
        public string pict { get; set; }
        public object logo { get; set; }
        public float price { get; set; }
        public string cateName { get; set; }
        public int displayOrder { get; set; }
        public string fileDir { get; set; }
        public bool isPurchased { get; set; }
        public string Content { get; set; } = "获取游戏";
    }
}
