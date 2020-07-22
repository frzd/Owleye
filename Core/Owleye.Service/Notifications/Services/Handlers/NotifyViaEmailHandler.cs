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

            await _emailSender.SendEmailAsync($"Owleye notification", message,
                 "f0rz0d@outlook.com", "owleye",
                 "f0rz0d@outlook.com", "Farzad dh", cancellationToken: cancellationToken);
        }
    }
}
