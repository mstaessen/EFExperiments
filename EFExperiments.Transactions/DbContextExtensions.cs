using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Text;

namespace EFExperiments.Transactions
{
    internal static class DbContextExtensions
    {
        public static TResult WithLogger<TContext, TResult>(this TContext context, Func<TContext, TResult> function, out string log)
            where TContext : DbContext
        {
            if (context == null) {
                throw new ArgumentNullException(nameof(context));
            }
            if (function == null) {
                throw new ArgumentNullException(nameof(function));
            }
            var stringBuilder = new StringBuilder();
            var logFunction = context.Database.Log;
            context.Database.Log = s => {
                stringBuilder.Append(s);
                logFunction?.Invoke(s);
            };
            var result = function(context);
            context.Database.Log = logFunction;
            log = stringBuilder.ToString();
            return result;
        }

        public static TResult WithCommandInterceptor<TContext, TResult>(this TContext context, Func<TContext, TResult> function, IDbCommandInterceptor interceptor)
            where TContext : DbContext
        {
            if (context == null) {
                throw new ArgumentNullException(nameof(context));
            }
            if (function == null) {
                throw new ArgumentNullException(nameof(function));
            }
            if (interceptor == null) {
                throw new ArgumentNullException(nameof(interceptor));
            }
            DbInterception.Add(interceptor);
            var result = function(context);
            DbInterception.Remove(interceptor);
            return result;
        }
    }
}