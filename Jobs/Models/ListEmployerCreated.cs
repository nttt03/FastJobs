using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace Jobs.Models
{
    public class ListEmployerCreated
    {
        public string name { get; set; }
        public string nameJob { get; set; }
        public long money { get; internal set; }
        public int id { get; internal set; }
        public int idJob { get; internal set; }
        public DateTime date { get; internal set; }

    }
}