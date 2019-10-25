using System.Threading.Tasks;

namespace HellsGate.Lib
{
    public static class LoginManager
    {

        public static async Task<bool> VerifyLogin(string user, string CyphredPassword) =>
            //https://code.msdn.microsoft.com/Token-Based-Authentication-6db2acc9#content
            //TODO: create a token based authentication for the mainteneance page
            false;
    }
}
