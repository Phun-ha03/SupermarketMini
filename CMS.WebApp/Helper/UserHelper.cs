using System.Security.Claims;

namespace CMS.WebApp.Helper
{
    public static class UserHelper
    {
        /*private readonly IUserService _userService;
        private readonly ISession _session;
        private readonly IConfiguration _configuration;
        private static UserViewModel user;

        public UserHelper(IUserService userService, ISession session, IConfiguration configuration)
        {
            _userService = userService;
            _session = session;
            _configuration = configuration;
        }*/

        public static int getUserId()
        {
            return 0;
        }

        /*private void getUser(ClaimsPrincipal principal) {
            if (user != null && user.Id > 0) return;
            if (_session.GetString(_configuration["SessionKeys:UserData"]) != null) {
                user = JsonConvert.DeserializeObject<UserViewModel>(_session.GetString(_configuration["SessionKeys:UserData"]));
                return;
            }
            if (user != null && user.Id > 0) return;
            var userId = principal == null || principal.FindFirst(ClaimTypes.NameIdentifier) == null || string.IsNullOrEmpty(principal.FindFirst(ClaimTypes.NameIdentifier).Value) ? 0 : Convert.ToInt32(principal.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (userId > 0) {
                var apiResult = _userService.GetById(userId).Result;
                if (apiResult.IsSuccessed && apiResult.ResultObj.Id > 0) {
                    user = apiResult.ResultObj;
                    _session.SetString(_configuration["SessionKeys:UserData"], JsonConvert.SerializeObject(user));
                }
            }
        }*/

        public static int getLoginUserId(ClaimsPrincipal principal)
        {
            return principal == null || principal.FindFirst(ClaimTypes.NameIdentifier) == null || string.IsNullOrEmpty(principal.FindFirst(ClaimTypes.NameIdentifier).Value) ? 0 : Convert.ToInt32(principal.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        public static string getLoginUsername_(ClaimsPrincipal principal)
        {
            return principal == null || principal.FindFirst(ClaimTypes.Name) == null ? string.Empty : principal.FindFirst(ClaimTypes.Name).Value;
        }

        /*public string getLoginUserFullname(ClaimsPrincipal principal)
        {
            getUser(principal);
            return user.FullName ?? "";
        }

        public string getLoginUserEmail(ClaimsPrincipal principal)
        {
            getUser(principal);
            return user.Email ?? "";
        }

        public UserViewModel getLoginUserViewModell(ClaimsPrincipal principal)
        {
            getUser(principal);
            return user;
        }*/
    }
}
