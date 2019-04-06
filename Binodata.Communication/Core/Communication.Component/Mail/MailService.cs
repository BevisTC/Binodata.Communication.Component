using Communication.Component.Entities;
using Communication.Component.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Component.Mail
{
    public class MailService : ISendMessageService
    {
        private SmtpClient client = null;
        private MailMessage mailEntity;


        public MailService(SmtpClient client)
        {
            this.client = client;
        }


        public MailService(SmtpClient client, MailMessage mailEntity)
        {
            this.client = client;
            this.mailEntity = mailEntity;
        }

        public void Send()
        {
            try
            {
                client.Send(this.mailEntity);
            }
            catch (Exception ex)
            {
                throw new MailServiceException(ex.Message, ex);
            }

        }

        public void Send(MailMessage mailMessage)
        {
            try
            {
                client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw new MailServiceException(ex.Message, ex);
            }
        }
    }
}
