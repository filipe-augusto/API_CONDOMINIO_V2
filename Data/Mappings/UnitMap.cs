using API_CONDOMINIO_2.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace API_CONDOMINIO_2.Data.Mappings;

    public class UnitMap : IEntityTypeConfiguration<Unit>
    {
        public void Configure(EntityTypeBuilder<Unit> builder)
        {
            builder.ToTable("Unit");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property(x => x.NumberUnit)
                .IsRequired()
                .HasColumnName("NumberUnit");


            builder.Property(x => x.PeopleLiving)
                .IsRequired()
                .HasColumnName("PeopleLiving")
                .HasDefaultValue(false);
        builder.Property(x => x.HasGarage)
            .IsRequired()
            .HasColumnName("HasGarage")
            .HasDefaultValue(false);

        builder.Property(x => x.Observation)
                 .IsRequired()
                 .HasColumnName("Observation").HasMaxLength(500);

            builder
                .HasOne(x => x.Block)
                .WithMany(x => x.Units)
                .HasConstraintName("FK_Block_Units")
                .OnDelete(DeleteBehavior.Cascade);




        }
    }

