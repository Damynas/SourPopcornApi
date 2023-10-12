using Domain.Abstractions.Interfaces;

namespace Domain.Directors.DataTransferObjects.Requests;

public sealed record UpdateDirectorRequest(int DirectorId, string Name, string Country, DateTime BornOn) : IRequest;
