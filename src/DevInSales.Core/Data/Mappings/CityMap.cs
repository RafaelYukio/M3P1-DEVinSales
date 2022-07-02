﻿using DevInSales.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevInSales.Core.Data.Mappings
{
    public class CityMap : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable("Cities");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
            .HasMaxLength(255)
            .IsUnicode(false)
            .IsRequired();

            builder.HasOne(p => p.State)
            .WithMany(p => p.Cities)
            .HasForeignKey(p => p.StateId);
            builder.HasData(
            new List<City>()
            {
                new(1, 24, "Florianópolis"),
                new(2, 25, "Campinas"),
                new(3, 25, "São Paulo"),
                new(4, 19, "Rio de Janeiro")
            }
            );
        }
    }
}
