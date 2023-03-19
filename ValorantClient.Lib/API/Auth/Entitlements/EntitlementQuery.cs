using MediatR;

namespace ValorantClient.Lib.API.Auth.Entitlements
{
    /// <summary>
    /// Get <see cref="Entitlement"/>
    /// </summary>
    public class EntitlementQuery : IRequest<Entitlement>
    {
    }
}
