namespace KolekcjeJKProba2.Kontrolki;

public partial class KontrolkaKolekcji : ContentView
{
	public static readonly BindableProperty nazwaKolekcjiPropety =
		BindableProperty.Create(nameof(nazwaKolekcji), typeof(string), typeof(KontrolkaKolekcji), string.Empty);
	public static readonly BindableProperty idProperty = 
		BindableProperty.Create(nameof(id), typeof(int), typeof(KontrolkaKolekcji), null);


	public string nazwaKolekcji
	{
		get => (string)GetValue(nazwaKolekcjiPropety);
		set => SetValue(nazwaKolekcjiPropety, value);
	}

    public string id
    {
        get => (string)GetValue(idProperty);
        set => SetValue(idProperty, value);
    }



    public KontrolkaKolekcji()
	{
		InitializeComponent();
	}
}