using System.Threading;
using System.Threading.Tasks;
using Extension.Methods;
using MediatR;
using Owleye.Model.Model;
using Owleye.Service.Notifications.Messages;

namespace Owleye.Service.Notifications.Services
{
    class PingNotifyService : INotificationHandler<PingNotificationMessage>
    {
        private readonly IMediator _mediator;

        public PingNotifyService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public Task Handle(PingNotificationMessage notification, CancellationToken cancellationToken)
        {
            if (notification.EmailNotify.IsNotNullOrEmpty())
               
                    _mediator.Publish(new NotifyViaEmailMessage
                    {
                        SensorType = SensorType.Ping,
                        IpAddress = notification.IpAddress,
                        EmailAddress = notification.EmailNotify,
                        IsServiceAlive = notification.PingSuccess
                    });
                

            return Task.CompletedTask;
        }
    }

}


