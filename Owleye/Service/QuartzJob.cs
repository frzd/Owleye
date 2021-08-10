using System.Threading.Tasks;
using MediatR;
using Owleye.Model.Model;
using Owleye.Service.Bl;
using Owleye.Service.Dto.Messages;
using Quartz;

namespace Owleye.Service
{
    public partial class QuartzBootStrap
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

                var sensors = await service.GetSensors(interval);
                await mediator.Publish(new EndPointCheckMessage
                {
                    EndPointList = sensors
                });
            }


        }

    }

}