using System;
using System.Text;
using EnumsNET;
using Owleye.Model.Model;
using Owleye.Service.Notifications.Messages;

namespace Owleye.Service.Notifications.Services
{
    public static class NotifyMessagePreparationService
    {
        public static string Prepare(NotifyViaEmailMessage message)
        {
            var stringBuilder = new StringBuilder();

            switch (message.IsServiceAlive)
            {
                case false:
                    {
                        stringBuilder.Append("Owleye Service ALERT!" + Environment.NewLine);

                        switch (message.SensorType)
                        {
                            case SensorType.Ping:
                                {
                                    stringBuilder.Append(
                                        $"{SensorType.Ping.AsString(EnumFormat.Description)} for IP {message.IpAddress} failed." + Environment.NewLine);
                                    break;
                                }

                            case SensorType.PageLoad:
                                {
                                    stringBuilder.Append(
                                        $"{SensorType.PageLoad.AsString(EnumFormat.Description)} for Url {message.ServiceUrl} failed." + Environment.NewLine);
                                    break;
                                }
                        }

                        break;
                    }

                case true:
                    {
                        stringBuilder.Append("Owleye Service Notification" + Environment.NewLine);

                        switch (message.SensorType)
                        {
                            case SensorType.Ping:
                                {
                                    stringBuilder.Append(
                                        $"{SensorType.Ping.AsString(EnumFormat.Description)} for IP {message.IpAddress} available." + Environment.NewLine);
                                    break;
                                }

                            case SensorType.PageLoad:
                                {
                                    stringBuilder.Append(
                                        $"{SensorType.PageLoad.AsString(EnumFormat.Description)} for Url {message.ServiceUrl} available." + Environment.NewLine);
                                    break;
                                }
                        }

                        break;
                    }
            }

            stringBuilder.Append("<br/> Owleye monitoring system");
            return stringBuilder.ToString();
        }
    }
}
