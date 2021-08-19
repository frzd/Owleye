using System.Threading.Tasks;
using MediatR;
using Owleye.Core.Dto.Messages;
using Quartz;
using Owleye.Core.Aggrigate;
using Owleye.Infrastructure.Service;
using Owleye.Core.Services;

namespace Owleye.Core
{
    [DisallowConcurrentExecution]
    public class QuartzJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var mediator = ServiceLocator.Resolve<IMediator>();
            var service = ServiceLocator.Resolve<ISensorService>();

            JobDataMap dataMap = context.JobDetail.JobDataMap;
            SensorInterval interval = (SensorInterval)dataMap["Interval"];

            var sensors = await service.GetSensorsByInterval(interval);
            await mediator.Publish(new EndPointCheckMessage
            {
                EndPointList = sensors
            });
        }
    }

}