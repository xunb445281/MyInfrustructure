using Standard.Infrastructure.Events.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Infrastructure.Events.Defaults
{
    public class EventHandlerManager : IEventHandlerManager
    {
        public List<IEventHandler<TEvent>> GetHandlers<TEvent>() where TEvent : IEvent
        {
            throw new NotImplementedException();
        }
    }
}
