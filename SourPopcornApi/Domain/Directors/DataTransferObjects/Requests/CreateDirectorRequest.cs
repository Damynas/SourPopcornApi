using Domain.Abstractions.Interfaces;

namespace Domain.Directors.DataTransferObjects.Requests;

public sealed record CreateDirectorRequest(string Name, string Country, DateTime BornOn) : IRequest;
