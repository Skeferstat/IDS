using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdsServer.Database.Models.Configurations;
public partial class BasketConfiguration : IEntityTypeConfiguration<Basket>
{
    public void Configure(EntityTypeBuilder<Basket> entity)
    {
        entity.ToTable("Basket");
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        entity.Property(e => e.Data)
            .IsRequired()
            .HasMaxLength(50000);

        entity.Property(e => e.HookUrl)
            .IsRequired()
            .HasMaxLength(300);

        entity.Property(e => e.LastUpdate)
            .IsRequired();

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Basket> entity);
}