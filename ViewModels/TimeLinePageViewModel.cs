using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Tweet = Tweetinvi.Logic.Tweet;

namespace TwitterUniversalApp.ViewModels
{
    public class TimeLinePageViewModel : ViewModelBase
    {

        public TimeLinePageViewModel()
        {
            this.Selecteduser = (Tweetinvi.Logic.User)User.GetAuthenticatedUser();
            this.TimeLineTweets = new ObservableCollection<Tweet>();
            var timeLine = Timeline.GetUserTimeline(this.Selecteduser.UserIdentifier);
            foreach (var tweet in timeLine)
            {
                this.TimeLineTweets.Add((Tweetinvi.Logic.Tweet) tweet);
            }
        }

        private string _pinInput;
        public string PinInput
        {
            get { return _pinInput; }
            set { Set(ref _pinInput, value); }
        }

        private Tweetinvi.Logic.User _selecteduser;
        public Tweetinvi.Logic.User Selecteduser
        {
            get { return _selecteduser; }
            set { Set(ref _selecteduser, value); }
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
        

        private RelayCommand _tweetsCmd;
        public RelayCommand TweetsCMD
        {
            get
            {
                if (_tweetsCmd == null)
                    _tweetsCmd = new RelayCommand(getTweets);
                return _tweetsCmd;
            }
        }

        private ObservableCollection<Tweetinvi.Logic.Tweet> timeLinetweets;
        public ObservableCollection<Tweetinvi.Logic.Tweet> TimeLineTweets
        {
            get { return timeLinetweets; }
            set { Set(ref timeLinetweets, value); }
        }

        public void getTweets()
        {
            var loggedUser = User.GetAuthenticatedUser();
            var tweets = Timeline.GetHomeTimeline();
            var user = User.GetUserFromScreenName("AmandaCerny");
            var a = 5;
        }
    }
}

