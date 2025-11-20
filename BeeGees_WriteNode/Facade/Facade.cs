using BeeGees;
using BeeGees_Messaging.Handlers;
using BeeGees_WriteNode.Facade.Services.Handlers;
using BeeGees_WriteNode.Facade.Helpers;

namespace BeeGees_WriteNode.Facade
{
    public class Facade : IWriterFacade
    {
        public IMessageHandler CreateHandler(HandlerType type)
        {
            switch(type)
            {
                case HandlerType.CreateShipment:
                    return new CreateShipmentHandler();
                case HandlerType.UpdateShimpent:
                    return new UpdateShipmentHandler();
                default:
                    return null;
            }
        }

        public BaseMessage GenerateEvent(object response)
        {
            return EventsHelper.GenerateEventMessageFromResponse(response);
        }
    }
}
