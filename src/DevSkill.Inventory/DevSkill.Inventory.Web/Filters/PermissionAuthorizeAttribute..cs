using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace DevSkill.Inventory.Web.Filters
{
    public class PermissionAuthorizeAttribute : TypeFilterAttribute
    {
        public PermissionAuthorizeAttribute(string permission) : base(typeof(PermissionAuthorizationFilter))
        {
            Arguments = new object[] { permission };
        }
    }

    public class PermissionAuthorizationFilter : IAuthorizationFilter
    {
        private readonly string _permission;

        public PermissionAuthorizationFilter(string permission)
        {
            _permission = permission;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var hasClaim = user.Claims.Any(c => c.Type == "Permission" && c.Value == _permission);

            if (!hasClaim)
            {
                context.Result = new ForbidResult();
            }
        }
    }

}
