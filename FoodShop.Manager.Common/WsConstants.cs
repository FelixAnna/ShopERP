namespace FoodShop.Manager.Common
{
    public class WsConstants
    {
        //Constant
        public const string UserIdClaim = "UserId";
        public const string RegionClaim = "Region";

        public const string GlobalAdmin = "GlobalAdmin";
        public const string Employee = "Employee";
        
        //Error Message
        public const string UnauthorizedErrorType = "User_Unauthorized_Error";
        public const string UnauthorizedErrorMessage = "登录失效！";

        public const string ForbiddenErrorType = "User_Forbidden_Error";
        public const string ForbiddenErrorMessage = "权限不足！";

        public const string GeneralErrorType = "General_Error";
        public const string GeneralErrorMessage = "服务器错误！";
    }
}
