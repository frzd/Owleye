using Owleye.Model.Model;
using Quartz;


namespace Owleye.Service
{
    public  class QrtzSchedule : IQrtzSchedule
    {

        public  void schedule<T>(IScheduler scheduler, SensorInterval interval) where T : IJob
        {
            
            var job = JobBuilder.Create<T>()
                .WithIdentity($"{nameof(SensorInterval)}-{(int)interval}-Job", "Sensors")
                .Build();

            job.JobDataMap["Interval"] = interval;

            var trigger = TriggerBuilder.Create()
                .WithIdentity($"{nameof(SensorInterval)}{(int)interval}-Trigger", "Triggers")
                .WithSimpleSchedule(x => x.WithIntervalInSeconds((int)interval).RepeatForever())
                .Build();
            

            scheduler.ScheduleJob(job, trigger);
        }
    }
}
