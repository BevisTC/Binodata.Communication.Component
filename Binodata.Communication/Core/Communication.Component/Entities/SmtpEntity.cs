using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Component.Entities
{
    public class SmtpEntity
    {
        public bool EnableSsl { get; set; }
        public string Host { get; set; }
        public SecureString Password { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
    }
}
