using HellsGate.Models.DatabaseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HellsGate.Pages.Areas.Management
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly HellsGateContext _context;

        public IndexModel(HellsGateContext context)
        {
            _context = context;
        }

        public IList<PeopleAnagraphicModel> PeopleAnagraphicModel { get; set; }

        public async Task OnGetAsync()
        {
            PeopleAnagraphicModel = await _context.Peoples.ToListAsync();
        }
    }
}