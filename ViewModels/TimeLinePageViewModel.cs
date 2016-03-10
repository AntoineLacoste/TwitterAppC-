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
            this.Selecteduser = (Tweetinvi.Logic.User) User.GetAuthenticatedUser();
            this.TimeLineTweets = getTimeLineObservableCollection(Selecteduser);
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
            set
            {
                Set(ref _timeLinetweets, this.getTimeLineObservableCollection(value));
                Set(ref _selecteduser, value);
            }
        }

        public ObservableCollection<Tweetinvi.Logic.Tweet> getTimeLineObservableCollection(Tweetinvi.Logic.User user)
        {
            var timeLine = Timeline.GetUserTimeline(user);
            var timeLineCollection = new ObservableCollection<Tweet>();
            timeLine = Timeline.GetUserTimeline("MisterVonline");
            foreach (var tweet in timeLine)
            {
                timeLineCollection.Add((Tweetinvi.Logic.Tweet) tweet);
            }

            return timeLineCollection;
        }

        private RelayCommand _authorizeCmd;
        public RelayCommand AuthorizeCmd
        {
            get
            {
                if (_authorizeCmd == null)
                    _authorizeCmd = new RelayCommand(GetPinConnection);
                return _authorizeCmd;
            }
        }

        public void GetPinConnection()
        {
            this.NavigationService.Navigate(typeof (Views.PinPage));
        }
        

        private RelayCommand _tweetsCmd;
        public RelayCommand TweetsCmd
        {
            get
            {
                if (_tweetsCmd == null)
                    _tweetsCmd = new RelayCommand(GetTweets);
                return _tweetsCmd;
            }
        }

        private ObservableCollection<Tweetinvi.Logic.Tweet> _timeLinetweets;
        public ObservableCollection<Tweetinvi.Logic.Tweet> TimeLineTweets
        {
            get { return _timeLinetweets; }
            set { Set(ref _timeLinetweets, value); }
        }

        public void GetTweets()
        {
            var loggedUser = User.GetAuthenticatedUser();
            var tweets = Timeline.GetHomeTimeline();
            var user = User.GetUserFromScreenName("AmandaCerny");
        }
    }
}

