﻿using ApiTemplate.Domain.User;
using ApiTemplate.Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiTemplate.Infrastructure.Persistence.Configurations;

public class UserConfiguration : BaseConfiguration<User, UserId>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(e => e.CreatedAt)
            .ValueGeneratedNever()
            .HasColumnOrder(102)
            .IsRequired();
        builder.Property(e => e.UpdatedAt)
            .ValueGeneratedNever()
            .HasColumnOrder(104)
            .IsRequired();
        
       ConfigureEntity(builder);
    }

    public override void ConfigureEntity(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever()
            .HasConversion(id => id.Value, 
                value => UserId.Create(value));
        
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(128);
        builder.Property(u => u.Password)
            .IsRequired()
            .HasMaxLength(128);
        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(128);
        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(128);
        builder.Property(u => u.BirthDate)
            .HasConversion(
                date => date.ToDateTime(new TimeOnly()),
                value => new DateOnly(value.Year, value.Month, value.Day))
            .IsRequired();
        
        // builder.HasMany(u => u.Likes)
        //     .WithOne(l => l.User)
        //     .HasForeignKey(l => l.UserId);
        // builder.Metadata.FindNavigation(nameof(User.Likes))
        //     .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}