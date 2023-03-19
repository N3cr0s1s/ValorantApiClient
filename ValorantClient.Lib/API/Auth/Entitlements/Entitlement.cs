namespace ValorantClient.Lib.API.Auth.Entitlements
{

    public class Entitlement
    {
        public string AccessToken { get; set; } = string.Empty;
        public string[] Entitlements { get; set; } = new string[0];
        public string Issuer { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }

}
