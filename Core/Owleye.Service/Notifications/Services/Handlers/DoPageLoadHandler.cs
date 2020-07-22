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
    public class DoPageLoadHandler : INotificationHandler<DoPageLoadMessage>
    {
        private readonly IMediator _mediator;
        private readonly IRedisCache _cache;

        public DoPageLoadHandler(
            IMediator mediator,
            IRedisCache cache)
        {
            _mediator = mediator;
            _cache = cache;
        }
        public async Task Handle(DoPageLoadMessage notification, CancellationToken cancellationToken)
        {
            var cacheKey = $"{notification.EndPointId}-{nameof(SensorType.PageLoad)}";
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

            var urlResult = WebSiteUtil.IsUrlAlive(notification.PageUrl);

            if (urlResult == false) // check network availability
                networkavailability = PingUtil.Ping("8.8.8.8");

            if (networkavailability == false)
            {
                //TODO Notify about  connection
            }
            else
            {
                await _mediator.Publish(new PageLoadNotificationMessage
                {
                    PageUrl = notification.PageUrl,
                    EmailNotify = notification.EmailNotify,
                    EndPointId = notification.EndPointId,
                    MobileNotify = notification.MobileNotify,
                    LoadSuccess = urlResult
                }, cancellationToken);

            }
        }
    }
}
