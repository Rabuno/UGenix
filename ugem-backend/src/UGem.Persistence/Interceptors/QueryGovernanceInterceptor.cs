using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using Microsoft.Extensions.Options;
using UGem.Persistence.Options;

namespace UGem.Persistence.Interceptors;

public class QueryGovernanceInterceptor : DbCommandInterceptor
{
    private readonly QueryPerformanceOptions _options;

    public QueryGovernanceInterceptor(IOptions<QueryPerformanceOptions> options)
    {
        _options = options.Value;
    }

    public override InterceptionResult<DbDataReader> ReaderExecuting(
        DbCommand command, 
        CommandEventData eventData, 
        InterceptionResult<DbDataReader> result)
    {
        // 1. Enforce Timeout budget
        command.CommandTimeout = _options.DefaultQueryTimeoutSeconds;

        // 2. Add observability tags (if not present)
        if (!command.CommandText.Contains("/*"))
        {
            command.CommandText = $"/* Service: UGem | TraceId: {Guid.NewGuid()} */ " + command.CommandText;
        }

        return base.ReaderExecuting(command, eventData, result);
    }
}
