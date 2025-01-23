using API.Persistence;
using Microsoft.EntityFrameworkCore;


namespace API.Services;


internal class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly BackendDbContext _context;

    public Repository(BackendDbContext context)
    {
        this._context = context;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await this._context.Set<TEntity>().ToListAsync(cancellationToken);
    }

    public async Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await this._context.Set<TEntity>().FindAsync(id, cancellationToken);
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await this._context.Set<TEntity>().AddAsync(entity, cancellationToken);
    }

    public void Delete(TEntity entity)
    {
        this._context.Set<TEntity>().Remove(entity);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await this._context.SaveChangesAsync(cancellationToken);
    }
}



public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken);
    void Delete(TEntity entity);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);
}