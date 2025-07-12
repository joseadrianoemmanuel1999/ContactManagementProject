using ContactManagementProject.Models;
using ContactManagementProject.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContactManagementProject.Pages.Contacts
{
    [Authorize(AuthenticationSchemes = "MyCookieAuth")]
    public class DetailsModel : PageModel
    {
        private readonly IContactRepository _repository;

        public DetailsModel(IContactRepository repository)
        {
            _repository = repository;
        }

        public Contact Contact { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var contact = await _repository.GetByIdAsync(id);

            if (contact == null)
                return NotFound();

            Contact = contact;
            return Page();
        }
    }
}
