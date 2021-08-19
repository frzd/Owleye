using System.Collections.Generic;
using System.Threading.Tasks;
using Owleye.Core.Aggrigate;

namespace Owleye.Core.Services
{
    public interface ISensorService
    {
        Task<IEnumerable<Sensor>> GetSensors(SensorInterval interval, SensorType sensorType);
        Task<IEnumerable<Sensor>> GetSensorsByInterval(SensorInterval interval);
    }
}
