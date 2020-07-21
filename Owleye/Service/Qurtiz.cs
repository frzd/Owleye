using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Owleye.Model.Model;
using Owleye.Service.Bl;
using Owleye.Service.Dto.Messages;
using Quartz;
using Quartz.Impl;

namespace Owleye.Service
{
    public class QuartzBootStrap
    {
        public void Boot()
        {

            var scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
            scheduler.Start();

            var jobOneMinute = JobBuilder.Create<OneMinuteSensorJob>()
                .WithIdentity($"{nameof(SensorInterval.OneMinute)}-Job", "Sensors")
                .Build();

            var triggerOneMinute = TriggerBuilder.Create()
                .WithIdentity($"{nameof(SensorInterval.OneMinute)}-Trigger", "Triggers")
                .WithSimpleSchedule(x => x.WithIntervalInSeconds((int)SensorInterval.OneMinute).RepeatForever())
                .Build();

            scheduler.ScheduleJob(jobOneMinute, triggerOneMinute);


            var jobThirtySecond = JobBuilder.Create<ThirtySecondSensorJob>()
                .WithIdentity($"{nameof(SensorInterval.ThirtySecond)}-Job", "Sensors")
                .Build();

            var triggerThirtySecond = TriggerBuilder.Create()
                .WithIdentity($"{nameof(SensorInterval.ThirtySecond)}-Trigger", "Triggers")
                .WithSimpleSchedule(x => x.WithIntervalInSeconds((int)SensorInterval.ThirtySecond).RepeatForever())
                .Build();

            scheduler.ScheduleJob(jobThirtySecond, triggerThirtySecond);
        }

        [DisallowConcurrentExecution]
        public class OneMinuteSensorJob : IJob
        {
            public async Task Execute(IJobExecutionContext context)
            {
                var mediator = ServiceLocator.Resolve<IMediator>();
                var service = ServiceLocator.Resolve<ISensorService>();

                var sensors = await service.GetSensors(SensorInterval.OneMinute);
                await Notify(sensors, mediator);
            }
        }

        [DisallowConcurrentExecution]
        public class ThirtySecondSensorJob : IJob
        {
            public async Task Execute(IJobExecutionContext context)
            {

                var mediator = ServiceLocator.Resolve<IMediator>();
                var service = ServiceLocator.Resolve<ISensorService>();

                var sensors = await service.GetSensors(SensorInterval.ThirtySecond);
                await Notify(sensors, mediator);

            }
        }


        private static async Task Notify(IEnumerable<Sensor> endPointList, IMediator mediator)
        {
            foreach (var sensor in endPointList)
            {
                switch (sensor.SensorType)
                {
                    case SensorType.Ping:
                        {
                            await mediator.Publish(
                                new DoPingMessage
                                {
                                    IpAddress = sensor.EndPoint.IpAddress,
                                    MobileNotify = sensor.EndPoint.Notification.PhoneNumber,
                                    EndPointId = sensor.EndPointId,
                                    EmailNotify = sensor.EndPoint.Notification.EmailAddress,
                                }
                            );

                            break;
                        }
                }

            }
        }

    }

}