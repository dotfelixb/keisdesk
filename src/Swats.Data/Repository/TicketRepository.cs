﻿using System.Threading;
using Dapper;
using Microsoft.Extensions.Options;
using Swats.Model;
using Swats.Model.Domain;
using Swats.Model.Queries;

namespace Swats.Data.Repository;

public interface ITicketRepository
{
    Task<long> GenerateTicketCode(CancellationToken cancellationToken);
    
    Task<int> CreateTicket(Ticket ticket, CancellationToken cancellationToken);

    Task<int> CreateTicketType(TicketType ticketType, DbAuditLog auditLog, CancellationToken cancellationToken);

    Task<FetchTicketType> GetTicketType(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<FetchTicketType>> ListTicketTypes(int offset = 0, int limit = 1000, bool includeDeleted = false, CancellationToken cancellationToken = default);

}

public class TicketRepository : BasePostgresRepository, ITicketRepository
{
    public TicketRepository(IOptions<ConnectionStringOptions> options) : base(options)
    {
    }

    public Task<int> CreateTicket(Ticket ticket, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        throw new NotImplementedException();
    }

    public Task<int> CreateTicketType(TicketType ticketType, DbAuditLog auditLog, CancellationToken cancellationToken)
    {
        return WithConnection(async conn =>
        {
            var cmd = $@"
                INSERT INTO public.tickettype
                    (id
                    , ""name""
                    , description
                    , color
                    , visibility
                    , rowversion
                    , createdby
                    , updatedby)
                VALUES
                    (@Id
                    , @Name
                    , @Description
                    , @Color
                    , @Visibility
                    , @RowVersion
                    , @CreatedBy
                    , @UpdatedBy);
                ";

            var ctt = await conn.ExecuteAsync(cmd, new
            {
                ticketType.Id,
                ticketType.Name,
                ticketType.Description,
                ticketType.Color,
                ticketType.Visibility,
                ticketType.RowVersion,
                ticketType.CreatedBy,
                ticketType.UpdatedBy
            });

            var logCmd = @"
                INSERT INTO public.tickettypeauditlog
                    (id
                    , target
                    , actionname
                    , description
                    , objectname
                    , objectdata
                    , createdby)
                VALUES
                    (@Id
                    , @Target
                    , @ActionName
                    , @Description
                    , @ObjectName
                    , @ObjectData::jsonb
                    , @CreatedBy);
                ";

            var cl = await conn.ExecuteAsync(logCmd, new
            {
                auditLog.Id,
                auditLog.Target,
                auditLog.ActionName,
                auditLog.Description,
                auditLog.ObjectName,
                auditLog.ObjectData,
                auditLog.CreatedBy
            });

            return ctt + cl;
        });
    }

    public Task<long> GenerateTicketCode(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(conn =>
        {
            var query = "SELECT NEXTVAL('TicketCode');";

            return conn.QuerySingleAsync<long>(query);
        });
    }

    public Task<FetchTicketType> GetTicketType(Guid id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            var query = @"
                SELECT t.*
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = t.createdby) AS CreatedByName
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = t.updatedby) AS UpdatedByName
                FROM tickettype t
                WHERE id = @Id";

            return await conn.QueryFirstOrDefaultAsync<FetchTicketType>(query, new { Id = id});
        });
    }

    public Task<IEnumerable<FetchTicketType>> ListTicketTypes(int offset = 0, int limit = 1000, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            var _includeDeleted = includeDeleted ? " " : " AND t.deleted = FALSE ";
            var query = $@"
                SELECT t.*
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = t.createdby) AS CreatedByName
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = t.updatedby) AS UpdatedByName
                FROM tickettype t
                WHERE 1=1
                {_includeDeleted}
                OFFSET @Offset LIMIT @Limit;
                ";

            return await conn.QueryAsync<FetchTicketType>(query, new { offset, limit });
        });
    }
}