using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Personnummer.Exceptions
{
    public class PersonnummerException : Exception
    {
        internal PersonnummerException() { }

        internal PersonnummerException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        internal PersonnummerException(string message) : base(message) { }

        internal PersonnummerException(string message, Exception innerException) : base(message, innerException) { }
    }
}
