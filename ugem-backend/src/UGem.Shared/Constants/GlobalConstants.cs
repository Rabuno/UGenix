namespace UGem.Shared.Constants;

public static class Roles
{
    public const string Admin = "Admin";
    public const string Merchant = "Merchant";
    public const string Customer = "Customer";
    public const string Reviewer = "Reviewer";
    public const string Staff = "Staff";
}

public static class Policies
{
    public const string RequireAdmin = "RequireAdmin";
    public const string RequireMerchant = "RequireMerchant";
}

public static class Tables
{
    public const string OutboxMessages = "OutboxMessages";
    public const string AuditLogs = "AuditLogs";
}
