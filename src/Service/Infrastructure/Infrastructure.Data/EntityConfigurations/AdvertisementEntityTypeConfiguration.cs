using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigurations;

public class AdvertisementEntityTypeConfiguration : IEntityTypeConfiguration<Advertisement>
{
    public void Configure(EntityTypeBuilder<Advertisement> builder)
    {
        builder.ToTable("advertisements");

        //builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("gen_random_uuid()")
            .IsRequired();
        
        builder.Property(x => x.Text)
            .HasColumnName("text")
            .IsRequired()
            .HasMaxLength(4000);

        builder.Property(x => x.Image)
            .HasColumnName("image")
            .IsRequired()
            .HasMaxLength(500);
        
        builder.Property(x => x.Number)
            .HasColumnName("number");
        
        builder.Property(x => x.Rating)
            .HasColumnName("rating");

        builder.Property(x => x.StartDate)
            .HasColumnName("start_date")
            .IsRequired();

        builder.Property(x => x.EndDate)
            .HasColumnName("end_date")
            .IsRequired();
        
        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            //.OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}