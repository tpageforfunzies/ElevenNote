using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ElevenNoteMobileApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
        }

        private async void BtnLogin_OnClicked(object sender, EventArgs e)
        {
            // Make sure they filled all the fields.
            if (string.IsNullOrWhiteSpace(fldUsername.Text) || string.IsNullOrWhiteSpace(fldPassword.Text))
            {
                await DisplayAlert("Whoops", "Please enter a username and password.", "Okie Dokie");
                return;
            }

            // Turn on the "please wait" spinner.
            pleaseWait.IsRunning = true;
            fldUsername.IsEnabled = false;
            fldPassword.IsEnabled = false;
            btnLogin.IsEnabled = false;

            // Attempt to log in.
            await App.NoteService.Login(fldUsername.Text.Trim(), fldPassword.Text)
                .ContinueWith(async (task) =>
                {
                    // Get the result.
                    var loggedIn = task.Result;

                    // Let them know if login failed.
                    if (!loggedIn)
                    {
                        await DisplayAlert("Whoops", "Login failed.", "Okie Dokie");
                        fldUsername.IsEnabled = true;
                        fldPassword.IsEnabled = true;
                        btnLogin.IsEnabled = true;
                        pleaseWait.IsRunning = false;
                        return;
                    }

                    // If login was successful, send them to the notes list page.
                    pleaseWait.IsRunning = false;
                    await Navigation.PushAsync(new NotesPage(), true);

                }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}