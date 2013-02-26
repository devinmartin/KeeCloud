
namespace KeeCloud.Providers
{
    public class CredentialClaimResult
    {
        public CredentialClaimResult()
        {
        }

        public CredentialClaimResult(string username, string password)
        {
            this.IsSuccess = true;
            this.UserName = username;
            this.Password = password;
        }

        public string UserName { get; private set; }
        public string Password { get; private set; }
        public bool IsSuccess { get; private set; }
    }
}
