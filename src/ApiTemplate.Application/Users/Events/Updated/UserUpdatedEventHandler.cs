﻿using ApiTemplate.Application.Common.Events.Updated;
using ApiTemplate.Application.Common.Interfaces.Persistence;
using ApiTemplate.Domain.Users;
using ApiTemplate.Domain.Users.ValueObjects;

namespace ApiTemplate.Application.Users.Events.Updated;

public class UserUpdatedEventHandler 
    : UpdatedEventHandler<IUserRepository, User, UserId, IUserDto, UpdatedEvent<User, UserId>>
{
    private readonly IUserRepository _repository;

    public UserUpdatedEventHandler(IUserRepository repository) : base(repository)
    {
        _repository = repository;
    }

    protected override async IAsyncEnumerable<string> GetCacheKeysAsync(UpdatedEvent<User, UserId> updatedEvent)
    {
        yield return await _repository.EntityValueCacheKeyAsync(nameof(_repository.GetByEmailAsync),
            updatedEvent.Updated.Email);
        yield return await _repository.EntityValueCacheKeyAsync(nameof(_repository.IsEmailUniqueAsync),
            updatedEvent.Updated.Email);
    }
}