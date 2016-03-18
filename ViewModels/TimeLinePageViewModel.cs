using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Template10.Mvvm;
using GalaSoft.MvvmLight.Command;
using Tweetinvi;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Parameters;
using Tweet = Tweetinvi.Logic.Tweet;
using User = Tweetinvi.User;

namespace TwitterUniversalApp.ViewModels
{
    public class TimeLinePageViewModel : ViewModelBase
    {
        private List<Tweetinvi.Core.Interfaces.DTO.IMedia> MediasTweet;

        public TimeLinePageViewModel()
        {
            this.Selecteduser = (Tweetinvi.Logic.User) User.GetAuthenticatedUser();
            this.TimeLineTweets = getTimeLineObservableCollection(this.Selecteduser);
            this._headerText = "Welcome @"+this.Selecteduser.Name;
            this._nbCharacterTweet = 140;
            this._stringPostTweet = "Characters left : "+this._nbCharacterTweet;
            this.MediasTweet = new List<Tweetinvi.Core.Interfaces.DTO.IMedia>();
        }

        private string _userNameInput;
        public string UserNameInput
        {
            get { return _userNameInput; }
            set { Set(ref _userNameInput, value); }
        }

        private string _searchInput;
        public string SearchInput
        {
            get { return _searchInput; }
            set { Set(ref _searchInput, value); }
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
                Set(ref _selecteduser, value);
            }
        }

        private ObservableCollection<Tweet> _timeLineTweets;

        public ObservableCollection<Tweet> TimeLineTweets
        {
            get { return _timeLineTweets; }
            set
            {
                Set(ref _timeLineTweets, value);
            }
        }
        public ObservableCollection<Tweetinvi.Logic.Tweet> getTimeLineObservableCollection(Tweetinvi.Logic.User user)
        {
            IEnumerable<Tweetinvi.Core.Interfaces.ITweet> timeLine;
            if (User.GetAuthenticatedUser().ScreenName == this.Selecteduser.ScreenName)
            {
                timeLine = Timeline.GetHomeTimeline();
            }
            else
            {
                timeLine = Timeline.GetUserTimeline(user);
            }
            var timeLineCollection = new ObservableCollection<Tweet>();
            timeLine = timeLine.Where(t => t.InReplyToScreenName == null).ToList();
            //timeLine = Timeline.GetUserTimeline("AmandaCerny");
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
            this.NavigationService.Navigate(typeof(Views.PinPage));
        }

        private RelayCommand _searchCmd;
        public RelayCommand SearchCmd
        {
            get
            {
                if (_searchCmd == null)
                    _searchCmd = new RelayCommand(SearchTwitter);
                return _searchCmd;
            }
        }

        private void SearchTwitter()
        {
            if (!string.IsNullOrEmpty(this._searchInput))
            {
                var tweets = Search.SearchTweets(this._searchInput);
                var timeLineCollection = new ObservableCollection<Tweet>();
                foreach (var tweet in tweets)
                {
                    timeLineCollection.Add((Tweetinvi.Logic.Tweet)tweet);
                }
                this.TimeLineTweets = timeLineCollection;
                this._searchInput = "";
            }
        }

        private RelayCommand _switchUserTimeLine;
        public RelayCommand SwitchUserTimeLine
        {
            get
            {
                if (_switchUserTimeLine == null)
                    _switchUserTimeLine = new RelayCommand(SetUserTimeLine);
                return _switchUserTimeLine;
            }
        }

