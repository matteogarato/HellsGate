using HellsGate.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HellsGate.Pages.Areas.Management
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly HellsGate.Models.HellsGateContext _context;

        public DetailsModel(HellsGate.Models.HellsGateContext context)
        {
            _context = context;
        }

        public PeopleAnagraphicModel PeopleAnagraphicModel { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PeopleAnagraphicModel = await _context.Peoples.FirstOrDefaultAsync(m => m.Id == id);

            if (PeopleAnagraphicModel == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}