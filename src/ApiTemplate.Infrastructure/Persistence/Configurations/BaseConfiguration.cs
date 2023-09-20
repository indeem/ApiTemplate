﻿using ApiTemplate.Domain.Models;
using ApiTemplate.Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiTemplate.Infrastructure.Persistence.Configurations;

public abstract class BaseConfiguration<TEntity, TId> : IEntityTypeConfiguration<TEntity> 
    where TId : IdObject<TId>
    where TEntity : Entity<TId> 
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(e => e.Id)
            .HasColumnOrder(0)
            .HasConversion(id => id.Value,
                value => IdObject<TId>.Create(value))
            .IsRequired();
        
        builder.Property(e => e.CreatedBy)
            .HasConversion(userId => userId.Value,
                guid => UserId.Create(guid))
            .HasColumnOrder(101)
            .IsRequired();
        builder.Property(e => e.CreatedAt)
            .ValueGeneratedNever()
            .HasColumnOrder(102)
            .IsRequired();
        builder.Property(e => e.UpdatedBy)
            .HasConversion(userId => userId.Value,
                guid => UserId.Create(guid))
            .HasColumnOrder(103)
            .IsRequired();
        builder.Property(e => e.UpdatedAt)
            .ValueGeneratedNever()
            .HasColumnOrder(104)
            .IsRequired();
        
        builder.HasOne(e => e.CreatedByUser)
            .WithMany()
            .HasForeignKey(e => e.CreatedBy)
            .HasPrincipalKey(u => u.Id)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Metadata.FindNavigation(nameof(Entity<TId>.CreatedByUser));
        
        builder.HasOne(e => e.UpdatedByUser)
            .WithMany()
            .HasForeignKey(e => e.UpdatedBy)
            .HasPrincipalKey(u => u.Id)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Metadata.FindNavigation(nameof(Entity<TId>.UpdatedByUser));
        
        ConfigureEntity(builder);
    }

    public abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);
}