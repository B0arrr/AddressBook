using AddressBook.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AddressBook.Interfaces;

public interface IContext : IAsyncDisposable, IDisposable
{
    public DatabaseFacade Database { get; }
    public DbSet<TEntity> Set<TEntity>() where TEntity : class;
    public EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    public DbSet<Book> Books { get; }
    public DbSet<City> Cities { get; }
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}