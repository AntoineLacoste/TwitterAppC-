using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Command;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Credentials;
using Tweetinvi;
using TwitterUniversalApp.Models;
using CredentialsCreator = Tweetinvi.CredentialsCreator;

namespace TwitterUniversalApp.ViewModels
{
    public class ConnectionPageViewModel : ViewModelBase
    {
        private string pinInput;
        public string PinInput
        {
            get { return pinInput; }
            set { Set(ref pinInput, value); }
        }

        private RelayCommand authorizeCmd;
        public RelayCommand AuthorizeCmd
        {
            get
            {
                if (authorizeCmd == null)
                    authorizeCmd = new RelayCommand(GetPinConnection);
                return authorizeCmd;
            }
        }

        public void GetPinConnection()
        {
            this.NavigationService.Navigate(typeof(Views.PinPage));
        }

        private RelayCommand connectionPinCmd;
        public RelayCommand ConnectionPinCmd
        {
            get
            {
                if (connectionPinCmd == null)
                    connectionPinCmd = new RelayCommand(connectPin);
                return connectionPinCmd;
            }
        }

        public void connectPin()
        {
            var userCredentials = CredentialsCreator.GetCredentialsFromVerifierCode(pinInput, TwitterConnectionInfoSingleton.getInstance().GetAppCredentials());
            Auth.SetCredentials(userCredentials);

            this.NavigationService.Navigate(typeof (Views.TimeLine));
        }
    }
}

