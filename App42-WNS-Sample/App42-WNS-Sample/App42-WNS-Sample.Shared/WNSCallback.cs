using com.shephertz.app42.paas.sdk.windows;
using com.shephertz.app42.paas.sdk.windows.push;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace App42_WNS_Sample
{
    class WNSCallback : App42Callback
    {

        public void OnException(App42Exception exception)
        {
            System.Diagnostics.Debug.WriteLine(":::EXCEPTION ::: " + exception.ToString());
            HideLoading();
            ShowMessage("Exception while storing Device Token to App42 cloud.", NotifyType.ErrorMessage);
        }

        public void OnSuccess(object response)
        {
            System.Diagnostics.Debug.WriteLine(":::SUCCESS ::: " + response.ToString());
            PushNotification pushObj = (PushNotification)response;
            String accessToken = pushObj.GetDeviceToken();
            System.Diagnostics.Debug.WriteLine(":::DeviceToken::: " + accessToken);
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey(AppConstants.channelUriKey))
            {
              localSettings.Values.Remove(AppConstants.channelUriKey);
            }
            localSettings.Values[AppConstants.channelUriKey] = accessToken;
            HideLoading();
            ShowMessage("Device Token stored to App42 cloud.",NotifyType.StatusMessage);
        }

        public void HideLoading()
        {
            DISPATCHER.ExecuteAsync(delegate()
            {
                var frame = (Frame)Window.Current.Content;
                var container = frame.Content as MainPage;
                container.HideLoadingBar();
            });
        }

        public void ShowMessage(String message, NotifyType type)
        {
            DISPATCHER.ExecuteAsync(delegate()
            {
                var frame = (Frame)Window.Current.Content;
                var container = frame.Content as MainPage;
                container.NotifyUser(message, type);
            });
        }
    }

    class WNSPushCallback : App42Callback
    {
        public void OnException(App42Exception exception)
        {
            System.Diagnostics.Debug.WriteLine(":::EXCEPTION ::: " + exception.ToString());
            new WNSCallback().HideLoading();
            new WNSCallback().ShowMessage("EXCEPTION SENDING PUSH.", NotifyType.ErrorMessage);
        }

        public void OnSuccess(object response)
        {
            System.Diagnostics.Debug.WriteLine(":::SUCCESS ::: " + response.ToString());
            new WNSCallback().HideLoading();
            new WNSCallback().ShowMessage("PUSH SUCCESSFULLY SENT.", NotifyType.StatusMessage);
        }
    }
}
