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
using Plugin.CurrentActivity;
using AndroidApp = Android.App.Application;

namespace LocalNotificationsApp.Droid.Broadcast
{
    [BroadcastReceiver(Enabled =true)]
    public class NotificationReciever : BroadcastReceiver
    {
        const string channelId = "default";
        public static string title = "";
        public static string message = "";
        const string channelName = "Default";
        const string channelDescription = "The default channel for notifications.";
        const int pendingIntentId = 0;

        public const string TitleKey = "title";
        public const string MessageKey = "message";

        public static long startSeconds;
        public static bool isRepeated;
        public static long[] intervalSeconds;
        private static int counter = ((int)DateTime.Now.DayOfWeek) - 1;

        int messageId = -1;
        NotificationManager manager;

        public override void OnReceive(Context context, Intent intent)
        {
            Intent _intent = new Intent(AndroidApp.Context, typeof(MainActivity));
            _intent.PutExtra(TitleKey, title);
            _intent.PutExtra(MessageKey, message);

            PendingIntent pendingIntent = PendingIntent.GetActivity(
                AndroidApp.Context,
                pendingIntentId,
                _intent,
                PendingIntentFlags.OneShot);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(AndroidApp.Context, channelId)
               .SetContentIntent(pendingIntent)
               .SetContentTitle(title)
               .SetContentText(message)
               .SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.OlympiaLogo))
               .SetSmallIcon(Resource.Drawable.OlympiaLogo)
               .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate);

            manager = (NotificationManager)context.GetSystemService(AndroidApp.NotificationService);
            
            var notification = builder.Build();
            manager.Notify(messageId, notification);

            //AndroidNotificationManager.alarmManager.Cancel(
            //    AndroidNotificationManager.pendingIntent);

            //if (isRepeated)
            //{
            //    if (counter >= intervalSeconds.Length)
            //        counter = ((int)DateTime.Now.DayOfWeek) - 1;

            //    AndroidNotificationManager.alarmManager.SetRepeating(
            //       AlarmType.ElapsedRealtimeWakeup,
            //       SystemClock.ElapsedRealtime() + (startSeconds * 1000),
            //       (intervalSeconds[counter++] * 1000),
            //       pendingIntent);
            //    return;
            //}
            if (counter >= intervalSeconds.Length)
                counter = ((int)DateTime.Now.DayOfWeek) - 1;

            var time = intervalSeconds[counter++];

            //AndroidNotificationManager.alarmManager.Set(
            //    AlarmType.ElapsedRealtimeWakeup,
            //    /*SystemClock.ElapsedRealtime() + (startSeconds * 1000)*/
            //    (time * 1000),
            //    pendingIntent);
            AndroidNotificationManager.EstablishNotification(time);
        }
    }
}