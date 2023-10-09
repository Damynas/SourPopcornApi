using Domain.Abstractions.Interfaces;

namespace Domain.Directors.DataTransferObjects.Requests;

public sealed record DeleteDirectorRequest(int Id) : IRequest;
