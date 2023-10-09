using Domain.Abstractions.Interfaces;

namespace Domain.Directors.DataTransferObjects.Requests;

public sealed record GetDirectorByIdRequest(int Id) : IRequest;
