using System;

namespace Owleye.Model.Model
{
    [Serializable]
    public class Sensor: BaseEntity
    {
        public string Name { get; set; }
        public int EndPointId { get; set; }
        public SensorType SensorType { get; set; }
        public SensorInterval SensorInterval { get; set; }
        public EndPoint EndPoint { get; set; }
    }
}
