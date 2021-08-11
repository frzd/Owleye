using Owleye.Core.Aggrigate;
using Quartz;

namespace Owleye.Infrastructure.Quartz
{
    public interface IQrtzSchedule
    {
        void schedule<T>(IScheduler scheduler, SensorInterval interval) where T : IJob;
    }
}
