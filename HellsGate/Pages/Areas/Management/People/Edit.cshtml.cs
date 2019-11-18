using HellsGate.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace HellsGate.Pages.Areas.Management
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly HellsGate.Models.HellsGateContext _context;

        public EditModel(HellsGate.Models.HellsGateContext context)
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

            PeopleAnagraphicModel = await _context.Peoples.FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);

            if (PeopleAnagraphicModel == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(PeopleAnagraphicModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await PeopleAnagraphicModelExists(PeopleAnagraphicModel.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private async Task<bool> PeopleAnagraphicModelExists(string id)
        {
            return await _context.Peoples.AnyAsync(e => e.Id == id).ConfigureAwait(false);
        }
    }
}