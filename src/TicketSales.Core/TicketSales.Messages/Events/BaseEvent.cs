using System;

namespace TicketSales.Messages.Events
{
    public abstract class BaseEvent
    {
        public DateTime CreatedTime { get; set; }

        public BaseEvent()
        {
            this.CreatedTime = DateTime.UtcNow;
        }
    }
}
