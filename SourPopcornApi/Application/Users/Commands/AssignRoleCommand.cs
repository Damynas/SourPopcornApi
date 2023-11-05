﻿using Application.Abstractions.Messaging;
using Domain.Users.DataTransferObjects.Requests;

namespace Application.Users.Commands;

public record AssignRoleCommand(ManageRolesRequest Request) : ICommand;
