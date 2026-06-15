// <copyright file="ErrorViewModel.cs" company="TBRZCom">
// Copyright (c) TBRZCom. All rights reserved.
// </copyright>

namespace PersonalBlog.Models;

/// <summary>
/// Represents the view model for error pages.
/// </summary>
public class ErrorViewModel
{
    /// <summary>
    /// Gets or sets the request identifier.
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// Gets a value indicating whether the request identifier should be shown.
    /// </summary>
    public bool ShowRequestId => !string.IsNullOrEmpty(this.RequestId);
}
