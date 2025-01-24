using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;
using BasketReceive;
using System.Xml.Serialization;
using IdsServer.Database.Converter;

namespace IdsServer.Database.Models.Configurations;
public partial class BasketConfiguration : IEntityTypeConfiguration<Basket>
{
    public void Configure(EntityTypeBuilder<Basket> entity)
    {
        entity.ToTable("Basket");
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        entity.Property(e => e.HookUrl)
            .IsRequired()
            .HasMaxLength(300);

        entity.Property(e => e.LastUpdate)
            .IsRequired();

        entity.Property(e => e.RawBasket)
            .IsRequired()
            .HasConversion(
                x => XmlConverter.SerializeToXml(x), 
                xml => XmlConverter.DeserializeFromXml<typeWarenkorb>(xml))
            .HasMaxLength(50000); 

        entity.Ignore(e => e.Xml);


        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Basket> entity);
}


