﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.Client.Entity.StoreEntitys
{
    public class GetGamesEntity
    {
        public int id { get; set; }
        public string title { get; set; }
        public string pict { get; set; }
        public object logo { get; set; }
        public float price { get; set; }
        public string cateName { get; set; }
        public int displayOrder { get; set; }
        public string fileDir { get; set; }
    }
   

}
