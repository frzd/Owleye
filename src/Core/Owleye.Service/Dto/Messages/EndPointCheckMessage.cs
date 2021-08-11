using MediatR;
using Owleye.Core.Aggrigate;
using System.Collections.Generic;

namespace Owleye.Core.Dto.Messages
{
    public class EndPointCheckMessage : INotification
    {
        public IEnumerable<Sensor> EndPointList { get; set; }
    }
}
