using System;
using System.Runtime.Serialization;

namespace JPProject.Domain.Core.Exceptions
{
    [Serializable]
    public class DatabaseNotFoundException : Exception
    {
        public DatabaseNotFoundException()
        {
        }

        public DatabaseNotFoundException(string message)
            : base(message)
        {
        }

        public DatabaseNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        // Without this constructor, deserialization will fail
        protected DatabaseNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }


}
