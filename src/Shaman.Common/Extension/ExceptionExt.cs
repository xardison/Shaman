using System;

namespace Shaman.Common.Extension
{
    public static class ExceptionExt
    {
        public static string GetMessage(this Exception exception)
        {
            if (exception == null)
            {
                return String.Empty;
            }

            string message;

            try
            {
                message = exception.Message;

                if (exception is AggregateException ae)
                {
                    foreach (var aeInnerException in ae.InnerExceptions)
                    {
                        message += Environment.NewLine + aeInnerException.GetMessage();
                    }
                }

                var im = GetMessage(exception.InnerException);
                if (!string.IsNullOrEmpty(im))
                {
                    message += Environment.NewLine + im;
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return message;
        }

        public static string GetStackTrace(this Exception exception)
        {
            if (exception == null)
            {
                return String.Empty;
            }

            string message;

            try
            {
                message = string.Format(
                    "[{1}] - {2}{0}{3}{0}{0}{4}",
                    Environment.NewLine,
                    exception.GetType().Name,
                    exception.Message,
                    exception.StackTrace,
                    GetStackTrace(exception.InnerException));
            }
            catch (Exception e)
            {
                return e.GetStackTrace();
            }

            return message;
        }

        public static string GetExceptionData(this Exception exception)
        {
            var result = string.Empty;

            if (exception != null)
            {
                foreach (var key in exception.Data.Keys)
                {
                    result += $"{key} = {exception.Data[key]}{Environment.NewLine}";
                }

                //result += GetExceptionData(exception.InnerException);
            }

            return result;
        }
    }
}