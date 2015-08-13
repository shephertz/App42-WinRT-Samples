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
using com.shephertz.app42.paas.sdk.windows.storage;
using Newtonsoft.Json.Linq;

// The Item Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234232

namespace Sample_App
{
    /// <summary>
    /// A page that displays details for a single item within a group.
    /// </summary>
    public sealed partial class App42StorageService : Page, App42Callback
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        public StorageService storageService = null;
        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }
        public void OnSuccess(Object response)
        {

            DISPATCHER.ExecuteAsync(delegate()
            {
                if (response is Storage)
                {
                    Storage storage = (Storage)response;
                    StorageResponseTBL.Text = "Storage Response is : " + storage;
                    if (storage.GetJsonDocList()[0].GetDocId() != null)
                    {
                        Constant.docId = storage.GetJsonDocList()[0].GetDocId();
                    }
                }
            });

        }
        public void OnException(App42Exception exception)
        {
            DISPATCHER.ExecuteAsync(delegate()
            {
                StorageResponseTBL.Text = "Exception is : " + exception;
            });
        }
        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public App42StorageService()
        {
            storageService = App42API.BuildStorageService();
            
            if (Constant.sessionId != null && Constant.sessionId != "")
            {
                storageService.SetSessionId(Constant.sessionId);
            }
            if (Constant.adminKey != null && Constant.adminKey != "")
            {
                storageService.SetAdminKey(Constant.adminKey);
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

        private void insert_document(object sender, RoutedEventArgs e)
        {
            JObject jsonObject = new JObject();
            jsonObject.Add("Name", "Nick");
            jsonObject.Add("Address", "Gurgaon,India");
            jsonObject.Add("Age", 24);
            storageService.InsertJSONDocument(Constant.dbName, Constant.collectionName, jsonObject, this);
        }

        private void find_all_document(object sender, RoutedEventArgs e)
        {
            storageService.FindAllDocuments(Constant.dbName, Constant.collectionName, this);
        }

        private void find_all_document_paging(object sender, RoutedEventArgs e)
        {
            storageService.FindAllDocuments(Constant.dbName, Constant.collectionName, 10, 0, this);
        }

        private void find_document_by_id(object sender, RoutedEventArgs e)
        {
            storageService.FindDocumentById(Constant.dbName, Constant.collectionName, Constant.docId, this);
        }

        private void find_document_by_Key_value(object sender, RoutedEventArgs e)
        {
            storageService.FindDocumentByKeyValue(Constant.dbName, Constant.collectionName, "Name", "Nick", this);
        }

        private void update_document_by_id(object sender, RoutedEventArgs e)
        {
            JObject jsonObject = new JObject();
            jsonObject.Add("Name", "Himanshu");
            jsonObject.Add("Age", 24);
            jsonObject.Add("Address", "Delhi,India");
            storageService.UpdateDocumentByDocId(Constant.dbName, Constant.collectionName, Constant.docId, jsonObject, this);
        }

        private void update_document_by_Key_value(object sender, RoutedEventArgs e)
        {
            JObject jsonObject = new JObject();
            jsonObject.Add("Name", "Sharma");
            jsonObject.Add("Age", 24);
            jsonObject.Add("Address", "Delhi,India");
            storageService.UpdateDocumentByKeyValue(Constant.dbName, Constant.collectionName, "Address", "Delhi,India", jsonObject, this);
        }

        private void delete_document_by_id(object sender, RoutedEventArgs e)
        {
            storageService.DeleteDocumentById(Constant.dbName, Constant.collectionName, Constant.docId, this);
        }

        private void delete_document_by_Key_value(object sender, RoutedEventArgs e)
        {
            storageService.FindDocumentByKeyValue(Constant.dbName, Constant.collectionName, "Name", "Sharma", this);
        }

        private void find_document_by_query(object sender, RoutedEventArgs e)
        {
            Query q1 = QueryBuilder.Build("Name", "", Operator.LIKE);
            Query q2 = QueryBuilder.Build("Age", 24, Operator.EQUALS);
            Query q3 = QueryBuilder.CompoundOperator(q1, Operator.OR, q2);
            storageService.FindDocumentByQuery(Constant.dbName, Constant.collectionName, q3, this);
        }

        private void add_or_update_keys(object sender, RoutedEventArgs e)
        {
            JObject jsonObject = new JObject();
            jsonObject.Add("Name", "Ajay");
            storageService.AddOrUpdateKeys(Constant.dbName, Constant.collectionName, Constant.docId, jsonObject, this);
        }
    }
}
