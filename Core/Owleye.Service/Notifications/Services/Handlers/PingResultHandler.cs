using System;
using System.Threading;
using System.Threading.Tasks;
using Extension.Methods;
using MediatR;
using Owleye.Common.Cache;
using Owleye.Model.Model;
using Owleye.Service.Dto;
using Owleye.Service.Notifications.Messages;

namespace Owleye.Service.Notifications.Services
{
    public class PingResultHandler : INotificationHandler<PingNotificationMessage>
    {
        private readonly IMediator _mediator;
        private readonly IRedisCache _cache;

        public PingResultHandler(IMediator mediator, IRedisCache cache)
        {
            _mediator = mediator;
            _cache = cache;
        }
        public async Task Handle(PingNotificationMessage notification, CancellationToken cancellationToken)
        {
            MonitoringHistoryDto history = null;

            //TODO  extension
            var cacheKey =
                $"{notification.EndPointId}-{nameof(SensorType.Ping)}-{DateTime.Now.ToString(@"yyyy-MM-dd")}";


            history = await _cache.GetAsync<MonitoringHistoryDto>(cacheKey) ?? new MonitoringHistoryDto();


            history.AddCheckEvent(DateTime.Now, notification.PingSuccess);
            await _cache.SetAsync(cacheKey, history);

            if (history.LastStatus != notification.PingSuccess)
            {
                await Notify(notification, cancellationToken);
            }

        }

        private async Task Notify(PingNotificationMessage notification, CancellationToken cancellationToken)
        {
            if (notification.EmailNotify.IsNotNullOrEmpty())
            {
                await _mediator.Publish(new NotifyViaEmailMessage
                {
                    IpAddress = notification.IpAddress,
                    SensorType = SensorType.Ping,
                    EmailAddress = notification.EmailNotify,
                    IsServiceAlive = notification.PingSuccess
                }, cancellationToken);
            }

            if (notification.MobileNotify.IsNotNullOrEmpty())
            {
                //todo notify via sms.
            }
        }
    }
}
