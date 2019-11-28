﻿using HellsGate.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace HellsGate.Pages.Areas.Management
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly HellsGate.Models.HellsGateContext _context;

        public CreateModel(HellsGate.Models.HellsGateContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public PeopleAnagraphicModel PeopleAnagraphicModel { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Peoples.Add(PeopleAnagraphicModel);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}