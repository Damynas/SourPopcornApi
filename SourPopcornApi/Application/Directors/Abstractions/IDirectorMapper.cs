using Application.Abstractions.Data;
using Domain.Directors.DataTransferObjects.Responses;
using Domain.Directors.Entities;

namespace Application.Directors.Abstractions;

public interface IDirectorMapper : IMapper<Director, DirectorResponse> { }
