using MediatR;

namespace Owleye.Service.Notifications.Messages
{
    public class PingNotificationMessage: INotification
    {
        public int EndPointId { get; set; }
        public string IpAddress { get; set; }
        public string EmailNotify { get; set; }
        public string MobileNotify { get; set; }
        public bool  PingSuccess { get; set; }


    }
}
