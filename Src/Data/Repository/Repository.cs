using System.Linq.Expressions;
using Data.Context;
using Domain.Common;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository;

public class Repository : IRepository
{
    private readonly AcumagContext _context;

    public Repository(AcumagContext context)
    {
        _context = context;
    }

    public async Task Update<TEntity>(TEntity entity) where TEntity : EntityBase
    {
        _context.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Add<TEntity>(TEntity entity) where TEntity : EntityBase
    {
        await _context.Set<TEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete<TEntity>(TEntity entity) where TEntity : EntityBase
    {
        _context.Set<TEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    private IQueryable<TEntity> Incluir<TEntity>(IQueryable<TEntity> query, string[] included)
        where TEntity : EntityBase
    {
        var incluidosConsulta = query;

        foreach (var incluido in included)
        {
            incluidosConsulta = incluidosConsulta.Include(incluido);
        }

        return incluidosConsulta;
    }

    private IQueryable<TEntity> Incluir<TEntity>(IQueryable<TEntity> query,
        params Expression<Func<TEntity, object>>[] included) where TEntity : EntityBase
    {
        foreach (var include in included)
        {
            query = query.Include(include);
        }

        return query;
    }

    public async Task<List<TEntity>> List<TEntity>(Expression<Func<TEntity, bool>> predicate,
        params string[] included) where TEntity : EntityBase
    {
        return await Incluir(_context.Set<TEntity>(), included).Where(predicate).ToListAsync();
    }

    public async Task<List<TEntity>> ListAll<TEntity>(params string[] included) where TEntity : EntityBase
    {
        return await Incluir(_context.Set<TEntity>(), included).ToListAsync();
    }

    public async Task<List<TEntity>> ListAllWith<TEntity>(params Expression<Func<TEntity, object>>[] included)
        where TEntity : EntityBase
    {
        return await Incluir(_context.Set<TEntity>(), included).ToListAsync();
    }

    public async Task<TEntity?> GetTheFirstOne<TEntity>(Expression<Func<TEntity, bool>> predicate,
        params string[] included) where TEntity : EntityBase
    {
        return await Incluir(_context.Set<TEntity>(), included).FirstOrDefaultAsync(predicate);
    }

    public async Task<TEntity?> GetForId<TEntity>(Guid id, params string[] included) where TEntity : EntityBase
    {
        return await Incluir(_context.Set<TEntity>(), included).SingleOrDefaultAsync(e => e.Id == id);
    }

    public async Task<TEntity> GetForIdWith<TEntity>(Guid id, params Expression<Func<TEntity, object>>[] included)
        where TEntity : EntityBase
    {
        return await Incluir(_context.Set<TEntity>(), included).SingleOrDefaultAsync(e => e.Id == id);
    }

    public async Task<List<TEntity>> GetAll<TEntity>() where TEntity : EntityBase
    {
        return await _context.Set<TEntity>().ToListAsync();
    }
}