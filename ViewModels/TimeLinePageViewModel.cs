using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Command;
using Microsoft.Xaml.Interactivity;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Credentials;
using Tweetinvi;
using TwitterUniversalApp.Models;
using CredentialsCreator = Tweetinvi.CredentialsCreator;
using Tweet = Tweetinvi.Logic.Tweet;
using User = Tweetinvi.User;

namespace TwitterUniversalApp.ViewModels
{
    public class TimeLinePageViewModel : ViewModelBase
    {

        public TimeLinePageViewModel()
        {
            this.Selecteduser = (Tweetinvi.Logic.User) User.GetAuthenticatedUser();
            this.TimeLineTweets = getTimeLineObservableCollection(this.Selecteduser);
            this._headerText = "Welcome @"+this.Selecteduser.Name;
            this._nbCharacterTweet = 140;
            this._stringPostTweet = "Characters left : "+this._nbCharacterTweet;
        }

        private string _pinInput;
        public string PinInput
        {
            get { return _pinInput; }
            set { Set(ref _pinInput, value); }
        }
        

         private string _headerText;
        public string HeaderText
        {
            get { return _headerText; }
            set { Set(ref _headerText, value); }
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
            timeLine = Timeline.GetUserTimeline("AmandaCerny");
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

        private RelayCommand<string> _retweetCmd;
        public RelayCommand<string> RetweetCmd
        {
            get
            {
                if (_retweetCmd == null)
                    _retweetCmd = new RelayCommand<string>(Retweet);
                return _retweetCmd;
            }
        }

        public void Retweet(string idTweet)
        { 
            Tweetinvi.Tweet.PublishRetweet(Int64.Parse(idTweet));
        }

        private RelayCommand<string> _favoriteCmd;
        public RelayCommand<string> FavoriteCmd
        {
            get
            {
                if (_favoriteCmd == null)
                    _favoriteCmd = new RelayCommand<string>(Favorite);
                return _favoriteCmd;
            }
        }

        public void Favorite(string idTweet)
        {
            Tweetinvi.Tweet.FavoriteTweet(Int64.Parse(idTweet));
        }

        private RelayCommand<string> _deleteTweetCmd;
        public RelayCommand<string> DeleteTweetCmd
        {
            get
            {
                if (_deleteTweetCmd == null)
                    _deleteTweetCmd = new RelayCommand<string>(Delete);
                return _deleteTweetCmd;
            }
        }

        public void Delete(string idTweet)
        {
            var id = Int64.Parse(idTweet);
            var tweet = this._timeLinetweets.First(t => t.Id == id);
            if (tweet.CreatedBy == this._selecteduser)
            {
                this._timeLinetweets.Remove((Tweet)tweet);
                Tweetinvi.Tweet.DestroyTweet(id);
            }
        }

        private RelayCommand _postTweetCmd;
        public RelayCommand PostTweetCmd
        {
            get
            {
                if (_postTweetCmd == null)
                    _postTweetCmd = new RelayCommand(PostTweet);
                return _postTweetCmd;
            }
        }

        private int _nbCharacterTweet;

        private string _stringPostTweet;

        public string StringPostTweet
        {
            get { return _stringPostTweet; }
            set { Set(ref _stringPostTweet, value); }
        }

        public async void PostTweet()
        {
            var msg = new ContentDialog();

            var stackPanel = new StackPanel();

            var textBox = new TextBox();
            textBox.KeyUp += new KeyEventHandler(checkTweetCharacter);
            textBox.MaxLength = 140;
            stackPanel.Children.Add(textBox);

            var textBlock = new TextBlock
            {
                Text = this.StringPostTweet
            };
            var binding = new Binding();
            binding.Source = this;
            binding.Path = new PropertyPath("StringPostTweet");
            textBlock.SetBinding(TextBlock.TextProperty, binding);
            stackPanel.Children.Add(textBlock);

            msg.Content = stackPanel;

            msg.Title = "Post a tweet";
            msg.PrimaryButtonText = "Post";
            msg.SecondaryButtonText = "Cancel";
            var contentDialog = await msg.ShowAsync();
            switch (contentDialog)
            {
                case ContentDialogResult.Primary:
                    var a = 5;
                    break;
                case ContentDialogResult.Secondary:
                case ContentDialogResult.None:
                    break;
            }
        }

        private void checkTweetCharacter(object sender, KeyRoutedEventArgs e)
        {
            var textBox = (TextBox) sender;

            this._nbCharacterTweet = 140 - textBox.Text.Length;
            this.StringPostTweet = "Characters left : " + this._nbCharacterTweet;
        }
    }
}

