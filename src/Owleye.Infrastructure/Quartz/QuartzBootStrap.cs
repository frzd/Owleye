using Owleye.Core.Aggrigate;
using Owleye.Infrastructure.Quartz;
using Owleye.Infrastructure.Service;
using Quartz.Impl;

namespace Owleye.Core
{
    public partial class QuartzBootStrap
    {
        public void Boot()
        {

            var scheduler = StdSchedulerFactory.GetDefaultScheduler().Result; //TODO fix this.
            scheduler.Start();

            var schedulerService = ServiceLocator.Resolve<IQrtzSchedule>();

            schedulerService.schedule<QuartzJob>(scheduler, SensorInterval.OneMinute);
            schedulerService.schedule<QuartzJob>(scheduler, SensorInterval.ThirtySecond);
            schedulerService.schedule<QuartzJob>(scheduler, SensorInterval.FifteenMinutes);
            schedulerService.schedule<QuartzJob>(scheduler, SensorInterval.FiveMinute);

        }
    }

}