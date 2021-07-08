using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Owleye.Model.Model;
using Owleye.Service.Bl;
using Owleye.Service.Dto.Messages;
using Quartz;
using Quartz.Impl;
using System.Linq;

namespace Owleye.Service
{
    public class QuartzBootStrap
    {
        public void Boot()
        {

            var scheduler = StdSchedulerFactory.GetDefaultScheduler().Result; //TODO fix this.
            scheduler.Start();


            //-- triggers for 1 minute

            var jobOneMinute = JobBuilder.Create<OneMinuteSensorJob>()
                .WithIdentity($"{nameof(SensorInterval.OneMinute)}-Job", "Sensors")
                .Build();


            var triggerOneMinute = TriggerBuilder.Create()
                .WithIdentity($"{nameof(SensorInterval.OneMinute)}-Trigger", "Triggers")
                .WithSimpleSchedule(x => x.WithIntervalInSeconds((int)SensorInterval.OneMinute).RepeatForever())
                .Build();

            scheduler.ScheduleJob(jobOneMinute, triggerOneMinute);

            //-- triggers for 15 minute

            var jobFifteenMinute = JobBuilder.Create<FifteenMinuteSensorJob>()
                .WithIdentity($"{nameof(SensorInterval.FifteenMinutes)}-Job", "Sensors")
                .Build();


            var triggerFifteenMinute = TriggerBuilder.Create()
                .WithIdentity($"{nameof(SensorInterval.FifteenMinutes)}-Trigger", "Triggers")
                .WithSimpleSchedule(x => x.WithIntervalInSeconds((int)SensorInterval.FifteenMinutes).RepeatForever())
                .Build();

            scheduler.ScheduleJob(jobFifteenMinute, triggerFifteenMinute);


            //-- trigger for 30 secound
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

        [DisallowConcurrentExecution]
        public class FifteenMinuteSensorJob : IJob
        {
            public async Task Execute(IJobExecutionContext context)
            {

                var mediator = ServiceLocator.Resolve<IMediator>();
                var service = ServiceLocator.Resolve<ISensorService>();

                var sensors = await service.GetSensors(SensorInterval.FifteenMinutes);
                await Notify(sensors, mediator);

            }
        }

        private static async Task Notify(IEnumerable<Sensor> endPointList, IMediator mediator)
        {
            foreach (var sensor in endPointList)
            {
                var phoneList = sensor.EndPoint.Notification.Select(q => q.PhoneNumber).ToList();
                var emailList = sensor.EndPoint.Notification.Select(q => q.EmailAddress).ToList();

                switch (sensor.SensorType)
                {
                    case SensorType.Ping:
                        {
                            await mediator.Publish(
                                new DoPingMessage
                                {
                                    IpAddress = sensor.EndPoint.IpAddress,
                                    MobileNotify = phoneList,
                                    EndPointId = sensor.EndPointId,
                                    EmailNotify = emailList
                                }
                            );

                            break;
                        }

                    case SensorType.PageLoad:
                        {
                            await mediator.Publish(
                                new DoPageLoadMessage
                                {
                                    PageUrl = sensor.EndPoint.Url,
                                    MobileNotify = phoneList,
                                    EndPointId = sensor.EndPointId,
                                    EmailNotify = emailList,
                                }
                            );

                            break;
                        }
                }

            }
        }

    }

}