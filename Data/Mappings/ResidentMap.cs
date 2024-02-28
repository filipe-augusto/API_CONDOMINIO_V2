using API_CONDOMINIO_2.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace API_CONDOMINIO_2.Data.Mappings;

public class ResidentMap : IEntityTypeConfiguration<Resident>
{
    public void Configure(EntityTypeBuilder<Resident> builder)
    {
        builder.ToTable("Resident");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnName("NumberUnit")
            .HasMaxLength(100);
        builder.Property(x => x.Email)
           .IsRequired()
           .HasColumnName("Email")
           .HasMaxLength(100);

        builder.Property(x => x.Phone)
           .IsRequired()
           .HasColumnName("Phone")
           .HasMaxLength(100);

        builder.Property(x => x.Image)
          .IsRequired(false)
          .HasColumnName("Image");

        builder.Property(x => x.Responsible)
        .IsRequired()
        .HasColumnName("Responsible")
        .HasDefaultValue(false);

        builder.Property(x => x.DisabledPerson)
        .IsRequired()
        .HasColumnName("DisabledPerson")
        .HasDefaultValue(false);

        builder.Property(x => x.Excluded)
        .IsRequired()
        .HasColumnName("Excluded")
        .HasDefaultValue(false);

        builder.Property(x => x.Defaulter)
            .IsRequired()
            .HasColumnName("Defaulter")
            .HasDefaultValue(false);

        builder.Property(x => x.Observation)
        .IsRequired()
        .HasColumnName("Observation")
        .HasMaxLength(1000);
        builder.Property(x => x.ExclusionDate)
              .HasColumnName("ExclusionDate");

        builder.Property(x => x.CreationDate)
      .HasColumnName("CreationDate").HasDefaultValue(DateTime.Now);

        builder
            .HasOne(x => x.Unit)
            .WithMany(x => x.Residents)
            .HasConstraintName("FK_Unit_Residents")
            .OnDelete(DeleteBehavior.Cascade);

        builder
        .HasOne(x => x.Sex)
        .WithMany(x => x.Residents)
        .HasConstraintName("FK_Sex_Residents")
        .OnDelete(DeleteBehavior.Cascade);


    }
}

