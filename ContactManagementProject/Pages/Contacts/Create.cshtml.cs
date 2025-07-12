using ContactManagementProject.Models;
using ContactManagementProject.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContactManagementProject.Pages.Contacts
{
    [Authorize(AuthenticationSchemes = "MyCookieAuth")]
    public class CreateModel : PageModel
    {
        private readonly IContactRepository _repository;

        public CreateModel(IContactRepository repository)
        {
            _repository = repository;
        }

        [BindProperty]
        public Contact Contact { get; set; } = new();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            if (await _repository.EmailExistsAsync(Contact.Email))
            {
                ModelState.AddModelError("Contact.Email", "Email is already in use.");
                return Page();
            }

            if (await _repository.ContactNumberExistsAsync(Contact.ContactNumber))
            {
                ModelState.AddModelError("Contact.ContactNumber", "Contact number is already in use.");
                return Page();
            }

            await _repository.AddAsync(Contact);
            return RedirectToPage("Index");
        }
    }
}
