using HellsGate.Models.DatabaseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HellsGate.Pages.Areas.Management
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly HellsGateContext _context;

        public DeleteModel(HellsGateContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PeopleAnagraphicModel = await _context.Peoples.FindAsync(id);

            if (PeopleAnagraphicModel != null)
            {
                _context.Peoples.Remove(PeopleAnagraphicModel);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}