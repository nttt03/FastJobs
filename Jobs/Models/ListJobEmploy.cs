using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jobs.Models
{
    public class ListJobEmploy
    {
        public string name { get; set; }
        public string nameCom { get; set; }
        public int id { get; internal set; }
        public int idCom { get; internal set; }
    }
}