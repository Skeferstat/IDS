using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static System.Net.WebRequestMethods;

namespace IdsServer.Database.Models.Configurations;
public partial class FakeArticleConfiguration : IEntityTypeConfiguration<FakeArticle>
{
    public void Configure(EntityTypeBuilder<FakeArticle> entity)
    {
        entity.ToTable("FakeArticle");
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        entity.Property(e => e.ArticleNumber)
            .IsRequired()
            .HasMaxLength(30);

        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        entity.Property(e => e.Description)
            .HasMaxLength(300);

        entity.Property(e => e.NetPrice).HasPrecision(2);
        entity.Property(e => e.OfferPrice).HasPrecision(2);

        entity.Property(e => e.ImageUrl)
            .HasMaxLength(300);


        entity.HasData(
            new FakeArticle
            {
                Id = Guid.Parse("fd7aed7f-7ef6-409b-a34f-da22a663aae5"),
                ArticleNumber = "1085922350000",
                Name = "filet mignon shank drumstick",
                Description = "Bacon ipsum dolor amet filet mignon shank drumstick sausage, cow sirloin rump corned beef tongue chicken kevin. Salami ham corned beef, kevin cow fatback bresaola sausage jerky hamburger tail capicola. Biltong pork jerky fatback venison pig. Jowl strip steak short ribs ham spare ribs, swine jerky pork belly frankfurter alcatra pork picanha boudin tongue beef. Doner meatloaf ribeye prosciutto tri-tip capicola. Landjaeger ham hock swine corned beef ground round tail biltong salami pastrami pork belly. Jerky shank landjaeger biltong, doner porchetta chicken pancetta shankle shoulder sausage pork chop corned beef burgdoggen.",
                NetPrice = 200.67m,
                OfferPrice = 237.99m,
                ImageUrl = "https://placehold.co/500x400"
            },
            new FakeArticle
            {
                Id = Guid.Parse("c3e89e64-9f00-4dc0-8adf-55223601d9f6"),
                ArticleNumber = "24485918350056",
                Name = "Bresaola picanha landjaeger",
                Description = "Bresaola picanha landjaeger prosciutto ball tip burgdoggen pork belly. Swine ham hock salami spare ribs turducken venison tri-tip biltong t-bone prosciutto jowl beef ribs turkey beef shank. Hamburger picanha strip steak, pork chop buffalo tenderloin corned beef ball tip jowl sirloin bacon pork. Ribeye kevin shankle, tongue short loin beef ribs pancetta tenderloin tri-tip short ribs. Strip steak brisket rump, boudin ham landjaeger chuck spare ribs.",
                NetPrice = 100.12m,
                OfferPrice = 123.00m,
                ImageUrl = "https://placehold.co/500x400"
            }
        );

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<FakeArticle> entity);
}


