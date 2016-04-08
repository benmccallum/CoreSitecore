using Sitecore.Security.Accounts;

/// <summary>
/// Extension methods off of Sitecore.Security.Accounts.User.
/// </summary>
public static class UserExtensions
{
    /// <summary>
    /// Is the user authenticated and do they belong to the "extranet" domain.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public static bool IsAuthenticatedInExtranet(this User user)
    {
        return user.IsAuthenticatedIn("extranet");
    }

    /// <summary>
    /// Is the user authenticated and do they belong to the <paramref name="domain"/>.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="domain"></param>
    /// <returns></returns>
    public static bool IsAuthenticatedIn(this User user, string domain)
    {
        return user.IsAuthenticated && user.GetDomainName() == domain;
    }
}
