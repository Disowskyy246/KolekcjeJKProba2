
namespace KolekcjeJKProba2.Modele
{
    public class Kolekcja
    {
        public string nazwaKolekcji { get; set; }
        public int id { get; set; }

        public Kolekcja(int idconst, string nazwaconst)
        {
            id = idconst;
            nazwaKolekcji = nazwaconst;
        }
    }
}
