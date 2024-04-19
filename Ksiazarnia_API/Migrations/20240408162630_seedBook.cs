using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ksiazarnia_API.Migrations
{
    /// <inheritdoc />
    public partial class seedBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "Description", "GenreId", "Image", "Price", "Publisher", "Title" },
                values: new object[,]
                {
                    { 1, "Jarosław Grzędowicz", "Planeta powitała go mgłą i śmiercią. Dalej jest tylko gorzej...\r\n\r\nVuko Drakkainen ląduje samotnie na odległej, zamieszkanej przez człekopodobną cywilizację planecie Midgaard. Musi odnaleźć wysłaną tu wcześniej ziemską ekipę badawczą, pod żadnym pozorem nie ingerując w rozwój nieznanej kultury. Trafia na zły czas. Trwa wojna bogów. Giną śmiertelnicy. Być może zmuszony będzie złamać drugą regułę misji.\r\n\r\n\"Mnożąc realizm przez fantastykę, Grzędowicz osiąga efekt jeszcze większego autentyzmu: elementy fantastyczne nie niwelują ale wzmacniają, powiększają cechy charakterystyczne - niczym soczewki w mikroskopie.\" - Jacek Dukaj, Nowa Fantastyka", 1, "https://imagesbookstore.blob.core.windows.net/bookstore/PLO_1.png", 30.989999999999998, "Fabryka Słów", "Pan Lodowego Ogrodu Tom 1" },
                    { 2, "Samantha Shannon", "Autorka światowego bestsellera Zakon Drzewa Pomarańczy\r\n\r\nDzień nastania nocy to epicka opowieść rozgrywająca się w świecie Zakonu Drzewa Pomarańczy.\r\n\r\nTunuva Melim, siostra Zakonu, przez pięćdziesiąt lat przygotowywała się do walki z wyrmami – ale od czasu pokonania Bezimiennego na świecie nie pojawił się żaden z nich. Młode pokolenie zaczyna kwestionować zasadność istnienia Zakonu.\r\nNa północy Sabran Ambitna poślubia nowego króla Hróth, by ocalić chylące się ku upadkowi królowiectwo. Ich córka, Glorian, pozostaje w cieniu rodziców – zgodnie ze swoją intencją.\r\nSmoki Wschodu od wieków trwają w uśpieniu. Dumai zamieszkuje na Seiiki w górskiej świątyni, której wierni usiłują zbudzić bogów z Długiego Snu – jednak ktoś z przeszłości jej matki zmierza jej na spotkanie.\r\n\r\nGdy wraz z erupcją Góry Trwogi nastają czasy terroru i przemocy, kobiety muszą stawić im czoła i obronić ludzkość przed śmiertelnym zagrożeniem.\r\n\r\nDzień nastania nocy to wielowątkowa, napisana z epickim rozmachem opowieść osadzona w świecie Zakonu Drzewa Pomarańczy, przybliżająca wydarzenia, które zmieniły bieg jego historii.", 1, "https://imagesbookstore.blob.core.windows.net/bookstore/Dzien_nastania_nocy.png", 29.989999999999998, "Wydawnictwo SQN", "Dzień nastania nocy" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
