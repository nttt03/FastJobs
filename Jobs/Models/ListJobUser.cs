using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jobs.Models
{
    public class ListJobUser
    {
        
        public string avatar { get; set; }
        public string name { get; set; }
        public string nameJob { get; set; }
        public string location { get; set; }
        public decimal? salaryMin { get; set; }
        public decimal? salaryMaX { get; set; }
        public string typeJob { get; set; }
        public int id { get; internal set; }
    }
}