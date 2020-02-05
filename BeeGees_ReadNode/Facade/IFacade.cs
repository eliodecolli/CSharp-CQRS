using BeeGees_Messaging;
using BeeGees_Messaging.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeeGees_ReadNode.Facade
{
    public interface IFacade
    {
        IMessageHandler GenerateHandler(MessageType type);

        void InitializeFastAccess(int threshold);
    }
}
