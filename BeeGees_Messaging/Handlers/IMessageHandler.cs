using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeeGees_Messaging.Handlers
{
    public interface IMessageHandler
    {
        IMessage HandleMessage(object message);
    }
}
