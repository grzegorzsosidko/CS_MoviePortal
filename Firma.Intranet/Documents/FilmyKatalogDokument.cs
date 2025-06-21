using Firma.Data.Data.Movie;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Firma.Intranet.Documents
{
    public class FilmyKatalogDokument : IDocument
    {
        public IEnumerable<Film> Filmy { get; }

        public FilmyKatalogDokument(IEnumerable<Film> filmy)
        {
            Filmy = filmy;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container
                .Page(StronaTytulowa) // Najpierw definiuje stronę tytułową
                .Page(StronaZawartosci); // Potem stronę (lub strony) z treścią
        }

        // Metoda definiująca wygląd strony tytułowej
        void StronaTytulowa(PageDescriptor page)
        {
            page.Content()
                .PaddingVertical(50)
                .Column(column =>
                {
                    column.Spacing(30);
                    column.Item().AlignCenter().Text("Katalog Filmów").Bold().FontSize(36);
                    column.Item().AlignCenter().Text("MoviePortal").FontSize(18);
                    column.Item().AlignCenter().Image(Placeholders.Image(200, 100)); // Przykładowe miejsce na logo

                    column.Item().AlignBottom().AlignCenter().Text(text =>
                    {
                        text.Span("Raport wygenerowany: ").SemiBold();
                        text.Span(DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
                    });
                });
        }

        // Metoda definiująca wygląd strony z tabelą
        void StronaZawartosci(PageDescriptor page)
        {
            page.Header()
                .AlignCenter()
                .Text("Katalog filmów - MoviePortal")
                .SemiBold().FontSize(16).FontColor(Colors.Grey.Darken2);

            page.Content()
                .PaddingVertical(20)
                .Table(table =>
                {
                    // Definicja kolumn
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(4); // Tytuł i Gatunki
                        columns.RelativeColumn(1); // Rok
                        columns.RelativeColumn(1.5f); // Ocena
                        columns.RelativeColumn(3); // Reżyser
                    });

                    // Nagłówek tabeli
                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.Grey.Darken3).Padding(5).Text("Tytuł / Gatunki").FontColor(Colors.White);
                        header.Cell().Background(Colors.Grey.Darken3).Padding(5).AlignCenter().Text("Rok").FontColor(Colors.White);
                        header.Cell().Background(Colors.Grey.Darken3).Padding(5).AlignCenter().Text("Śr. ocena").FontColor(Colors.White);
                        header.Cell().Background(Colors.Grey.Darken3).Padding(5).Text("Reżyser").FontColor(Colors.White);
                    });

                    // Wiersze tabeli
                    foreach (var film in Filmy)
                    {
                        // Obliczam średnią ocenę, zabezpieczając się przed dzieleniem przez zero
                        var sredniaOcena = film.Recenzje.Any() ? film.Recenzje.Average(r => r.Ocena) : 0;

                        var cell = table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5);

                        // Komórka na Tytuł i Gatunki
                        cell.Column(column =>
                        {
                            column.Item().Text(film.Title).Bold();
                            // Dodaję "pigułki" z gatunkami pod tytułem
                            column.Item().Row(row =>
                            {
                                row.Spacing(5);
                                foreach (var gatunek in film.Gatunki.Take(3)) // Biorę max 3 gatunki, żeby nie zaśmiecać
                                {
                                    row.AutoItem().Badge(gatunek.Name, Colors.Blue.Lighten2);
                                }
                            });
                        });

                        // Pozostałe komórki
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).AlignCenter().Text(film.ReleaseYear);
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).AlignCenter().Text(sredniaOcena > 0 ? sredniaOcena.ToString("F2") : "Brak");
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(film.Reżyser != null ? $"{film.Reżyser.Imie} {film.Reżyser.Nazwisko}" : "Brak danych");
                    }
                });

            page.Footer()
                .AlignCenter()
                .Text(text => { text.Span("Strona "); text.CurrentPageNumber(); });
        }
    }

    // Prosta metoda rozszerzająca do tworzenia "pigułek"
    public static class CustomExtensions
    {
        public static void Badge(this IContainer container, string text, string color)
        {
            container
                .Background(color)
                .PaddingHorizontal(5).PaddingVertical(2)
                .Text(text.ToUpper()).FontSize(8).FontColor(Colors.White).SemiBold();
        }
    }
}