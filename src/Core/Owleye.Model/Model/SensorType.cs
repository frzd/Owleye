using System.ComponentModel;

namespace Owleye.Shared.Data
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