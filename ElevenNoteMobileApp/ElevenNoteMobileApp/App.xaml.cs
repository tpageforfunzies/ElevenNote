using ElevenNoteMobileApp.Contracts;
using ElevenNoteMobileApp.Pages;
using ElevenNoteMobileApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

// Define dependencies for injection.
[assembly: Xamarin.Forms.Dependency(typeof(FakeNoteService))]
namespace ElevenNoteMobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class App : Application
    {
        /// <summary>
        ///  Note service access. We set our dependencies above the namespace declaration.
        /// </summary>
        internal static readonly INoteService NoteService = DependencyService.Get<INoteService>();

        public App()
        {
            InitializeComponent();

            this.MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
