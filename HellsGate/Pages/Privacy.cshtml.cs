using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HellsGate
{
    [Authorize]
    public class PrivacyModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}