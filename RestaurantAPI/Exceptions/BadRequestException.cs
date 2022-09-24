using System;
using System.Runtime.Serialization;

namespace RestaurantAPI
{
    public class BadRequestException : Exception
    { 
        public BadRequestException(string message) : base(message)
        {
        }
    }
}