using API_CONDOMINIO_2.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace API_CONDOMINIO_2.Data.Mappings
{
    public class BlockMap : IEntityTypeConfiguration<Block>
    {
        public void Configure(EntityTypeBuilder<Block> builder)
        {
            builder.ToTable("Block");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property(x => x.NameBlock)
                .IsRequired()
                .HasColumnName("Name")
                .HasColumnType("VARCHAR")
                .HasMaxLength(80);

            builder.Property(x => x.QuantityeUnit)
                .IsRequired()
                .HasColumnName("QuantityeUnit").IsRequired();

            builder.Property(x => x.QuantityFloor)
                 .IsRequired()
                 .HasColumnName("QuantityFloor").IsRequired();


            builder.HasIndex(x => x.NameBlock, "IX_NameBlock")
                  .IsUnique();

        }
    }
}

