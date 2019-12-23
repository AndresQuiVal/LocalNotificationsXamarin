using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using LocalNotificationsApp.Droid.Broadcast;
using LocalNotificationsApp.Interfaces;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using AndroidApp = Android.App.Application;

[assembly: Dependency(typeof(LocalNotificationsApp.Droid.AndroidNotificationManager))]
namespace LocalNotificationsApp.Droid
{
    public class AndroidNotificationManager : INotificationManager
    {
        const string channelId = "default";
        const string channelName = "Default";
        const string channelDescription = "The default channel for notifications.";
        const int pendingIntentId = 0;

        public const string TitleKey = "title";
        public const string MessageKey = "message";

        bool channelInitialized = false;
        int messageId = -1;
        NotificationManager manager;
        public static AlarmManager alarmManager;

        public event EventHandler NotificationReceived;

        public void Initialize()
        {
            CreateNotificationChannel();
        }

        public int ScheduleNotification(
            string title, 
            string message, 
            long startSeconds,
            bool isRepeated = false,
            /*long[] intervalSeconds = null*/
            long intervalSeconds = 0)
        {
            if (!channelInitialized)
                CreateNotificationChannel();

            NotificationReciever.title = title;
            NotificationReciever.message = message;
            AlarmManager alarmManager = (AlarmManager)AndroidApp.Context.GetSystemService(Context.AlarmService);
            Intent intent = new Intent(AndroidApp.Context, typeof(NotificationReciever)/*typeof(MainActivity)*/);
            intent.PutExtra(TitleKey, title);
            intent.PutExtra(MessageKey, message);
            intent.AddFlags(ActivityFlags.ClearTop);

            PendingIntent pendingIntent = PendingIntent.GetBroadcast(AndroidApp.Context, 0, intent, PendingIntentFlags.UpdateCurrent); ;

            //NotificationReciever.startSeconds = startSeconds;
            //NotificationReciever.isRepeated = isRepeated;
            //NotificationReciever.intervalSeconds = intervalSeconds;

            alarmManager.Set(AlarmType.ElapsedRealtimeWakeup, SystemClock.ElapsedRealtime() + (startSeconds * 1000), /*2000, */pendingIntent);

            return messageId;
        }

        public void ReceiveNotification(string title, string message)
        {
            var args = new NotificationEventArgs()
            {
                Title = title,
                Message = message,
            };
            NotificationReceived?.Invoke(null, args);
        }

        void CreateNotificationChannel()
        {
            manager = (NotificationManager)AndroidApp.Context.GetSystemService(AndroidApp.NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelNameJava = new Java.Lang.String(channelName);
                var channel = new NotificationChannel(channelId, channelNameJava, NotificationImportance.Default)
                {
                    Description = channelDescription
                };
                manager.CreateNotificationChannel(channel);
            }

            channelInitialized = true;
        }
    }
}