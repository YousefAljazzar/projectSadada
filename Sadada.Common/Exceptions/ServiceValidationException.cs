﻿using System;

namespace Sadada.Common.Extensions
{
    public class ServiceValidationException : SadadaException
    {        
        public ServiceValidationException() : base("Service Validation Exception")
        {
        }

        public ServiceValidationException(string message) : base(message)
        {
        }
        
        public ServiceValidationException(int code, string message) : base(code, message)
        {
        }

        public ServiceValidationException(string message, Exception ex) : base(message, ex)
        {
        }

        public ServiceValidationException(int code, string message, Exception ex) : base(code, message, ex)
        {
        }
    }
}
