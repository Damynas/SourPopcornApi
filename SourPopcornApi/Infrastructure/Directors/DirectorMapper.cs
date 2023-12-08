using Application.Directors.Abstractions;
using Domain.Directors.DataTransferObjects.Responses;
using Domain.Directors.Entities;

namespace Infrastructure.Directors;

public class DirectorMapper : IDirectorMapper
{
    public DirectorResponse ToResponse(Director director)
    {
        return new DirectorResponse(director.Id, director.CreatedOn, director.ModifiedOn, director.Name, director.Country, director.BornOn);
    }

    public IEnumerable<DirectorResponse> ToResponses(IEnumerable<Director> directors)
    {
        return directors.Select(ToResponse).ToList();
    }
}
