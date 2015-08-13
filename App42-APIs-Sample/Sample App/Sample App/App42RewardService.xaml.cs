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
using com.shephertz.app42.paas.sdk.windows.reward;
using System.Collections;

// The Item Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234232

namespace Sample_App
{
    /// <summary>
    /// A page that displays details for a single item within a group.
    /// </summary>
    public sealed partial class App42RewardService : Page,App42Callback
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        public RewardService rewardService = null;
        public String rewardName = null;
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
        public void OnSuccess(Object response)
        {
            DISPATCHER.ExecuteAsync(delegate()
            {
                if (response is Reward)
                {
                    Reward reward = (Reward)response;
                    RewardResponseTBL.Text = "Reward  Response is : " + reward;
                }
                else
                {
                    IList<Reward> rewardObj = (IList<Reward>)response;
                    RewardResponseTBL.Text = "Reward  Response is : " + rewardObj;
                }
            });

        }
        public void OnException(App42Exception exception)
        {
            DISPATCHER.ExecuteAsync(delegate()
            {
                RewardResponseTBL.Text = "Exception is : " + exception;
            });
        }
        public App42RewardService()
        {
            rewardName = "Golden Reward" + DateTime.Now.Millisecond;
            rewardService = App42API.BuildRewardService();
            if (Constant.sessionId != null && Constant.sessionId != "")
            {
                rewardService.SetSessionId(Constant.sessionId);
            }
            if (Constant.adminKey != null && Constant.adminKey != "")
            {
                rewardService.SetAdminKey(Constant.adminKey);
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

        private void create_reward(object sender, RoutedEventArgs e)
        {
            rewardService.CreateReward(rewardName, "Reward Added SuccessFully", this);
        }

        private void earn_reward(object sender, RoutedEventArgs e)
        {
            rewardService.EarnRewards(Constant.gameName, Constant.userName, rewardName, 1000, this);
            rewardService.EarnRewards(Constant.gameName,"Nick", rewardName, 2000, this);
        }

        private void redeem_reward(object sender, RoutedEventArgs e)
        {
            rewardService.RedeemRewards(Constant.gameName, Constant.userName, rewardName, 400, this);
        }

        private void get_game_reward_point(object sender, RoutedEventArgs e)
        {
            rewardService.GetGameRewardPointsForUser(Constant.gameName, Constant.userName, this);
        }

        private void get_all_rewards(object sender, RoutedEventArgs e)
        {
            rewardService.GetAllRewards(this);
        }

        private void get_all_rewards_by_paging(object sender, RoutedEventArgs e)
        {
            rewardService.GetAllRewards(10,0,this);
        }
    }
}
