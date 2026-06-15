// <copyright file="AuditableEntityBase.cs" company="TBRZCom">
// Copyright (c) TBRZCom. All rights reserved.
// </copyright>

namespace PersonalBlog.Models.Common;

/// <summary>
/// Clase base abstracta para entidades que requieren seguimiento de auditoría.
/// </summary>
public abstract class AuditableEntityBase
{
    /// <summary>
    /// Gets or sets the created at date.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the created by user identifier.
    /// </summary>
    public Guid CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the last updated at date.
    /// </summary>
    public DateTime LastUpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the last updated by user identifier.
    /// </summary>
    public Guid LastUpdatedBy { get; set; }
}
