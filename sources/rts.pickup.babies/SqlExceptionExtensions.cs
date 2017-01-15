using System.Data.SqlClient;
using Bespoke.Sph.Domain;

namespace Bespoke.PosEntt.CustomActions
{
    public static  class SqlExceptionExtensions
    {
        public static bool IsTimeout(this SqlException exception)
        {
            if (exception.Number == -2) return true;
            if (exception.Number == 11) return true;
            if (exception.Number == -2146232060) return true;
            var message = exception.Message.ToEmptyString().ToLowerInvariant();
            if (message.Contains("timeout")) return true;
            if (message.Contains("network")) return true;

            return false;
        }
        public static bool IsDeadlock(this SqlException exception)
        {
            if (exception.Number == 1205) return true;

            var message = exception.Message.ToEmptyString().ToLowerInvariant();
            if (message.Contains("deadlock")) return true;
            if (message.Contains("victim")) return true;

            return false;
        }
        public static bool IsDeadlockOrTimeout(this SqlException exception)
        {
            return exception.IsTimeout() || exception.IsDeadlock();
        }
    }
}
