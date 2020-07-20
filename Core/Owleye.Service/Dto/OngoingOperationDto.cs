using System;

namespace Owleye.Service.Dto
{
    [Serializable]
    public class OngoingOperationDto
    {
        public OngoingOperationDto(DateTime startDate)
        {
            StartDate = startDate;
        }
        public DateTime StartDate { get; protected set; }
    }
}
