﻿using System;

namespace Sadada.Common.Extensions
{
    public class SadadaException : Exception
    {
        public int ErrorCode { get; set; }

        public SadadaException() : base("Sadada Exception")
        {
        }

        public SadadaException(string message) : base(message)
        {
        }

        public SadadaException(int statusCode, string message) : base(message)
        {
            ErrorCode = statusCode;
        }

        public SadadaException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public SadadaException(int statusCode, string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = statusCode;
        }
    }
}
