using MediatR;
using Owleye.Model.Model;

namespace Owleye.Service.Notifications.Messages
{
    public class NotifyViaEmailMessage : INotification
    {
        public string ServiceUrl { get; set; }
        public string IpAddress { get; set; }
        public SensorType SensorType { get; set; }
        public string EmailAddress { get; set; }
        public bool IsServiceAlive { get; set; }
    }
}
