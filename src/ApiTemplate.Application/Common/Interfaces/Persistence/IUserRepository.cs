using ApiTemplate.Domain.User;
using ApiTemplate.Domain.User.ValueObjects;
using ErrorOr;

namespace ApiTemplate.Application.Common.Interfaces.Persistence;

public interface IUserRepository : IRepository<Domain.User.User, UserId>
{
    Task<Domain.User.User> AddAsync(Domain.User.User entity, CancellationToken cancellationToken);
    Task<Domain.User.User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken);
}