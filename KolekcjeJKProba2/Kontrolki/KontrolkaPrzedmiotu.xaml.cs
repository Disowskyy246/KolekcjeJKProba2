using System.Windows.Input;

namespace KolekcjeJKProba2.Kontrolki
{
    public partial class KontrolkaPrzedmiotu : ContentView
    {
        public static readonly BindableProperty nazwaPrzedmiotuProperty =
            BindableProperty.Create(nameof(nazwaPrzedmiotu), typeof(string), typeof(KontrolkaPrzedmiotu), string.Empty);
        public static readonly BindableProperty iloscPrzedmiotuProperty =
            BindableProperty.Create(nameof(iloscPrzedmiotu), typeof(int), typeof(KontrolkaPrzedmiotu), null);
        public static readonly BindableProperty idKolekcjaProperty =
            BindableProperty.Create(nameof(idKolekcja), typeof(int), typeof(KontrolkaPrzedmiotu), null);

        public string nazwaPrzedmiotu
        {
            get => (string)GetValue(nazwaPrzedmiotuProperty);
            set => SetValue(nazwaPrzedmiotuProperty, value);
        }

        public int iloscPrzedmiotu
        {
            get => (int)GetValue(iloscPrzedmiotuProperty);
            set => SetValue(iloscPrzedmiotuProperty, value);
        }


        public int idKolekcja
        {
            get => (int)GetValue(idKolekcjaProperty);
            set => SetValue(idKolekcjaProperty, value);
        }

        public KontrolkaPrzedmiotu()
        {
            InitializeComponent();
        }
    }
}
