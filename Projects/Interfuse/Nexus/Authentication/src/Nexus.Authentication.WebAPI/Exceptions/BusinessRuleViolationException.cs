﻿namespace Nexus.Authentication.WebAPI.Exceptions
{
    public class BusinessRuleViolationException : Exception
    {
        public BusinessRuleViolationException()
        {
        }

        public BusinessRuleViolationException(string message) : base(message)
        {
        }

        public BusinessRuleViolationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
