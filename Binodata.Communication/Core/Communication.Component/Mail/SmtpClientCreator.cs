using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Communication.Component.Entities;
using System.Net;

namespace Communication.Component.Mail
{
    public class SmtpClientCreator
    {
        public static SmtpClient CreateSmtpClient(SmtpEntity entity)
        {
            SmtpClient client = new SmtpClient(entity.Host,entity.Port);
            client.EnableSsl = entity.EnableSsl;
            client.Credentials = new NetworkCredential(entity.UserName, entity.Password);

            return client;
        }
    }
}
