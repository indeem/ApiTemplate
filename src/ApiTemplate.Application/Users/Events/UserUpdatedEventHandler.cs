﻿using ApiTemplate.Application.Common.EventHandlers;
using ApiTemplate.Application.Common.Interfaces.Persistence;
using ApiTemplate.Domain.Common.Events;
using ApiTemplate.Domain.Users;
using ApiTemplate.Domain.Users.ValueObjects;

namespace ApiTemplate.Application.Users.Events;

public class UserUpdatedEventHandler : UpdatedEventHandler<IUserRepository, User, UserId, UpdatedEvent<User, UserId>>
{
    private readonly IUserRepository _repository;

    public UserUpdatedEventHandler(IUserRepository repository) : base(repository)
    {
        _repository = repository;
    }

    protected override async IAsyncEnumerable<string> GetCacheKeysAsync(UpdatedEvent<User, UserId> notification)
    {
        yield return await _repository.EntityValueCacheKeyAsync(nameof(_repository.GetByEmailAsync),
            notification.Updated.Email);
        yield return await _repository.EntityValueCacheKeyAsync(nameof(_repository.IsEmailUniqueAsync),
            notification.Updated.Email);
        
        await foreach (var cacheKey in base.GetCacheKeysAsync(notification))
        {
            yield return cacheKey;
        }
    }
}