namespace CarbonaraWebAPI.Util
{
    public static class Extentions
    {
        public static int GetUserID(this System.Security.Claims.ClaimsPrincipal user)
        {
            return int.Parse(user.Claims.First().Value);
        }
    }
}
