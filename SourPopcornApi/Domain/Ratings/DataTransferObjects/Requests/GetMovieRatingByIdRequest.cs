﻿using Domain.Abstractions.Interfaces;

namespace Domain.Ratings.DataTransferObjects.Requests;

public sealed record GetMovieRatingByIdRequest(int MovieId, int RatingId) : IRequest;
