using ContactManagementProject.Models;
using ContactManagementProject.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContactManagementProject.Pages.Contacts
{
    [Authorize(AuthenticationSchemes = "MyCookieAuth")]
    public class DeleteModel : PageModel
    {
        private readonly IContactRepository _repository;

        public DeleteModel(IContactRepository repository)
        {
            _repository = repository;
        }

        [BindProperty]
        public Contact Contact { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var contact = await _repository.GetByIdAsync(id);

            if (contact == null)
                return NotFound();

            Contact = contact;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Contact.ID > 0)
            {
                await _repository.SoftDeleteAsync(Contact.ID);
                return RedirectToPage("Index");
            }

            return NotFound();
        }
    }
}
