using Owleye.Model.Model;
using Quartz;

namespace Owleye.Service
{
    public interface IQrtzSchedule
    {
        void schedule<T>(IScheduler scheduler, SensorInterval interval) where T : IJob;
    }
}
