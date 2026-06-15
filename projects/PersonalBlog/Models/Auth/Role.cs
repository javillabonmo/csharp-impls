// <copyright file="Role.cs" company="TBRZCom">
// Copyright (c) TBRZCom. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Identity;

namespace PersonalBlog.Models.Auth
{
    /// <summary>
    /// Represents a role in the application.
    /// </summary>
    public class Role : IdentityRole<Guid>
    {
    }
}
