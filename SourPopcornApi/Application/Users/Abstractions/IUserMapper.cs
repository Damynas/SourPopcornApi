using Application.Abstractions.Data;
using Domain.Users.DataTransferObjects.Responses;
using Domain.Users.Entities;

namespace Application.Users.Abstractions;

public interface IUserMapper : IMapper<User, UserResponse> { }
