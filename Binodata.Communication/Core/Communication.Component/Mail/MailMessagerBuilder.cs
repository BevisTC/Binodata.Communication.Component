using Communication.Component.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Component.Mail
{
    public class MailMessagerBuilder
    {
        public static MailMessage CreateMailMessage(MailEntity entity)
        {
            MailMessage message = new MailMessage();
            message.Subject = entity.Subject;
            message.SubjectEncoding = Encoding.UTF8;
            message.HeadersEncoding = Encoding.UTF8;
            message.From = new MailAddress(entity.From);
            SetMailTo(entity, message);
            message.IsBodyHtml = true;
            message.Body = entity.BodyHtml;
            message.BodyEncoding = Encoding.UTF8;

            if (String.IsNullOrEmpty(entity.AttachFilePath) == false)
            {
                SetMailAttachment(entity, message);
            }

            return message;
        }

        /// <summary>
        /// Mail 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private static void SetMailAttachment(MailEntity entity, MailMessage message)
        {
            string filePath = entity.AttachFilePath;
            Attachment data;
            filePath =  entity.AttachFilePath;
            data = new Attachment(filePath, MediaTypeNames.Application.Octet);
            data.Name = System.IO.Path.GetFileName(filePath);
            data.NameEncoding = Encoding.GetEncoding("utf-8");
            data.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
            message.Attachments.Add(data);
        }

        private static void SetMailTo(MailEntity entity, MailMessage message)
        {
            foreach (string address in entity.To)
            {
                message.To.Add(address);
            }
        }
    }
}
