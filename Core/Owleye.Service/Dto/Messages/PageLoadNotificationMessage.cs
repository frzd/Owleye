using MediatR;

namespace Owleye.Service.Notifications.Messages
{
    public class PageLoadNotificationMessage : INotification
    {
        public int EndPointId { get; set; }
        public string PageUrl { get; set; }
        public string EmailNotify { get; set; }
        public string MobileNotify { get; set; }
        public bool LoadSuccess { get; set; }
    }
}
