using System;

namespace Shaman.Common.Exc
{
    [Serializable]
    public class UserException : Exception
    {
        public UserException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public UserException(string message) : base(message)
        {
        }

        public static void Throw(string message, Exception innerException = null)
        {
            throw new UserException(message, innerException);
        }
    }
}