using System.Net.NetworkInformation;

namespace Owleye.Common.Util
{
    public static class PingUtil
    {
        public static bool Ping(string ip)
        {
            var pingSuccess = false;
            var ping = new Ping();

            try
            {
                var reply = ping.Send(ip);
                if (reply != null) pingSuccess = reply.Status == IPStatus.Success;
            }
            catch (PingException) { }
            finally
            {
                ping.Dispose();
            }

            return pingSuccess;
        }
    }
}
