using Windows.UI.Xaml.Controls;
using TwitterUniversalApp.ViewModels;

// Pour plus d'informations sur le modèle d'élément Page vierge, voir la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace TwitterUniversalApp.Views
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class TimeLine : Page
    {
        public TimeLine()
        {
            this.InitializeComponent();
            TimeLineList.IsSwipeEnabled = false;
            TimeLineList.IsTapEnabled = false;
            TimeLineList.IsDoubleTapEnabled = false;
            TimeLineScroll.ViewChanged += scrolled;

        }

        private void scrolled(object sender, ScrollViewerViewChangedEventArgs scrollViewerViewChangedEventArgs)
        {
            var data = (TimeLinePageViewModel)DataContext;
            var scrollView = (ScrollViewer)sender;
            data.Scrolled(scrollView);
        }
    }
}
