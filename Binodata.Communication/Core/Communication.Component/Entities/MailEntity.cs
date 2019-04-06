using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Component.Entities
{
    public class MailEntity
    {
        /// <summary>
        /// 標題
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 寄件者
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// 收件者
        /// </summary>
        public List<string> To { get; set; }

        /// <summary>
        /// Html body
        /// </summary>
        public string BodyHtml { get; set; }

        /// <summary>
        /// Attachment full path including file name
        /// </summary>
        public string AttachFilePath
        {
            get; set;
        }
    }
}