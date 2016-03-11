using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Command;
using Tweetinvi;
using TwitterUniversalApp.Models;
using TwitterUniversalApp.Service;

namespace TwitterUniversalApp.ViewModels
{
    public class ConnectionPageViewModel : ViewModelBase
    {
        private string _pinInput;
        public string PinInput
        {
            get { return _pinInput; }
            set { Set(ref _pinInput, value); }
        }

        private RelayCommand _getPinCmd;
        public RelayCommand GetPinCmd
        {
            get
            {
                if (_getPinCmd == null)
                    _getPinCmd = new RelayCommand(GetPinConnection);
                return _getPinCmd;
            }
        }

        public void GetPinConnection()
        {
            this.NavigationService.Navigate(typeof(Views.PinPage));
        }

        private RelayCommand _connectionPinCmd;
        public RelayCommand ConnectionPinCmd
        {
            get
            {
                if (_connectionPinCmd == null)
                    _connectionPinCmd = new RelayCommand(ConnectPin);
                return _connectionPinCmd;
            }
        }

        public async void ConnectPin()
        {
            if (File.Exists(ApplicationData.Current.LocalFolder.Path + "\\config.json"))
            {
                var tokens = AccountToken.ReadTokens();
                var userCredentials = Auth.CreateCredentials(TwitterConnectionInfoSingleton.getInstance().getConsumerKey(), TwitterConnectionInfoSingleton.getInstance().getConsumerSecret(), tokens.token, tokens.tokenSecret);
                Auth.SetCredentials(userCredentials);
                this.NavigationService.Navigate(typeof(Views.TimeLine));
            }
            if (!string.IsNullOrEmpty(_pinInput))
            {
                var userCredentials = CredentialsCreator.GetCredentialsFromVerifierCode(PinInput, TwitterConnectionInfoSingleton.getInstance().GetAppCredentials());

                if (userCredentials != null)
                {
                    Auth.SetCredentials(userCredentials);
                    var account = new Token(userCredentials.AccessToken, userCredentials.AccessTokenSecret);
                    AccountToken.SaveAccountData(account);
                    this.NavigationService.Navigate(typeof(Views.TimeLine));
                }
                else
                {
                    var msgDialogue = new MessageDialog("Pin code invalid", "Connection error");
                    await msgDialogue.ShowAsync();
                }
            }

        }
    }
}

