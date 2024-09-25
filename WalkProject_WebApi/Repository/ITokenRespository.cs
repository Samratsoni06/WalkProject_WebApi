using Microsoft.AspNetCore.Identity;

namespace WalkProject_WebApi.Repository
{
    public interface ITokenRespository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
