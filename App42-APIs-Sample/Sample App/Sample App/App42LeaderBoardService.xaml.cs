using Sample_App.Common;
using Sample_App.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using com.shephertz.app42.paas.sdk.windows;
using com.shephertz.app42.paas.sdk.windows.user;
using com.shephertz.app42.paas.sdk.windows.game;
using Newtonsoft.Json.Linq;

// The Item Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234232

namespace Sample_App
{
    /// <summary>
    /// A page that displays details for a single item within a group.
    /// </summary>
    public sealed partial class App42LeaderBoardService : Page,App42Callback
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        public String gameName = null;
        public GameService gameService = null;
        public ScoreBoardService leaderBoardService = null;
        public Boolean scoreIdValue = false;
        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public App42LeaderBoardService()
        {
            gameName = Constant.gameName;
            gameService = App42API.BuildGameService();
            leaderBoardService = App42API.BuildScoreBoardService();
            if (Constant.sessionId != null && Constant.sessionId != "")
            {
                gameService.SetSessionId(Constant.sessionId);
                leaderBoardService.SetSessionId(Constant.sessionId);
            }
            if (Constant.adminKey != null && Constant.adminKey != "")
            {
                gameService.SetAdminKey(Constant.adminKey);
                leaderBoardService.SetAdminKey(Constant.adminKey);
            }
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            var item = await SampleDataSource.GetItemAsync((String)e.NavigationParameter);
            this.DefaultViewModel["Item"] = item;
        }
        public void OnSuccess(Object response)
        {
            DISPATCHER.ExecuteAsync(delegate()
            {
                if (response is Game)
                {
                    Game game = (Game)response;
                    LeaderBoardResponseTBL.Text = "LeaderBoard Response is :" + game;

                    if (scoreIdValue)
                    {
                        scoreIdValue = false;
                        Constant.scoreId = game.GetScoreList()[0].GetScoreId();
                    }
                }
            });

        }
        public void OnException(App42Exception exception)
        {
            scoreIdValue = false;
            DISPATCHER.ExecuteAsync(delegate()
            {
                LeaderBoardResponseTBL.Text = "Exception is : " + exception;
            });
        }
        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void create_game(object sender, RoutedEventArgs e)
        {
            gameService.CreateGame(gameName, "Game Has SuccessFully Created", this);
        }

        private void get_all_games(object sender, RoutedEventArgs e)
        {
            gameService.GetAllGames(this);
        }

        private void save_user_score(object sender, RoutedEventArgs e)
        {
            scoreIdValue = true;
            leaderBoardService.SaveUserScore(gameName, Constant.userName, 1000, this);
            leaderBoardService.SaveUserScore(gameName, "Himanshu", 1000, this);
            leaderBoardService.SaveUserScore(gameName, "Ajay", 1000, this);
        }

        private void saving_addtion_data(object sender, RoutedEventArgs e)
        {
            App42API.SetDbName(Constant.dbName);
            JObject jsonObj = new JObject();
            jsonObj.Add("FirstName", "Nick");
            jsonObj.Add("LastName", "Sharma");
            jsonObj.Add("Age", 24);
            leaderBoardService.AddJSONObject(Constant.collectionName, jsonObj);
            leaderBoardService.SaveUserScore(gameName, Constant.userName, 1000, this);
        }


        private void get_average_score(object sender, RoutedEventArgs e)
        {
            leaderBoardService.GetAverageScoreByUser(gameName, Constant.userName, this);
        }
        private void get_highest_score(object sender, RoutedEventArgs e)
        {
            leaderBoardService.GetHighestScoreByUser(gameName, Constant.userName, this);
        }

        private void get_lowest_score(object sender, RoutedEventArgs e)
        {
            leaderBoardService.GetLowestScoreByUser(gameName, Constant.userName, this);
        }

        private void get_last_game_score(object sender, RoutedEventArgs e)
        {
            leaderBoardService.GetLastGameScore(Constant.userName, this);
        }

        private void edit_score_value_id(object sender, RoutedEventArgs e)
        {
            leaderBoardService.EditScoreValueById(Constant.scoreId, 2000, this);
        }

        private void get_user_ranking(object sender, RoutedEventArgs e)
        {
            leaderBoardService.GetUserRanking(gameName, Constant.userName,this);
        }

        private void get_top_n_rankers(object sender, RoutedEventArgs e)
        {
            leaderBoardService.GetTopNRankers(gameName, 10, this);
        }

        private void get_top_n_rankers_with_sorting(object sender, RoutedEventArgs e)
        {
            Dictionary<String, String> otherMetaHeaders = new Dictionary<String, String>();
            otherMetaHeaders.Add("orderByAscending", "score");// Use orderByDescending for Descending or orderByAscending for Ascending
            leaderBoardService.SetOtherMetaHeaders(otherMetaHeaders);
            leaderBoardService.GetTopNRankers(gameName, 10, this);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
    }
}
