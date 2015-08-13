using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace App42_WNS_Sample
{
    class DISPATCHER
    {
        private static CoreDispatcher dispatcher;

        public static void Initialize()
        {
            dispatcher = Window.Current.Dispatcher;
        }

        public static async void ExecuteAsync(Action action)
        {
            if (dispatcher.HasThreadAccess)
                action();

            else await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
        }

        public static void Execute(Action action)
        {
            InnerExecute(action).Wait();
        }

        private static async Task InnerExecute(Action action)
        {
            if (dispatcher.HasThreadAccess)
                action();
            else await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
        }
    }
}
