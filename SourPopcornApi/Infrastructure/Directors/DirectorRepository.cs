using Application.Abstractions.Data;
using Application.Directors.Abstractions;
using Domain.Directors.Entities;
using Domain.Users.Entities;
using Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Directors;

public class DirectorRepository(IApplicationDbContext context) : Repository<Director>(context), IDirectorRepository { }
