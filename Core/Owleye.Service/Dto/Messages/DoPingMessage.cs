using MediatR;

namespace Owleye.Service.Dto.Messages
{
   

    public class DoPingMessage : INotification
    {
        public int EndPointId { get; set; }
        public string IpAddress { get; set; }
        public string EmailNotify { get; set; }
        public string MobileNotify { get; set; }
    }
}
