using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPITest.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string location { get; set; }
    }

    public class Company_rep
    {
        public static IQueryable<Company> list = null;
        static Company_rep()
        {
            
          var  listcomp = new List<Company>
            {
                new Company { Id = 1, location = "kolkata", name = "TCS" },
                new Company { Id = 2, location = "Delhi", name = "Wipro" },
                new Company { Id = 3, location = "Bangalore", name = "IBM" }
            };
            list = listcomp.AsQueryable();
        }
    }
}