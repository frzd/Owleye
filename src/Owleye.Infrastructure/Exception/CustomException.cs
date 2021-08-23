using System;
using System.Collections.Generic;
using System.Net;

namespace GlobalExceptionHandler
{

    [Serializable]
    public class ValidationException : AppException
    {
        public ValidationException(string message, int customCode) : base(message, HttpStatusCode.BadRequest, customCode)
        {
        }
    }

    [Serializable]
    public class DomainLogicException : AppException
    {
        public DomainLogicException(string message, int customCode) : base(message, HttpStatusCode.InternalServerError, customCode)
        {
        }
        public DomainLogicException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }


    [Serializable]
    public class ExcepionResponseModel
    {
        public string Message { get; set; }
        public List<string> ErrorList { get; set; }
        public int Code { get; set; }
    }


    public interface IHttpStatusCodeAwareException
    {
        HttpStatusCode ApiStatusCode { get; }
    }

    public class AppException : Exception, IHttpStatusCodeAwareException
    {
        public AppException(string message, HttpStatusCode statusCode, int customCode) : base(message)
        {
            ApiStatusCode = statusCode;
            Code = customCode;
        }
        public AppException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public HttpStatusCode ApiStatusCode { get; }
        public int Code { get; }
        public List<string> MessageList { get; set; }

        public void AddErrorToMessageList(string errorMessage)
        {
            if (MessageList == null) MessageList = new List<string>();
            MessageList.Add(errorMessage);
        }
    }

}
