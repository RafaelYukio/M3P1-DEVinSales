using DevInSales.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevInSales.Core.Data.Mappings
{
    public class AddressMap : IEntityTypeConfiguration<Address>
    {

        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Addresses");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Street)
                .HasMaxLength(150)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(p => p.Cep)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(p => p.Complement)
                .HasMaxLength(255)
                .IsUnicode(false)
                .IsRequired(false);

            builder.Property(p => p.Number)
                .IsRequired();

            builder.HasOne(p => p.City)
                .WithMany(p => p.Addresses)
                .HasForeignKey(p => p.CityId);
            builder.HasData(
                new List<Address> {
                    new(1, "Rua Aqui", "13000000", 100, "Casa", 1),
                    new(2, "Rua Ali", "11111222", 200, "Ap", 2),
                    new(3, "Rua Rua", "33333444", 333, "Chácara", 3),
                    new(4, "Rua 1", "55555666", 333, "Chácara", 4),
                    new(5, "Rua 2", "77777888", 444, "Casa", 4)
                }

                );
        }
    }
}