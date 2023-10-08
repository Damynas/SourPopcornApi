using Domain.Abstractions.Interfaces;

namespace Application.Abstractions.Data;

public interface IMapper<TEntity, TResponse>
        where TEntity : IEntity
        where TResponse : IResponse
{
    TResponse ToResponse(TEntity entity);
    ICollection<TResponse> ToResponses(ICollection<TEntity> entities);
}
