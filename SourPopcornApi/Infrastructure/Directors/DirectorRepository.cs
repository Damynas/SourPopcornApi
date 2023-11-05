using Application.Abstractions.Data;
using Application.Directors.Abstractions;
using Domain.Directors.Entities;
using Infrastructure.Abstractions;

namespace Infrastructure.Directors;

public class DirectorRepository(IApplicationDbContext context) : Repository<Director>(context), IDirectorRepository { }
