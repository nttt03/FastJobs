using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jobs.Models
{
    public class JobUser
    {
        public Job jobdata { get; set; }
        public Company companydata { get; set; }
        public JobCategory jobcategorydata { get; set; }

        public Recument recumentdata { get; set; }
        public Career careerdata { get; set; }
        public EmployerCreatedCompany createdCompanydata { get; set; }
    }
}