using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Owleye.Common.Cache;
using Owleye.Common.Util;
using Owleye.Model.Model;
using Owleye.Service.Dto;
using Owleye.Service.Dto.Messages;
using Owleye.Service.Notifications.Messages;

namespace Owleye.Service.Notifications.Services
{
    public class DoPingServiceHandler : INotificationHandler<DoPingMessage>
    {
        private readonly IMediator _mediator;
        private readonly IRedisCache _cache;

        public DoPingServiceHandler(
            IMediator mediator,
            IRedisCache cache)
        {
            _mediator = mediator;
            _cache = cache;
        }
        public async Task Handle(DoPingMessage notification, CancellationToken cancellationToken)
        {
            var cacheKey = $"{notification.EndPointId}-{nameof(SensorType.Ping)}";
            var operation = await _cache.GetAsync<OngoingOperationDto>(cacheKey);

            if (operation != null)
            {
                if ((DateTime.Now - operation.StartDate).TotalMinutes <= 1)
                    return;
            }
            else
            {
                await _cache.SetAsync(cacheKey, new OngoingOperationDto(DateTime.Now));
            }

            var networkavailability = true;

            var pingResult = PingUtil.Ping(notification.IpAddress);

            if (pingResult == false) // IS Network availability
                networkavailability = PingUtil.Ping("8.8.8.8");

            if (networkavailability == false)
            {
                //TODO Notify about  connection
            }
            else
            {
                await _mediator.Publish(new PingNotificationMessage
                {
                    IpAddress = notification.IpAddress,
                    EmailNotify = notification.EmailNotify,
                    EndPointId = notification.EndPointId,
                    MobileNotify = notification.MobileNotify,
                    PingSuccess = pingResult
                }, cancellationToken);

            }



        }
    }
}
