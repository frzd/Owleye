using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LiteX.Email.Core;
using MediatR;
using Owleye.Service.Notifications.Messages;

namespace Owleye.Service.Notifications.Services
{
    public class NotifyViaEmailHandler : INotificationHandler<NotifyViaEmailMessage>
    {
        private readonly ILiteXEmailSender _emailSender;

        public NotifyViaEmailHandler(ILiteXEmailSender emailSender)
        {
            _emailSender = emailSender;
        }
        public async Task Handle(NotifyViaEmailMessage notification, CancellationToken cancellationToken)
        {
            var message = NotifyMessagePreparationService.Prepare(notification);

            var mainEmailAddress = notification.EmailAddresses.First(); // TODO fix this, this is random pick email address.
            var bccAddresses = notification.EmailAddresses.Skip(1)?.ToList();

            await _emailSender.SendEmailAsync($"OwlEye notification", message,
            "server@pouyabadkoobeh.com", "Badkoobeh Web Server Monitoring",
            mainEmailAddress, "owl eye user", null, null, bccAddresses, null, null, cancellationToken: cancellationToken);

        }
    }
}
