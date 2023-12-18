using MauiEFStress.ViewModel;

namespace MauiEFStress
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = new MainViewModel();
        }
    }
}