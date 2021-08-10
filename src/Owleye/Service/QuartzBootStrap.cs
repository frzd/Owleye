using Owleye.Model.Model;
using Quartz.Impl;

namespace Owleye.Service
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