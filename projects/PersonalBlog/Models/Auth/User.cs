// <copyright file="User.cs" company="TBRZCom">
// Copyright (c) TBRZCom. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Identity;

namespace PersonalBlog.Models.Auth;

/// <summary>
/// Represents a user in the application.
/// </summary>
public class User : IdentityUser<Guid>
{
}
