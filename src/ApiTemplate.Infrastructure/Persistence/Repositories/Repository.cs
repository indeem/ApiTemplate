﻿using ApiTemplate.Application.Common.EventHandlers;
using ApiTemplate.Application.Common.Interfaces.Persistence;
using ApiTemplate.Domain.Common.Events;
using ApiTemplate.Domain.Common.Specification;
using ApiTemplate.Domain.Models;
using ApiTemplate.Domain.Users.ValueObjects;
using ApiTemplate.Infrastructure.Attributes;
using ApiTemplate.Infrastructure.Extensions;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace ApiTemplate.Infrastructure.Persistence.Repositories;

[CacheDomainEvent(typeof(UpdatedEvent<,>), typeof(UpdatedEventHandler<,,,>))]
[CacheDomainEvent(typeof(DeletedEvent<,>), typeof(DeletedEventHandler<,,,>))]
public class Repository<TEntity, TId> : IRepository<TEntity, TId>
    where TEntity : Entity<TId>
    where TId : IdObject<TId>
{
    private readonly ApiTemplateDbContext _dbContext;
    
    public Repository(ApiTemplateDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual async Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken, Specification<TEntity, TId> specification = null)
    {
        return await _dbContext.Set<TEntity>()
            .Specificate(specification)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken, Specification<TEntity, TId> specification = null)
    {
        return await _dbContext.Set<TEntity>()
            .Specificate(specification)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken: cancellationToken);
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity, UserId userId, CancellationToken cancellationToken)
    {
        entity.CreatedBy = userId;
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedBy = userId;
        entity.UpdatedAt = DateTime.UtcNow;
        await _dbContext.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return entity;
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        entity.AddDomainEventAsync(new UpdatedEvent<TEntity, TId>(entity));

        await _dbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public virtual async Task<Deleted> DeleteAsync(TId id, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Set<TEntity>().FindAsync(new object?[] { id }, cancellationToken: cancellationToken);
        await entity.AddDomainEventAsync(new DeletedEvent<TEntity, TId>(entity));
        
        _dbContext.Set<TEntity>().Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Deleted();
    }

    public async Task ClearCacheAsync(List<string> cacheKeys = null)
    {
        throw new NotImplementedException();
    }

    public async Task<string> EntityValueCacheKeyAsync(string usage, string value)
    {
        throw new NotImplementedException();
    }
}