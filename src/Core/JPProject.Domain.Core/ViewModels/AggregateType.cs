namespace JPProject.Domain.Core.ViewModels
{
    /// <summary>
    /// Types of aggregate
    /// </summary>
    public enum AggregateType
    {
        Client,
        ApiResource,
        IdentityResource,
        ProtectedGrant,
        Users,
        Roles,
        Email,
        GlobalSettings
    }
}