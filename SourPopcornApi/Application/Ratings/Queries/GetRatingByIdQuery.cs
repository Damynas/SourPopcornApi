﻿using Application.Abstractions.Messaging;
using Domain.Ratings.DataTransferObjects.Requests;
using Domain.Ratings.Entities;

namespace Application.Ratings.Queries;

public sealed record GetRatingByIdQuery(GetRatingByIdRequest Request) : IQuery<Rating?>;
