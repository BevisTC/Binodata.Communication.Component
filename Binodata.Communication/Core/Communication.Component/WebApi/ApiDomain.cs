using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Component.WebApi
{
    public  class ApiDomain
    {

        public  string Domain { get; set; }

        public ApiDomain()
        {
            Domain = "http://localhost";
        }

      

        public  string Combine(string url)
        {
            return $"{Domain}/{url}";
        }
    }
}
