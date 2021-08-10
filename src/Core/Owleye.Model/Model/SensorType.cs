using System.ComponentModel;

namespace Owleye.Model.Model
{
    public enum SensorType
    {
        [Description("Ping")]
        Ping = 0,
        DnsCheck = 1,
        [Description("PageLoad")]
        PageLoad = 2
    }
}