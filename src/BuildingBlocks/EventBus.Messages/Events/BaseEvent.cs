using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Events
{
    public class BaseEvent
    {
        public BaseEvent(Guid id, DateTime creationDate)
        {
            Id = id;
            CreationDate = creationDate;
        }
        public BaseEvent() { 
            Id= Guid.NewGuid();
            CreationDate= DateTime.Now;
        }
        public Guid Id {get; private set; }
        public DateTime CreationDate { get; private set; }
    }
}
