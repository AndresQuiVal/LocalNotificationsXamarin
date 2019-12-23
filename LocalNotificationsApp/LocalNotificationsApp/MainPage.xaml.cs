using LocalNotifications.Plugin;
using LocalNotifications.Plugin.Abstractions;
using LocalNotificationsApp.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LocalNotificationsApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        INotificationManager notificationManager;
        public MainPage()
        {
            InitializeComponent();
            notificationManager = DependencyService.Get<INotificationManager>();
            notificationManager.NotificationReceived += GetNotificationData;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

            notificationManager.ScheduleNotification("Local notification",
                "this notification is used with DependencyService!", 2, true,
                new long[] { 120, 180, 60, 420, 300, 120, 60});

            //CrossLocalNotifications.CreateLocalNotifier();
        }

        private void GetNotificationData(object sender, EventArgs e)
        {
            var evtData = (NotificationEventArgs) e;
            ShowNotification(evtData.Title, evtData.Message);
        }

        private void ShowNotification(object title, object message)
        {
            NotificationLabel.Text = $"You have touched the notification " +
                $"{title.ToString()}";
        }
    }
}
