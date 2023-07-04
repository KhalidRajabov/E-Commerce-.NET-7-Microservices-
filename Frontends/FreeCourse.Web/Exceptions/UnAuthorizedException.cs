using System.Runtime.Serialization;

namespace FreeCourse.Web.Exceptions
{
    public class UnAuthorizedException : Exception
    {
        public UnAuthorizedException()
        {
        }

        public UnAuthorizedException(string? message) : base(message)
        {
        }

        public UnAuthorizedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        /*protected UnAuthorizedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }*/
    }
}
