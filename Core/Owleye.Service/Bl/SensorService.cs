using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Owleye.Model.Model;
using Owleye.Model.Repository;

namespace Owleye.Service.Bl
{
    public class SensorService : ISensorService
    {
        private readonly IGenericRepository<Sensor> _sensorRepository;

        public SensorService(IGenericRepository<Sensor> sensorRepository)
        {
            _sensorRepository = sensorRepository;
        }
        public async Task<IEnumerable<Sensor>> GetSensors(SensorInterval interval, SensorType sensorType)
        {
            var endPointList = (await GetSensors(interval)).Where(q => q.SensorType == sensorType);
            return endPointList;
        }

        public async Task<IEnumerable<Sensor>> GetSensors(SensorInterval interval)
        {
            var includeProperties = new Expression<Func<Sensor, dynamic>>[2];
            includeProperties[0] = i => i.EndPoint;
            includeProperties[1] = i => i.EndPoint.Notification;

            var endPointList = await _sensorRepository
                .GetAsync(filter: q => q.SensorInterval == interval,
                    includeProperties: includeProperties);

            return endPointList;
        }
    }
}
