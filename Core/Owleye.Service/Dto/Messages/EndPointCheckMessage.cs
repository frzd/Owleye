using MediatR;
using Owleye.Model.Model;
using System.Collections.Generic;

namespace Owleye.Service.Dto.Messages
{
    public class EndPointCheckMessage : INotification
    {
        public IEnumerable<Sensor> EndPointList { get; set; }
    }
}
