using MediatR;
using Owleye.Model.Model;
using System;
using System.Collections.Generic;

namespace Owleye.Service.Notifications.Messages
{
    public class NotifyViaEmailMessage : INotification
    {
        public string ServiceUrl { get; set; }
        public string IpAddress { get; set; }
        public SensorType SensorType { get; set; }
        public List<string> EmailAddresses { get; set; }
        public bool IsServiceAlive { get; set; }
        public DateTime LastAvailable { get; set; }
    }
}
