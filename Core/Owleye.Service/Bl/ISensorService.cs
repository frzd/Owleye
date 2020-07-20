using System.Collections.Generic;
using System.Threading.Tasks;
using Owleye.Model.Model;

namespace Owleye.Service.Bl
{
    public interface ISensorService
    {
        Task<IEnumerable<Sensor>> GetSensors(SensorInterval interval);
    }
}
