using com.shephertz.app42.paas.sdk.windows;
using com.shephertz.app42.paas.sdk.windows.push;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Networking.Connectivity;
using Windows.Networking.PushNotifications;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace App42_WNS_Sample
{
    public class AppHelper
    {
        // Initializing PushNotification Service.
        PushNotificationService pushNotificationService = null;

        public AppHelper() {
            // Initializing App42.
            App42API.Initialize(AppConstants.apiKey, AppConstants.secretKey);
            // Initializing PushNotification Service.
            pushNotificationService = App42API.BuildPushNotificationService();
        }
        String _channelUri;

        /// <summary>
        /// Get or set the value of ChannelUri
        /// </summary>
        public string ChannelUri
        {
            get { return _channelUri; }
            set { _channelUri = value; }
        }

        PushNotificationChannel channel = null;

        /// <summary>
        /// Creating Channel Uri.
        /// </summary>
        public async void CreateOrUpdateChannelUri()
        {
            try
            {
                var vProfile = NetworkInformation.GetInternetConnectionProfile();
                if (vProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess)
                {
                    channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
                    ChannelUri = channel.Uri;
                    System.Diagnostics.Debug.WriteLine("Channel Uri :: " + ChannelUri);
                    CheckIfNewUri();
                    channel.PushNotificationReceived += OnPushNotification;
                }
            }

            catch (Exception ex)
            {
                // Could not create a channel. 
                // Error codes are explained on msdn https://msdn.microsoft.com/en-us/library/windows/apps/windows.networking.pushnotifications.pushnotificationchannelmanager.createpushnotificationchannelforapplicationasync.aspx
                System.Diagnostics.Debug.WriteLine("Error while creating channel :: " + ex.ToString());
                new WNSCallback().HideLoading();
                string errorMessage = "Error requesting channel uri : " + ex.ToString();
                new WNSCallback().ShowMessage(errorMessage, NotifyType.ErrorMessage);

            }
        }

        void CheckIfNewUri()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey(AppConstants.channelUriKey))
            {
                string savedUri = (string)localSettings.Values[AppConstants.channelUriKey];
                if (!savedUri.Equals(ChannelUri))
                {
                    // Sending Channel Uri to app42 cloud service.
                    SendUriToCloudService();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Channel URI is same. No need to send it to cloud service.");
                    new WNSCallback().HideLoading();
                }
            }
            else
            {
                // Sending Channel Uri to app42 cloud service.
                SendUriToCloudService();
            }
        }

        void SendUriToCloudService() {
            pushNotificationService.StoreDeviceToken(AppConstants.userName, ChannelUri, new WNSCallback());
        }

        public void SendSimpleToast()
        {
            pushNotificationService.SendPushToastMessageToUser(AppConstants.userName, AppConstants.toastTitle, AppConstants.toastContent, null, new WNSPushCallback());
        }

        public void SendToastWithParams()
        {
            Dictionary<string, object> toastParams = new Dictionary<string, object>();
            toastParams.Add("param1", "Value1");
            toastParams.Add("param2", true);
            toastParams.Add("param3", 1000);
            pushNotificationService.SendPushToastMessageToUser(AppConstants.userName, AppConstants.toastTitle, AppConstants.toastContent, toastParams, new WNSPushCallback());
        }

        public void SendSimpleTile()
        {
            Tile tile = new Tile();
            tile.Title = AppConstants.tileTitle;
            tile.Content = AppConstants.tileContent;
            tile.BadgeCount = AppConstants.badgeCount;
            pushNotificationService.SendPushTileMessageToUser(AppConstants.userName, tile, new WNSPushCallback());
        }

        public void SendTileWithImage()
        {
            Tile tile = new Tile();
            tile.Title = AppConstants.tileTitle;
            tile.Content = AppConstants.tileContent;
            tile.BadgeCount = AppConstants.badgeCount;
            tile.BackgroundImage = AppConstants.tileBGImage;
            pushNotificationService.SendPushTileMessageToUser(AppConstants.userName, tile, new WNSPushCallback());
        }


        /// <summary>
        /// Reciever for WNS notifiations.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPushNotification(PushNotificationChannel sender, PushNotificationReceivedEventArgs e)
        {
            String notificationContent = String.Empty;
            System.Diagnostics.Debug.WriteLine(":::OnPushNotification:::");
            switch (e.NotificationType)
            {
                case PushNotificationType.Badge:
                    notificationContent = e.BadgeNotification.Content.GetXml();
                    System.Diagnostics.Debug.WriteLine("Badge recieved :: " + notificationContent);
                    BadgeNotification badge = new BadgeNotification(e.BadgeNotification.Content);
                    BadgeUpdateManager.CreateBadgeUpdaterForApplication().Update(badge);
                    break;

                case PushNotificationType.Tile:
                    notificationContent = e.TileNotification.Content.GetXml();
                    System.Diagnostics.Debug.WriteLine("Tile recieved :: " + notificationContent);
                    TileNotification tileNotification = new TileNotification(e.TileNotification.Content);
                    TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
                    break;

                case PushNotificationType.Toast:
                    notificationContent = e.ToastNotification.Content.GetXml();
                    System.Diagnostics.Debug.WriteLine("Toast recieved :: " + notificationContent);
                    ToastNotification toast = new ToastNotification(e.ToastNotification.Content);
                    ToastNotificationManager.CreateToastNotifier().Show(toast);
                    break;
                case PushNotificationType.Raw:
                    notificationContent = e.RawNotification.Content;
                    System.Diagnostics.Debug.WriteLine("Raw recieved :: " + notificationContent);
                    break;

                default:
                    System.Diagnostics.Debug.WriteLine("Unknown notification type recieved :: " + e.NotificationType);
                    break;

            }
            e.Cancel = true;
        }
    }
    public enum NotifyType
    {
        StatusMessage,
        ErrorMessage
    };
}
