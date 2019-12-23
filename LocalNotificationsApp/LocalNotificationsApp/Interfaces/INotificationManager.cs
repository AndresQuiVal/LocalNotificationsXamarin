using System;
using System.Collections.Generic;
using System.Text;

namespace LocalNotificationsApp.Interfaces
{
    public interface INotificationManager
    {
        event EventHandler NotificationReceived;
        void Initialize();
        int ScheduleNotification(
            string title,
            string message,
            long startSeconds,
            bool isRepeated = false,
            /*long[] intervalSeconds = null*/
            long intervalSeconds = 0);
        void ReceiveNotification(string title, string message);
    }
}
