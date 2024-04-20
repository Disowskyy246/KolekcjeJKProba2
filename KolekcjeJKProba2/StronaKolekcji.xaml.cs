using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using KolekcjeJKProba2.Modele;
using Microsoft.Maui.Controls;

namespace KolekcjeJKProba2
{
    public partial class StronaKolekcji : ContentPage
    {
        private string nazwaPliku;
        private ObservableCollection<Przedmiot> listaPrzedmiotow;
        private int ostatniIndeks = 0; // Pole przechowuj¹ce ostatni indeks przedmiotu

        public StronaKolekcji(string nazwaPliku)
        {
            InitializeComponent();
            this.nazwaPliku = nazwaPliku;
            listaPrzedmiotow = new ObservableCollection<Przedmiot>(); // Inicjalizacja ObservableCollection
            BindingContext = listaPrzedmiotow;
            WyswietlKolekcje();
        }

        private async void WyswietlKolekcje()
        {
            var sciezkaDoPliku = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Kolekcje", $"{nazwaPliku}.txt");

            if (File.Exists(sciezkaDoPliku))
            {
                var lines = await File.ReadAllLinesAsync(sciezkaDoPliku);

                foreach (var line in lines)
                {
                    string[] parts = line.Split('^'); // Rozdzielamy liniê na czêœci po znaku '^'

                    if (parts.Length >= 3) // Sprawdzamy, czy mamy wszystkie potrzebne dane
                    {
                        int id = int.Parse(parts[0]);
                        string nazwa = parts[1];
                        int ilosc = int.Parse(parts[2]);

                        listaPrzedmiotow.Add(new Przedmiot(id, nazwa, ilosc));
                        ostatniIndeks = Math.Max(ostatniIndeks, id);
                    }
                }

                listaElementow.ItemsSource = listaPrzedmiotow; // Przypisanie ItemsSource
            }
            else
            {
                await DisplayAlert("B³¹d", "Nie mo¿na odnaleŸæ pliku kolekcji do wyœwietlenia", "OK");
            }
        }

        private async void DodajElement_Clicked(object sender, EventArgs e)
        {
            string nowyElement = nowyElementEntry.Text;

            if (string.IsNullOrWhiteSpace(nowyElement))
            {
                await DisplayAlert("B³¹d", "WprowadŸ nazwê nowego elementu", "OK");
                return;
            }

            // Sprawdzamy, czy nazwa przedmiotu zawiera znaki specjalne
            if (await SprawdzNazwePrzedmiotu(nowyElement))
            {
                string sciezkaDoPliku = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Kolekcje", $"{nazwaPliku}.txt");

                try
                {
                    // Inkrementacja ostatniego indeksu
                    ostatniIndeks++;

                    using (StreamWriter writer = File.AppendText(sciezkaDoPliku))
                    {
                        int ilosc = 1; // Domyœlnie ustawiamy iloœæ na 1
                        writer.WriteLine($"{ostatniIndeks}^{nowyElement}^{ilosc}"); // Zapisujemy nowy element w formacie: id|nazwa|ilosc
                    }

                    // Dodaj nowy przedmiot do listy
                    listaPrzedmiotow.Add(new Przedmiot(ostatniIndeks, nowyElement, 1));
                    nowyElementEntry.Text = ""; // Wyczyœæ pole tekstowe po dodaniu elementu
                }
                catch (Exception ex)
                {
                    await DisplayAlert("B³¹d", $"Wyst¹pi³ b³¹d podczas dodawania elementu: {ex.Message}", "OK");
                }
            }
        }



        private async Task<bool> SprawdzNazwePrzedmiotu(string nazwa)
        {
            // Lista znaków specjalnych
            char[] znakiSpecjalne = { '~', '`', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '_', '-', '+', '=', '{', '}', '[', ']', '\\', '|', ';', ':', '\'', '"', '<', '>', ',', '.', '?', '/' };

            if (nazwa.Any(znakiSpecjalne.Contains))
            {
                await DisplayAlert("B³¹d", "Nazwa przedmiotu nie mo¿e zawieraæ nastêpuj¹cych znaków: " + string.Join(", ", znakiSpecjalne), "OK");
                return false;
            }

            return true;
        }

        private async void Odejmij_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var przedmiot = button.BindingContext as Przedmiot;
                if (przedmiot != null && przedmiot.iloscPrzedmiotu > 0)
                {
                    przedmiot.iloscPrzedmiotu--;

                    string sciezkaDoPliku = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Kolekcje", $"{nazwaPliku}.txt");

                    var lines = await File.ReadAllLinesAsync(sciezkaDoPliku);
                    var modifiedLines = lines.Select(line =>
                    {
                        var parts = line.Split('^');
                        if (parts.Length >= 3 && parts[0] == przedmiot.idKolekcja.ToString())
                        {
                            int ilosc = int.Parse(parts[2]);
                            if (ilosc > 0)
                            {
                                parts[2] = (ilosc - 1).ToString();
                            }
                        }
                        return string.Join("^", parts);
                    });

                    await File.WriteAllLinesAsync(sciezkaDoPliku, modifiedLines);
                    listaElementow.ItemsSource = null;
                    listaElementow.ItemsSource = listaPrzedmiotow;
                }
            }
        }


        private async void Dodaj_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var przedmiot = button.BindingContext as Przedmiot;
                if (przedmiot != null)
                {
                    przedmiot.iloscPrzedmiotu++;

                    string sciezkaDoPliku = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Kolekcje", $"{nazwaPliku}.txt");

                    var lines = await File.ReadAllLinesAsync(sciezkaDoPliku);
                    var modifiedLines = lines.Select(line =>
                    {
                        var parts = line.Split('^');
                        if (parts.Length >= 3 && parts[0] == przedmiot.idKolekcja.ToString())
                        {
                            parts[2] = (int.Parse(parts[2]) + 1).ToString();
                            return string.Join("^", parts);
                        }
                        return line;
                    });

                    await File.WriteAllLinesAsync(sciezkaDoPliku, modifiedLines);
                    listaElementow.ItemsSource = null;
                    listaElementow.ItemsSource = listaPrzedmiotow;
                }
            }
        }

        private async void Usun_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var przedmiot = button.BindingContext as Przedmiot;
                if (przedmiot != null)
                {
                    listaPrzedmiotow.Remove(przedmiot);

                    string sciezkaDoPliku = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Kolekcje", $"{nazwaPliku}.txt");

                    var lines = await File.ReadAllLinesAsync(sciezkaDoPliku);
                    var modifiedLines = lines.Where(line =>
                    {
                        var parts = line.Split('^');
                        return !(parts.Length >= 1 && parts[0] == przedmiot.idKolekcja.ToString());
                    });

                    await File.WriteAllLinesAsync(sciezkaDoPliku, modifiedLines);
                    listaElementow.ItemsSource = null;
                    listaElementow.ItemsSource = listaPrzedmiotow;
                }
            }
        }
    }
}
