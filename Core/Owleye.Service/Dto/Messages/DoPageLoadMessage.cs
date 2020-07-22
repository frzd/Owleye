using MediatR;

namespace Owleye.Service.Dto.Messages
{
    public class DoPageLoadMessage : INotification
    {
        public int EndPointId { get; set; }
        public string PageUrl { get; set; }
        public string EmailNotify { get; set; }
        public string MobileNotify { get; set; }
    }
}
