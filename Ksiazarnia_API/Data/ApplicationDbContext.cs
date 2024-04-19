using Ksiazarnia_API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;


namespace Ksiazarnia_API.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    Id = 1,
                    Title = "Pan Lodowego Ogrodu Tom 1",
                    Author = "Jarosław Grzędowicz",
                    Description = "Planeta powitała go mgłą i śmiercią. Dalej jest tylko gorzej..." +
                    "\r\n\r\nVuko Drakkainen ląduje samotnie na odległej, zamieszkanej przez człekopodobną cywilizację planecie Midgaard." +
                    " Musi odnaleźć wysłaną tu wcześniej ziemską ekipę badawczą, pod żadnym pozorem nie ingerując w rozwój nieznanej kultury." +
                    " Trafia na zły czas. Trwa wojna bogów. Giną śmiertelnicy. Być może zmuszony będzie złamać drugą regułę misji." +
                    "\r\n\r\n\"Mnożąc realizm przez fantastykę, Grzędowicz osiąga efekt jeszcze większego autentyzmu: elementy fantastyczne nie niwelują ale wzmacniają, " +
                    "powiększają cechy charakterystyczne - niczym soczewki w mikroskopie.\" - Jacek Dukaj, Nowa Fantastyka",
                    Genre = "Fantasy",
                    Publisher = "Fabryka Słów",
                    Price = 30.99,
                    Image = "https://imagesbookstore.blob.core.windows.net/bookstore/PLO_1.png"
                },
                new Book
                {
                    Id = 2,
                    Title = "Dzień nastania nocy",
                    Author = "Samantha Shannon",
                    Description = "Autorka światowego bestsellera Zakon Drzewa Pomarańczy\r\n\r\n" +
                    "Dzień nastania nocy to epicka opowieść rozgrywająca się w świecie Zakonu Drzewa Pomarańczy." +
                    "\r\n\r\nTunuva Melim, siostra Zakonu, przez pięćdziesiąt lat przygotowywała się do walki z wyrmami – ale od czasu pokonania Bezimiennego na świecie nie" +
                    " pojawił się żaden z nich. Młode pokolenie zaczyna kwestionować zasadność istnienia Zakonu.\r\nNa północy Sabran Ambitna poślubia nowego króla Hróth, by" +
                    " ocalić chylące się ku upadkowi królowiectwo. Ich córka, Glorian, pozostaje w cieniu rodziców – zgodnie ze swoją intencją.\r\nSmoki Wschodu od wieków trwają" +
                    " w uśpieniu. Dumai zamieszkuje na Seiiki w górskiej świątyni, której wierni usiłują zbudzić bogów z Długiego Snu – jednak ktoś z przeszłości jej matki zmierza" +
                    " jej na spotkanie.\r\n\r\nGdy wraz z erupcją Góry Trwogi nastają czasy terroru i przemocy, kobiety muszą stawić im czoła i obronić ludzkość przed śmiertelnym" +
                    " zagrożeniem.\r\n\r\nDzień nastania nocy to wielowątkowa, napisana z epickim rozmachem opowieść osadzona w świecie Zakonu Drzewa Pomarańczy, przybliżająca " +
                    "wydarzenia, które zmieniły bieg jego historii.",
                    Genre = "Fantasy",
                    Publisher = "Wydawnictwo SQN",
                    Price = 29.99,
                    Image = "https://imagesbookstore.blob.core.windows.net/bookstore/Dzien_nastania_nocy.png"
                });
        }
    }
}
