using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Tweetinvi;
using TwitterUniversalApp.Models;

// Pour plus d'informations sur le modèle d'élément Page vierge, voir la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace TwitterUniversalApp.Views
{
    public sealed partial class PinPage : Page
    {
        public PinPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var url =
                CredentialsCreator.GetAuthorizationURL(TwitterConnectionInfoSingleton.getInstance().GetAppCredentials());

            TwitterPin.Source = new Uri(url);
        }
    }
}
