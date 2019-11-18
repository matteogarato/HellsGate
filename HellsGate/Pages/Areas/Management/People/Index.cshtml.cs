using HellsGate.Models;
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
        private readonly HellsGate.Models.HellsGateContext _context;

        public IndexModel(HellsGate.Models.HellsGateContext context)
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