        private void SetUserTimeLine()
        {
            if (!String.IsNullOrEmpty(this._userNameInput))
            {
                var user = (Tweetinvi.Logic.User) User.GetUserFromScreenName(this._userNameInput);
                if (user != null)
                {
                    this.Selecteduser = user;
                    this.TimeLineTweets = this.getTimeLineObservableCollection(this.Selecteduser);
                    this._userNameInput = "";
                }
            }
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
            var tweetId = long.Parse(idTweet);

            var tweet = Tweetinvi.Tweet.GetTweet(tweetId);
            if (tweet.Retweeted)
            {
                Tweetinvi.Tweet.UnRetweet(tweetId);
            }
            else
            {
                Tweetinvi.Tweet.PublishRetweet(tweetId);
            }
            
            tweet = Tweetinvi.Tweet.GetTweet(tweetId);
            var index = this.TimeLineTweets.IndexOf((this.TimeLineTweets.First(t => t.Id == tweetId)));
            this.TimeLineTweets[index] = (Tweet)tweet;
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
            var tweetId = long.Parse(idTweet);
            var tweet = Tweetinvi.Tweet.GetTweet(tweetId);

            if (tweet.Retweeted)
            {
                Tweetinvi.Tweet.UnFavoriteTweet(tweetId);
            }
            else
            {
                Tweetinvi.Tweet.FavoriteTweet(tweetId);
            }

            tweet =Tweetinvi.Tweet.GetTweet(tweetId);
            var index = this.TimeLineTweets.IndexOf((this.TimeLineTweets.First(t => t.Id == tweetId)));
            this.TimeLineTweets[index] = (Tweet) tweet;
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
            var id = long.Parse(idTweet);
            var tweet = this._timeLineTweets.First(t => t.Id == id);
            if (tweet.CreatedBy.ScreenName == this._selecteduser.ScreenName)
            {
                this._timeLineTweets.Remove((Tweet)tweet);
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

        private RelayCommand _addMediaCmd;
        public RelayCommand AddMediaCmd
        {
            get
            {
                if (_addMediaCmd == null)
                    _addMediaCmd = new RelayCommand(AddMedia);
                return _addMediaCmd;
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
            var binding = new Binding
            {
                Source = this,
                Path = new PropertyPath("StringPostTweet")
            };
            textBlock.SetBinding(TextBlock.TextProperty, binding);
            stackPanel.Children.Add(textBlock);

            var buttonImage = new Button
            {
                Command = this.AddMediaCmd,
                Content = "Add a media",
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            stackPanel.Children.Add(buttonImage);

            msg.Content = stackPanel;

            msg.Title = "Post a tweet";
            msg.PrimaryButtonText = "Post";
            msg.SecondaryButtonText = "Cancel";
            var contentDialog = await msg.ShowAsync();
            switch (contentDialog)
            {
                case ContentDialogResult.Primary:
                    var medias = this.MediasTweet;
                    if (!string.IsNullOrEmpty(textBox.Text))
                    {
                        var tweet = Tweetinvi.Tweet.PublishTweet(textBox.Text, new PublishTweetOptionalParameters
                        {
                            Medias = medias
                        });
                        this._nbCharacterTweet = 140;
                        this.MediasTweet = new List<IMedia>();
                    }
                    else
                    {
                        this.PostTweet();
                    }
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

        private async void AddMedia()
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker
            {
                ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail,
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary
            };
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".mp4");
            picker.FileTypeFilter.Add(".avi");
            picker.FileTypeFilter.Add(".wmv");

            var file = await picker.PickSingleFileAsync();
            await Task.Factory.StartNew(async () => {
                var stream = await file.OpenStreamForReadAsync();
                var file1 = new byte[(int)stream.Length];
                stream.Read(file1, 0, (int)stream.Length);
                this.MediasTweet.Add(Upload.UploadImage(file1));
            });
        }
        private RelayCommand<string> _answerTweetCmd;

        public RelayCommand<string> AnswerTweetCmd
        {
            get
            {
                if (_answerTweetCmd == null)
                    _answerTweetCmd = new RelayCommand<string>(AnswerTweet);
                return _answerTweetCmd;
            }
        }


        private async void AnswerTweet(string tweetId)
        {
            var tweetToReplyTo = Tweetinvi.Tweet.GetTweet(long.Parse(tweetId));

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
            var binding = new Binding
            {
                Source = this,
                Path = new PropertyPath("StringPostTweet")
            };
            textBlock.SetBinding(TextBlock.TextProperty, binding);
            stackPanel.Children.Add(textBlock);

            var buttonImage = new Button
            {
                Command = this.AddMediaCmd,
                Content = "Add a media",
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            stackPanel.Children.Add(buttonImage);

            msg.Content = stackPanel;

            msg.Title = "Answer the tweet of @"+ tweetToReplyTo.CreatedBy.Name;
            msg.PrimaryButtonText = "Answer";
            msg.SecondaryButtonText = "Cancel";
            var contentDialog = await msg.ShowAsync();
            switch (contentDialog)
            {
                case ContentDialogResult.Primary:
                    var medias = this.MediasTweet;
                    if (!string.IsNullOrEmpty(textBox.Text))
                    {
                        var textToPublish = string.Format("@{0} {1}", tweetToReplyTo.CreatedBy.ScreenName, textBox.Text, new PublishTweetOptionalParameters
                        {
                            Medias = medias
                        });
                        Tweetinvi.Tweet.PublishTweetInReplyTo(textToPublish, long.Parse(tweetId));
                        this._nbCharacterTweet = 140;
                        this.MediasTweet = new List<IMedia>();
                    }
                    else
                    {
                        this.PostTweet();
                    }
                    break;
                case ContentDialogResult.Secondary:
                case ContentDialogResult.None:
                    break;
            }
        }

        public void Scrolled(ScrollViewer scrollViewer)
        {
            if (scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight - 100)
            {
                this.AddTimeLineTweets();
            }
        }

        private void AddTimeLineTweets()
        {
            IEnumerable<Tweetinvi.Core.Interfaces.ITweet> tweets;
            if (User.GetAuthenticatedUser().ScreenName == this.Selecteduser.ScreenName)
            {
                var homeTimelineParameter = new HomeTimelineParameters
                {
                    MaxId = this.TimeLineTweets.Last().Id,
                    MaximumNumberOfTweetsToRetrieve = 15
                };
                tweets = Timeline.GetHomeTimeline(homeTimelineParameter);
            }
            else
            {
                var userTimelineParameters = new UserTimelineParameters
                {
                    MaxId = this.TimeLineTweets.Last().Id,
                    MaximumNumberOfTweetsToRetrieve = 15
                };
                tweets = Timeline.GetUserTimeline(this.Selecteduser.Id, userTimelineParameters);
            }
            var tweetsList = tweets.ToList();
            tweetsList.RemoveAt(0);
            foreach (var tweet in tweetsList)
            {
                this.TimeLineTweets.Add((Tweet) tweet);
            }
        }
    }
}

