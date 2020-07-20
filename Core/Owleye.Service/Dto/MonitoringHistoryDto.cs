using System;
using System.Collections.Generic;

namespace Owleye.Service.Dto
{
    [Serializable]
    public class MonitoringHistoryDto
    {
        public DateTime LastCheck { get; protected set; }
        public bool LastStatus { get; protected set; }

        public List<MonitoringTimeHistoryDto> TimeHistories { get; protected set; }

        public void AddCheckEvent(DateTime time, bool status)
        {
            if (TimeHistories == null)
            {
                TimeHistories = new List<MonitoringTimeHistoryDto>();
            }

            LastCheck = time;
            LastStatus = status;

            TimeHistories.Add(new MonitoringTimeHistoryDto().Set(time, status));
        }


    }
}
