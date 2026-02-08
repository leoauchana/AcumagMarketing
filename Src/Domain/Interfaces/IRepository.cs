using System.Linq.Expressions;
using Domain.Common;

namespace Domain.Interfaces;

public interface IRepository
{
    Task<List<TEntity>> ListAll<TEntity>(params string[] included) where TEntity : EntityBase;
    Task<List<TEntity>> ListAllWith<TEntity>(params Expression<Func<TEntity, object>>[] included) where TEntity : EntityBase;
    Task<List<TEntity>> List<TEntity>(Expression<Func<TEntity, bool>> predicate, params string[] included) where TEntity : EntityBase;
    Task<TEntity?> GetTheFirstOne<TEntity>(Expression<Func<TEntity, bool>> predicate, params string[] included) where TEntity : EntityBase;
    Task Add<TEntity>(TEntity entity) where TEntity : EntityBase;
    Task Update<TEntity>(TEntity entity) where TEntity : EntityBase;
    Task Delete<TEntity>(TEntity entity) where TEntity : EntityBase;
    Task<TEntity?> GetForId<TEntity>(Guid id, params string[] included) where TEntity : EntityBase;
    Task<TEntity> GetForIdWith<TEntity>(Guid id, params Expression<Func<TEntity, object>>[] included) where TEntity : EntityBase;
    Task<List<TEntity>> GetAll<TEntity>() where TEntity : EntityBase;
}