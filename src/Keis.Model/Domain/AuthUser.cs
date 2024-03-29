﻿using MassTransit;

namespace Keis.Model.Domain;

public class AuthUser : DbAudit
{
    public string Id { get; set; } = NewId.NextGuid().ToString();
    public string UserName { get; set; }
    public string NormalizedUserName { get; set; }
    public string Email { get; set; }
    public string NormalizedEmail { get; set; }
    public bool EmailConfirmed { get; set; }
    public string PasswordHash { get; set; }
    public string SecurityStamp { get; set; }
    public string Phone { get; set; }
    public bool PhoneConfirmed { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public bool Lockout { get; set; }
    public int FailedCount { get; set; }
}

public class LoginAudit
{
    public string Id { get; set; }
    public string AuthUser { get; set; }
    public string Device { get; set; }
    public string Platform { get; set; }
    public string Browser { get; set; }
    public string Address { get; set; }
}