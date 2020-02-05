using BeeGees;
using BeeGees_Messaging.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeeGees_WriteNode.Facade
{
    public enum HandlerType
    {
        CreateShipment,
        UpdateShimpent
    }

    public interface IWriterFacade
    {
        IMessageHandler CreateHandler(HandlerType type);

        BaseMessage GenerateEvent(object response);
    }
}
