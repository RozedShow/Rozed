using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using WebApp;

public class AuxiliarAuthorizationRequirement : IAuthorizationRequirement
{

}

public class AuxiliarAuthorizationHandler : AuthorizationHandler<AuxiliarAuthorizationRequirement>
{
    private readonly IOptionsSnapshot<GeneralOptions> grlOpts;

    public AuxiliarAuthorizationHandler(IOptionsSnapshot<GeneralOptions> grlOpts)
    {
        this.grlOpts = grlOpts;
    }
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuxiliarAuthorizationRequirement requirement)
    {
        if(context.User.EsMod() || (grlOpts.Value.ModoSerenito && context.User.EsAuxiliar(grlOpts.Value.ModoSerenito))) 
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
        context.Fail();
        return Task.CompletedTask;
    }
}