using System;

namespace Owleye.Shared.Data
{
    [Serializable]
    public class Notification : BaseEntity
    {
        public int EndPointId { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }

        public EndPoint EndPoint { get; set; }
    }
}
