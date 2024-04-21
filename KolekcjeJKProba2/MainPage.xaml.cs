using KolekcjeJKProba2.Modele;
using System.Collections;
using System.Diagnostics;

namespace KolekcjeJKProba2
{
    public partial class MainPage : ContentPage
    {
        private List<Kolekcja> kolekcje = new List<Kolekcja>();
        private string sciezkaDoFolderu;

        public MainPage()
        {
            InitializeComponent();

            // Ścieżka do folderu
            sciezkaDoFolderu = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Kolekcje");

            // Sprawdzenie czy folder istnieje, jeśli nie, utwórz go
            if (!Directory.Exists(sciezkaDoFolderu))
            {
                Directory.CreateDirectory(sciezkaDoFolderu);
            }

            Wczytaj_Kolekcje();
        }

        private async void Dodaj_Clicked(object sender, EventArgs e)
        {
            var nazwaKolekcji = textInput.Text;

            if (string.IsNullOrWhiteSpace(nazwaKolekcji)) // Sprawdź, czy nazwa kolekcji jest pusta lub zawiera tylko białe znaki
            {
                await DisplayAlert("Błąd", "Nazwa kolekcji nie może być pusta", "OK");
                return;
            }

            var sciezkaDoPliku = Path.Combine(sciezkaDoFolderu, $"{nazwaKolekcji}.txt");

            if (File.Exists(sciezkaDoPliku))
            {
                await DisplayAlert("Ostrzeżenie!", "Istnieje już kolekcja o tej nazwie, wybierz inną nazwę", "OK");
                return;
            }

            // Sprawdź czy nazwa zawiera znaki specjalne
            if (!await SprawdzNazweKolekcji(nazwaKolekcji))
            {
                return; // Jeśli nazwa zawiera znaki specjalne, zakończ działanie funkcji
            }

            File.Create(sciezkaDoPliku).Dispose();
            kolekcje.Add(new Kolekcja(kolekcje.Count + 1, nazwaKolekcji));
            wyswietlenieKolekcji.ItemsSource = null; // Usuń stare źródło danych
            wyswietlenieKolekcji.ItemsSource = kolekcje; // Przypisz nowe źródło danych
        }



        private void Wczytaj_Kolekcje()
        {
            kolekcje.Clear(); // Wyczyść kolekcję przed ponownym wczytaniem, aby uniknąć duplikatów

            var listaKolekcji = Directory.GetFiles(sciezkaDoFolderu, "*.txt");
            foreach (var item in listaKolekcji)
            {
                var nazwaPliku = Path.GetFileNameWithoutExtension(item);
                kolekcje.Add(new Kolekcja(kolekcje.Count + 1, nazwaPliku));
            }
            wyswietlenieKolekcji.ItemsSource = kolekcje;
        }

        private async Task<bool> SprawdzNazweKolekcji(string nazwa)
        {
            var znakiSpecjalne = new char[] { '\\', '/', ':', '*', '?', '"', '<', '>', '|', ',', '.', '!', '@', '#', '$', '%', '^', '*', '(', ')', '`', '"', '~', '-', '+', '=', ' ',};

            if (nazwa.IndexOfAny(znakiSpecjalne) != -1)
            {
                await DisplayAlert("Błąd", "Nazwa kolekcji nie może zawierać znaków specjalnych", "OK");
                return false;
            }

            return true;
        }

        private async void Edytuj_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var kolekcja = button?.BindingContext as Kolekcja;

            if (kolekcja != null)
            {
                // Pobierz nową nazwę kolekcji od użytkownika
                string nowaNazwa = await DisplayPromptAsync("Edytuj nazwę", "Wprowadź nową nazwę dla kolekcji:", "OK", "Anuluj", kolekcja.nazwaKolekcji);
                if (nowaNazwa != null)
                {
                    // Znajdź ścieżkę do starego pliku
                    var staraNazwaPliku = $"{kolekcja.nazwaKolekcji}.txt";
                    var staraSciezkaDoPliku = Path.Combine(sciezkaDoFolderu, staraNazwaPliku);

                    // Znajdź ścieżkę do nowego pliku
                    var nowaNazwaPliku = $"{nowaNazwa}.txt";
                    var nowaSciezkaDoPliku = Path.Combine(sciezkaDoFolderu, nowaNazwaPliku);

                    // Sprawdź, czy plik z nową nazwą już istnieje
                    if (File.Exists(nowaSciezkaDoPliku))
                    {
                        await DisplayAlert("Ostrzeżenie!", "Istnieje już kolekcja o tej nazwie, wybierz inną nazwę", "OK");
                        return;
                    }

                    // Kopiuj zawartość starego pliku do nowego pliku
                    File.Copy(staraSciezkaDoPliku, nowaSciezkaDoPliku);

                    // Usuń stary plik
                    File.Delete(staraSciezkaDoPliku);

                    // Zaktualizuj nazwę kolekcji
                    kolekcja.nazwaKolekcji = nowaNazwa;

                    // Odśwież widok
                    wyswietlenieKolekcji.ItemsSource = null;
                    wyswietlenieKolekcji.ItemsSource = kolekcje;
                }
            }
        }
        private async void Wyswietl_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var kolekcja = button?.BindingContext as Kolekcja;

            if (kolekcja != null)
            {
                await Navigation.PushAsync(new StronaKolekcji(kolekcja.nazwaKolekcji));
            }
        }

        private void Usun_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var kolekcja = button?.BindingContext as Kolekcja;

            if (kolekcja != null)
            {
                var nazwaPliku = $"{kolekcja.nazwaKolekcji}.txt";
                var sciezkaDoPliku = Path.Combine(sciezkaDoFolderu, nazwaPliku);

                if (File.Exists(sciezkaDoPliku))
                {
                    File.Delete(sciezkaDoPliku);

                    // Usuwanie z listy kolekcji
                    kolekcje.Remove(kolekcja);

                    // Aktualizacja widoku
                    wyswietlenieKolekcji.ItemsSource = null;
                    wyswietlenieKolekcji.ItemsSource = kolekcje;
                }
                else
                {
                    DisplayAlert("Błąd", "Nie można odnaleźć pliku kolekcji do usunięcia", "OK");
                }
            }
            Wczytaj_Kolekcje();
        }
    }
}