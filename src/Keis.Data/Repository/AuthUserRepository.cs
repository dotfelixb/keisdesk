﻿using Dapper;
using Keis.Model;
using Keis.Model.Domain;
using Keis.Model.Queries;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Keis.Data.Repository;

public interface IAuthUserRepository
{
    Task<FetchUser> GetUser(string id, CancellationToken cancellationToken);

    Task WriteLoginAuditAsync(LoginAudit audit, CancellationToken cancellationToken);
}

public class AuthUserRepository : BasePostgresRepository
    , IUserStore<AuthUser>
    , IUserPasswordStore<AuthUser>
    , IUserEmailStore<AuthUser>
    , IUserPhoneNumberStore<AuthUser>
    , IUserTwoFactorStore<AuthUser>
    , IUserRoleStore<AuthUser>
    , IAuthUserRepository
{
    #region Identity Base

    public AuthUserRepository(IOptions<ConnectionStringOptions> options) : base(options)
    {
    }

    public Task<AuthUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            var query = @"SELECT * FROM public.authuser WHERE normalizedemail = @NormalizedEmail";

            return await conn.QueryFirstOrDefaultAsync<AuthUser>(query, new {NormalizedEmail = normalizedEmail});
        });
    }

    public Task<string> GetEmailAsync(AuthUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(user.Email);
    }

    public Task<bool> GetEmailConfirmedAsync(AuthUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(user.EmailConfirmed);
    }

    public Task<string> GetNormalizedEmailAsync(AuthUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(user.NormalizedEmail);
    }

    public Task SetEmailAsync(AuthUser user, string email, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Task.Run(() => { user.Email = email; }, cancellationToken);
    }

    public Task SetEmailConfirmedAsync(AuthUser user, bool confirmed, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Task.Run(() => { user.EmailConfirmed = confirmed; }, cancellationToken);
    }

    public Task SetNormalizedEmailAsync(AuthUser user, string normalizedEmail, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Task.Run(() => { user.NormalizedEmail = normalizedEmail; }, cancellationToken);
    }

    public Task<string> GetPasswordHashAsync(AuthUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(user.PasswordHash);
    }

    public Task<bool> HasPasswordAsync(AuthUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
    }

    public Task SetPasswordHashAsync(AuthUser user, string passwordHash, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Task.Run(() => { user.PasswordHash = passwordHash; }, cancellationToken);
    }

    public Task<string> GetPhoneNumberAsync(AuthUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(user.Phone);
    }

    public Task<bool> GetPhoneNumberConfirmedAsync(AuthUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(user.PhoneConfirmed);
    }

    public Task SetPhoneNumberAsync(AuthUser user, string phoneNumber, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        throw new NotImplementedException();
    }

    public Task SetPhoneNumberConfirmedAsync(AuthUser user, bool confirmed, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Task.Run(() => { user.PhoneConfirmed = confirmed; }, cancellationToken);
    }

    public Task AddToRoleAsync(AuthUser user, string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        throw new NotImplementedException();
    }

    public Task<IList<string>> GetRolesAsync(AuthUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection<IList<string>>(async conn =>
        {
            var query = @"
                SELECT r.""name""
                FROM authrole r
                JOIN authuserrole ur ON r.id = ur.authrole
                WHERE ur.authuser = @Id
                ";

            var result = await conn.QueryAsync<string>(query, new {user.Id});
            return result.ToList();
        });
    }

    public Task<IList<AuthUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        throw new NotImplementedException();
    }

    public Task<bool> IsInRoleAsync(AuthUser user, string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        throw new NotImplementedException();
    }

    public Task RemoveFromRoleAsync(AuthUser user, string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        throw new NotImplementedException();
    }

    public Task<IdentityResult> CreateAsync(AuthUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            var query = $@"INSERT INTO public.authuser
                    (id
                    , username
                    , normalizedusername
                    , email
                    , normalizedemail
                    , emailconfirmed
                    , passwordhash
                    , securitystamp
                    , phone
                    , phoneconfirmed
                    , twofactorenabled
                    , lockout
                    , failedcount
                    , rowversion
                    , createdby
                    , updatedby)
                VALUES
	                (@{nameof(AuthUser.Id)}
                    , @{nameof(AuthUser.UserName)}
                    , @{nameof(AuthUser.NormalizedUserName)}
                    , @{nameof(AuthUser.Email)}
                    , @{nameof(AuthUser.NormalizedEmail)}
                    , FALSE
                    , @{nameof(AuthUser.PasswordHash)}
                    , @{nameof(AuthUser.SecurityStamp)}
                    , @{nameof(AuthUser.Phone)}
                    , FALSE
                    , FALSE
                    , FALSE
                    , 0
                    , @{nameof(AuthUser.RowVersion)}
                    , @{nameof(AuthUser.CreatedBy)}
                    , @{nameof(AuthUser.UpdatedBy)});
            ";

            var result = await conn.ExecuteAsync(query, new
            {
                user.Id,
                user.UserName,
                user.NormalizedUserName,
                user.Email,
                user.NormalizedEmail,
                user.PasswordHash,
                user.SecurityStamp,
                user.Phone,
                user.RowVersion,
                user.CreatedBy,
                user.UpdatedBy
            });

            // TODO : Save Audit log

            return result > 0
                ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError {Description = "User creation failed!"});
        });
    }

    public Task<IdentityResult> DeleteAsync(AuthUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            var query = @"UPDATE public.authuser SET deleted = TRUE WHERE id = @Id";

            var cmd = await conn.ExecuteAsync(query, new {user.Id});

            return cmd > 0
                ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError {Description = "User deletion failed!"});
        });
    }

    public void Dispose()
    {
        // Nothing to dispose.
    }

    public Task<AuthUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            var query = @"SELECT * FROM public.authuser WHERE id = @Id";

            return await conn.QueryFirstOrDefaultAsync<AuthUser>(query, new {Id = userId});
        });
    }

    public Task<AuthUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            var query = @"SELECT * FROM public.authuser WHERE normalizedusername = @NormalizedUserName";

            return await conn.QueryFirstOrDefaultAsync<AuthUser>(query, new {NormalizedUserName = normalizedUserName});
        });
    }

    public Task<string> GetNormalizedUserNameAsync(AuthUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(user.NormalizedUserName);
    }

    public Task<string> GetUserIdAsync(AuthUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(user.Id);
    }

    public Task<string> GetUserNameAsync(AuthUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(user.UserName);
    }

    public Task SetNormalizedUserNameAsync(AuthUser user, string normalizedName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Task.Run(() => { user.NormalizedUserName = normalizedName; }, cancellationToken);
    }

    public Task SetUserNameAsync(AuthUser user, string userName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Task.Run(() => { user.UserName = userName; }, cancellationToken);
    }

    public Task<IdentityResult> UpdateAsync(AuthUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            var query = $@"UPDATE public.authuser
                SET username=@{nameof(user.UserName)}
                    , normalizedusername=@{nameof(AuthUser.NormalizedUserName)}
                    , email=@{nameof(AuthUser.Email)}
                    , normalizedemail=@{nameof(AuthUser.NormalizedEmail)}
                    , emailconfirmed=@{nameof(AuthUser.EmailConfirmed)}
                    , passwordhash=@{nameof(AuthUser.PasswordHash)}
                    , securitystamp=@{nameof(AuthUser.SecurityStamp)}
                    , phone=@{nameof(AuthUser.Phone)}
                    , phoneconfirmed=@{nameof(AuthUser.PhoneConfirmed)}
                    , twofactorenabled=@{nameof(AuthUser.TwoFactorEnabled)}
                    , lockout=@{nameof(AuthUser.Lockout)}
                    , failedcount=@{nameof(AuthUser.FailedCount)}
                    , rowversion=@{nameof(AuthUser.RowVersion)}
                    , updatedby=@{nameof(AuthUser.UpdatedBy)}
                    , updatedat=now()
                WHERE id=@Id;";

            var result = await conn.ExecuteAsync(query, new
            {
                user.Id,
                user.UserName,
                user.NormalizedUserName,
                user.Email,
                user.NormalizedEmail,
                user.EmailConfirmed,
                user.PasswordHash,
                user.SecurityStamp,
                user.Phone,
                user.PhoneConfirmed,
                user.TwoFactorEnabled,
                user.Lockout,
                user.FailedCount,
                user.RowVersion,
                user.UpdatedBy
            });

            // TODO : Save Audit log

            return result > 0
                ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError {Description = "User creation failed!"});
        });
    }

    public Task<bool> GetTwoFactorEnabledAsync(AuthUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(user.TwoFactorEnabled);
    }

    public Task SetTwoFactorEnabledAsync(AuthUser user, bool enabled, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Task.Run(() => { user.TwoFactorEnabled = enabled; }, cancellationToken);
    }

    #endregion Identity Base

    #region AuthUser

    public Task WriteLoginAuditAsync(LoginAudit audit, CancellationToken cancellationToken)
    {
        return WithConnection(async conn =>
        {
            var cmd = @"
                INSERT INTO public.authloginauditlog
                    (id
                    , authuser
                    , device
                    , platform
                    , browser
                    , address)
                VALUES(@Id
                    , @AuthUser
                    , @Device
                    , @Platform
                    , @Browser
                    , @Address);
                ";

            var result = await conn.ExecuteAsync(cmd, new
            {
                audit.Id,
                audit.AuthUser,
                audit.Device,
                audit.Platform,
                audit.Browser,
                audit.Address
            });
            return result;
        });
    }

    public Task<FetchUser> GetUser(string id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            var query = @"
                SELECT u.*
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = u.createdby) AS CreatedByName
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = u.updatedby) AS UpdatedByName
                FROM authuser u
                WHERE u.id =  @Id";

            return await conn.QueryFirstOrDefaultAsync<FetchUser>(query, new {Id = id});
        });
    }

    #endregion AuthUser
}