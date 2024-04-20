
namespace KolekcjeJKProba2.Modele
{
    class Przedmiot
    {
        public string nazwaPrzedmiotu { get; set; }
        public int iloscPrzedmiotu { get; set; }
        public int idKolekcja { get; set; }

        public Przedmiot(int idKolekcjaconst, string nazwaconst, int iloscPrzedmiotuconst)
        {
            idKolekcja = idKolekcjaconst;
            nazwaPrzedmiotu = nazwaconst;
            iloscPrzedmiotu = iloscPrzedmiotuconst;
            
        }
    }
}
