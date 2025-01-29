using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using LibraryProject.classes;

public class BarChartGenerator
{
    private static void CreateBarChart(List<DataBaseBook> books)
    {
        int width = 600;
        int height = 400;
        int barWidth = 50;
        int maxTitleLength = 0;

        // Znajdź najdłuższy tytuł książki, aby dopasować odstępy
        foreach (DataBaseBook x in books)
        {
            if (x.Title.Length > maxTitleLength)
            {
                maxTitleLength = x.Title.Length;
            }
        }

        int spacing = maxTitleLength * 2; // Dostosowanie odstępu między słupkami do długości tytułu
        int margin = 50; // Margines
        int maxHeight = 200; // Maksymalna wysokość słupka

        // Zdobądź maksymalną liczbę wypożyczeń
        int maxBorrowCount = books.Max(b => b.borrowCount);

        // Posortuj książki od największej liczby wypożyczeń do najmniejszej
        var sortedBooks = books.OrderByDescending(b => b.borrowCount).ToList();

        using (Bitmap bmp = new Bitmap(width, height))
        using (Graphics g = Graphics.FromImage(bmp))
        {
            g.Clear(Color.White); // Tło wykresu

            Font font = new Font("Arial", 10);
            Brush textBrush = Brushes.Black;
            Brush barBrush = Brushes.Black;

            int x = margin;
            foreach (var book in sortedBooks)
            {
                int barHeight = (int)((book.borrowCount / (double)maxBorrowCount) * maxHeight); // Oblicz wysokość słupka
                int y = height - barHeight - margin;

                // Rysowanie słupka
                g.FillRectangle(barBrush, x, y, barWidth, barHeight);

                // Wyświetlanie liczby wypożyczeń nad słupkiem
                g.DrawString(book.borrowCount.ToString(), font, textBrush, x + barWidth / 4, y - 15);

                // Wyświetlanie tytułu książki pod słupkiem
                string[] titleParts = book.Title.Split(' '); // Dziel tytuł na części (człony)

                // Jeśli tytuł zawiera więcej niż 1 część, wyświetl ją na dwóch liniach
                if (titleParts.Length > 1)
                {
                    g.DrawString(titleParts[0], font, textBrush, x, height - margin + 5); // Pierwsza część tytułu
                    g.DrawString(string.Join(" ", titleParts.Skip(1)), font, textBrush, x, height - margin + 20); // Pozostała część
                }
                else
                {
                    g.DrawString(book.Title, font, textBrush, x, height - margin + 5); // Jeśli tytuł jest jednolity, wyświetl go na jednej linii
                }

                x += barWidth + spacing; // Zwiększ współrzędną x dla kolejnego słupka
            }

            bmp.Save($"..\\..\\..\\images\\iloscWypozyczenWKategorii{books[0].Category}.bmp"); // Zapisz plik BMP
        }
    }

    public static async Task generate()
    {
        // Wczytaj listę książek
        List<DataBaseBook> books = await LibrarySystem.LoadBooksAsync();
        // Utwórz wykres
        CreateBarChart(books);
    }

}