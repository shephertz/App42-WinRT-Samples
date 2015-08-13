using Sample_App.Common;
using Sample_App.Data;
using System;
using System.Collections;
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
using Newtonsoft.Json.Linq;
using com.shephertz.app42.paas.sdk.windows.storage;
using System.Collections;

// The Item Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234232

namespace Sample_App
{
    /// <summary>
    /// A page that displays details for a single item within a group.
    /// </summary>
    public sealed partial class App42UserService : Page,App42Callback
    {
        public UserService userService = null;
        public string userName = null;
        public string password = null;
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        public void OnSuccess(Object response)
        {
            DISPATCHER.ExecuteAsync(delegate()
            {
                if (response is User)
                {
                    User user = (User)response;
                    if (user.GetSessionId() != null)
                    {
                        Constant.sessionId = user.GetSessionId();
                    }
                    UserResponseTBL.Text = "User Response is : " + user;
                }
                else
                {
                    UserResponseTBL.Text = "User Response is : " + response;
                }
            });

        }
        public void OnException(App42Exception exception)
        {
            DISPATCHER.ExecuteAsync(delegate()
            {
                UserResponseTBL.Text = "Exception is : " + exception;
            });
        }
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

        public App42UserService()
        {
            userService = App42API.BuildUserService();
            if (Constant.sessionId != null && Constant.sessionId != "")
            {
                userService.SetSessionId(Constant.sessionId);
            }
            if (Constant.adminKey != null && Constant.adminKey != "")
            {
                userService.SetAdminKey(Constant.adminKey);
            }
            userName = Constant.userName;
            password = "DemoUser";
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

        private void create_userClick(object sender, RoutedEventArgs e)
        {
            userService.CreateUser(userName, password, userName+ "@gmail.com", this);
        }

        private void authenticate_user(object sender, RoutedEventArgs e)
        {
            userService.Authenticate(userName, password, this);
        }

        private void get_userclick(object sender, RoutedEventArgs e)
        {
            userService.GetUser(userName, this);
        }

        private void get_user_by_email(object sender, RoutedEventArgs e)
        {
            userService.GetUserByEmailId(userName+"@gmail.com", this);
        }

        private void get_all_users(object sender, RoutedEventArgs e)
        {
            userService.GetAllUsers(this);
        }

        private void saving_addtionaldata(object sender, RoutedEventArgs e)
        {
            App42API.SetDbName(Constant.dbName);
            JObject jsonObj = new JObject();
            jsonObj.Add("FirstName", "Nick");
            jsonObj.Add("LastName", "Sharma");
            jsonObj.Add("Age", 24);
            userService.AddJSONObject(Constant.collectionName,jsonObj);
            userService.CreateUser(userName + "Saving", password, "Saving" + userName + "@gmail.com", this);
        }
        private void update_email(object sender, RoutedEventArgs e)
        {
            userService.UpdateUser(userName, userName + "@gmail.co.in", this);
        }

        private void reset_password(object sender, RoutedEventArgs e)
        {
            userService.ResetUserPassword(userName, password, this);
        }

        private void delete_user(object sender, RoutedEventArgs e)
        {
            Dictionary<String, String> otherMetaHeaders = new Dictionary<String, String>();
            otherMetaHeaders.Add("deletePermanent", "true");
            userService.SetOtherMetaHeaders(otherMetaHeaders);
            userService.DeleteUser(userName, this);
        }
    }
}
