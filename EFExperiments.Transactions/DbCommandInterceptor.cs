using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Text;

namespace EFExperiments.Transactions
{
    public class DbCommandInterceptor : IDbCommandInterceptor
    {
        private static readonly ISet<DbType> QuotedDbTypes = new HashSet<DbType> {
            DbType.AnsiString,
            DbType.Date,
            DbType.DateTime,
            DbType.Guid,
            DbType.String,
            DbType.AnsiStringFixedLength,
            DbType.StringFixedLength
        };

        private readonly StringBuilder log = new StringBuilder();

        public string Log => log.ToString();

        public void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            log.AppendLine(FormatCommand(command));
        }

        public void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext) {}

        public void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            log.AppendLine(FormatCommand(command));
        }

        public void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext) {}

        public void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            log.AppendLine(FormatCommand(command));
        }

        public void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext) {}

        public static string FormatCommand(DbCommand command)
        {
            var result = new StringBuilder(command.CommandText);
            foreach (DbParameter parameter in command.Parameters) {
                result.Replace(parameter.ParameterName, QuotedDbTypes.Contains(parameter.DbType) ? $"'{parameter.Value}'" : Convert.ToString(parameter.Value));
            }
            return result.ToString();
        }
    }
}