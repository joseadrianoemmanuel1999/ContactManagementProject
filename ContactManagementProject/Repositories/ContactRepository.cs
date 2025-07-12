using ContactManagementProject.Data;
using ContactManagementProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactManagementProject.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly ApplicationDbContext _context;

        public ContactRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Contact>> GetAllAsync()
        {
            return await _context.Contacts.Where(c => !c.IsDeleted).ToListAsync();
        }

        public async Task<Contact?> GetByIdAsync(int id)
        {
            return await _context.Contacts.FirstOrDefaultAsync(c => c.ID == id && !c.IsDeleted);
        }

        public async Task AddAsync(Contact contact)
        {
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Contact contact)
        {
            _context.Contacts.Update(contact);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                contact.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
        {
            return await _context.Contacts.AnyAsync(c =>
                c.Email == email && !c.IsDeleted && (!excludeId.HasValue || c.ID != excludeId));
        }

        public async Task<bool> ContactNumberExistsAsync(string contactNumber, int? excludeId = null)
        {
            return await _context.Contacts.AnyAsync(c =>
                c.ContactNumber == contactNumber && !c.IsDeleted && (!excludeId.HasValue || c.ID != excludeId));
        }
    }
}
