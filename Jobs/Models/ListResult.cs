using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jobs.Models
{
    public class ListResult
    {
        public Job Jobdata { get; set; }
        public Recument Recumentdata { get; set; }
        public CV CVdata { get; set; }
    }
}