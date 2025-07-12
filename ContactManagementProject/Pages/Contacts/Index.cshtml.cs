using Microsoft.AspNetCore.Mvc.RazorPages;
using ContactManagementProject.Models;
using ContactManagementProject.Repositories;

namespace ContactManagementProject.Pages.Contacts
{
    public class IndexModel : PageModel
    {
        private readonly IContactRepository _repository;

        public IndexModel(IContactRepository repository)
        {
            _repository = repository;
        }

        public List<Contact> Contacts { get; set; } = new();

        public async Task OnGetAsync()
        {
            Contacts = (await _repository.GetAllAsync()).ToList();
        }
    }
}
