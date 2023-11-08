using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Identity;

public class IdentityDynamicClaimsPrincipalContributor : AbpDynamicClaimsPrincipalContributorBase
{
    public async override Task ContributeAsync(AbpClaimsPrincipalContributorContext context)
    {
        var identity = context.ClaimsPrincipal.Identities.FirstOrDefault();
        var userId = identity?.FindUserId();
        if (userId == null)
        {
            return;
        }

        var dynamicClaimsCache = context.GetRequiredService<IdentityDynamicClaimsPrincipalContributorCache>();
        var dynamicClaims = await dynamicClaimsCache.GetAsync(userId.Value, identity.FindTenantId());

        await MapCommonClaimsAsync(identity, dynamicClaims);
        await AddDynamicClaims(identity, dynamicClaims);
    }
}
