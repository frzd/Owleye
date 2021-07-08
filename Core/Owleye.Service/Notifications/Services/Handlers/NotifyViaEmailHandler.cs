using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LiteX.Email.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Owleye.Service.Notifications.Messages;

namespace Owleye.Service.Notifications.Services
{
    public class NotifyViaEmailHandler : INotificationHandler<NotifyViaEmailMessage>
    {
        private readonly ILiteXEmailSender _emailSender;
        private readonly ILogger<NotifyViaEmailHandler> _logger;
        public NotifyViaEmailHandler(ILiteXEmailSender emailSender, ILogger<NotifyViaEmailHandler> logger)
        {
            _emailSender = emailSender;
            _logger = logger;
        }
        public async Task Handle(NotifyViaEmailMessage notification, CancellationToken cancellationToken)
        {
            var message = NotifyMessagePreparationService.Prepare(notification);

            _logger.LogInformation($"Endpoint {notification.ServiceUrl} availabily status is {notification.IsServiceAlive}");

            var mainEmailAddress = notification.EmailAddresses.First(); // TODO fix this, this is random pick email address.
            var bccAddresses = notification.EmailAddresses.Skip(1)?.ToList();
            if (bccAddresses.Any() == false)
                bccAddresses = null;

            var mailTitle = NotifyMessagePreparationService.PrepareMailTitle(notification.ServiceUrl, notification.IsServiceAlive);

            await _emailSender.SendEmailAsync(mailTitle, message,
            "server@badkoobehschool.com", "Badkoobeh Web Server Monitoring",
            mainEmailAddress, "owl eye user", bcc: bccAddresses, cancellationToken: cancellationToken);

        }
    }
}
