using ContactManagementProject.Data;
using ContactManagementProject.Models;
using ContactManagementProject.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ContactManagementProject.Tests.Repositories
{
    public class ContactRepositoryTests
    {
        private async Task<ApplicationDbContext> GetInMemoryDbContextAsync()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new ApplicationDbContext(options);
            await context.Database.EnsureCreatedAsync();

            return context;
        }

        [Fact]
        public async Task AddAsync_ShouldAddContact()
        {
            var context = await GetInMemoryDbContextAsync();
            var repo = new ContactRepository(context);
            var contact = new Contact { Name = "John Doe", ContactNumber = "912345678", Email = "john@example.com" };

            await repo.AddAsync(contact);

            Assert.Single(context.Contacts);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnContacts_ExcludingSoftDeleted()
        {
            var context = await GetInMemoryDbContextAsync();
            context.Contacts.Add(new Contact { Name = "A", ContactNumber = "912345678", Email = "a@example.com" });
            context.Contacts.Add(new Contact { Name = "B", ContactNumber = "923456789", Email = "b@example.com", IsDeleted = true });
            await context.SaveChangesAsync();

            var repo = new ContactRepository(context);
            var contacts = await repo.GetAllAsync();

            Assert.Single(contacts);
            Assert.DoesNotContain(contacts, c => c.IsDeleted);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnContact_IfExistsAndNotDeleted()
        {
            var context = await GetInMemoryDbContextAsync();
            var contact = new Contact { Name = "Jane", ContactNumber = "934567890", Email = "jane@example.com" };
            context.Contacts.Add(contact);
            await context.SaveChangesAsync();

            var repo = new ContactRepository(context);
            var result = await repo.GetByIdAsync(contact.ID);

            Assert.NotNull(result);
            Assert.Equal("Jane", result?.Name);
        }

        [Fact]
        public async Task UpdateAsync_ShouldModifyContact()
        {
            var context = await GetInMemoryDbContextAsync();
            var contact = new Contact { Name = "Old", ContactNumber = "945678901", Email = "old@example.com" };
            context.Contacts.Add(contact);
            await context.SaveChangesAsync();

            contact.Name = "New";
            var repo = new ContactRepository(context);
            await repo.UpdateAsync(contact);

            var updated = await context.Contacts.FindAsync(contact.ID);
            Assert.Equal("New", updated?.Name);
        }

        [Fact]
        public async Task SoftDeleteAsync_ShouldSetIsDeletedToTrue()
        {
            var context = await GetInMemoryDbContextAsync();
            var contact = new Contact { Name = "ToDelete", ContactNumber = "956789012", Email = "delete@example.com" };
            context.Contacts.Add(contact);
            await context.SaveChangesAsync();

            var repo = new ContactRepository(context);
            await repo.SoftDeleteAsync(contact.ID);

            var deleted = await context.Contacts.FindAsync(contact.ID);
            Assert.True(deleted?.IsDeleted);
        }

        [Fact]
        public async Task EmailExistsAsync_ShouldReturnTrue_IfEmailExists()
        {
            var context = await GetInMemoryDbContextAsync();
            context.Contacts.Add(new Contact { Name = "E", ContactNumber = "967890123", Email = "exists@example.com" });
            await context.SaveChangesAsync();

            var repo = new ContactRepository(context);
            var exists = await repo.EmailExistsAsync("exists@example.com");

            Assert.True(exists);
        }

        [Fact]
        public async Task ContactNumberExistsAsync_ShouldReturnTrue_IfContactNumberExists()
        {
            var context = await GetInMemoryDbContextAsync();
            context.Contacts.Add(new Contact { Name = "F", ContactNumber = "978901234", Email = "f@example.com" });
            await context.SaveChangesAsync();

            var repo = new ContactRepository(context);
            var exists = await repo.ContactNumberExistsAsync("978901234");

            Assert.True(exists);
        }
    }
}
