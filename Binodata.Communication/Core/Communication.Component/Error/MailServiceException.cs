using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Component.Error
{
    public class MailServiceException : Exception
    {
        /// <summary>
        /// 錯誤代碼
        /// </summary>
        public int ErrorCode { get; private set; }

        /// <summary>
        /// 建構子
        /// </summary>
        public MailServiceException()
            : base()
        {

        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="message">exception message</param>
        public MailServiceException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="message">exception message</param>
        /// <param name="innerException">inner exception</param>
        public MailServiceException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="errorCode">exception error code</param>
        public MailServiceException(int errorCode)
            : base()
        {
            this.ErrorCode = errorCode;
        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="errorCode">exception error code</param>
        /// <param name="message">exception message</param>
        public MailServiceException(int errorCode, string message)
            : base(message)
        {
            this.ErrorCode = errorCode;
        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="errorCode">exception error code</param>
        /// <param name="message">exception message</param>
        /// <param name="innerException">inner exception</param>
        public MailServiceException(int errorCode, string message, Exception innerException)
            : base(message, innerException)
        {
            this.ErrorCode = errorCode;
        }

        /// <summary>
        /// exception tranfer to string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {

            String errorMessage = String.Format("Error code : {0} | Error message : {1} |", this.ErrorCode, Message);

            return errorMessage;

        }
    }
}
