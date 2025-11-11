using Application.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Persistence;

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    private readonly ApplicationDbContext _context;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<TEntity>> GetAllAsync() => await _context.Set<TEntity>().ToListAsync();

    public async Task<TEntity?> GetByIdAsync(Guid id) => await _context.Set<TEntity>().FindAsync(id);

    public IQueryable<TEntity> GetQueryable() => _context.Set<TEntity>();

    public void Insert(TEntity entity) => _context.Set<TEntity>().Add(entity);

    public void Update(TEntity entity) => _context.Set<TEntity>().Update(entity);

    public void Delete(TEntity entity) => _context.Set<TEntity>().Remove(entity);

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
