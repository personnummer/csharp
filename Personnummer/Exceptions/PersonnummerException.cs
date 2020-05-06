using System;

namespace Personnummer.Exceptions
{
    public class PersonnummerException : Exception
    {
        internal PersonnummerException(string message) : base(message) { }
    }
}
