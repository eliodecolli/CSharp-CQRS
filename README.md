# CSharp-CQRS
 A basic CQRS implementation, with a similiar approach to that of microservices.
## Summary
The project is divided into three main components:

 1. A writer node, which accepts and executes commands.
 2. A reader node, which accepts and executes queries.
 3. A client, which acts as an initial interface to contact the other two components.

The writer and reader nodes are both implemented using a Facade, while the client is a simple console application.

RabbitMQ is used to exchange messages across the components above. To serialize the messages I chose to go for Protobuffers (as the initial idea was that each component would be written in a different language, protobufs made more sense to me).
I made my implementation of an in-memory cache, which can be navigated using a "special" form of query which I called Cached Query String (CQS). It's just a "map" of the path that the instance must follow across multiple dictionaries to access a specific request.

The initial commit contains a code which is far from being well documented, but my goal is to fix that and perhaps improve the actual implementation in general.

## Synchronization between Write-Read nodes

Each node has its own database, therefore to keep the data up-to-date, I tried to implement an event-oriented pattern, and let RabbitMQ act as persistence for published events.
The application flow goes as follows:

 1. The client publishes a command to the writer's queue.
 2. The message is then routed to the writer which makes the necessary updates to its database.
 3. Based on the command being sent (CQRS favors this kind of approach), the writer generates a specific event with the data needed by the reader to apply the same changes.
 4. The writer publishes the event in a queue for events only.
 5. The reader, being subscribed to the events queue, reads any new event and updates its database accordingly.
 6. The reader publishes a message to the writer telling it that their databases are now in-sync.
 7. The writer after confirmation from the reader signals the client that its command has been completed.

In step 5, we don't wait for any pending reads to complete, nor do we consider them at all, which is a flaw per se. Yet I wanted to keep this as simple as possible, and implementing a whole saga would be a little bit of an overkill. Perhaps in later updates, I might fix this. :)

## In-memory cache
If a client submits the same query multiple times to the reader then, a better approach would be to store the results of the said query in memory for faster access.

For instance, a client might submit numerous GetShipments queries to refresh or update the user interface. In that case, accessing the database for each call made by that client is quite inefficient, so instead, we store the result of that query in memory.
However, if a client submits a command changing the status of a specific shipment then once the reader receives the event, it checks whether we have an item in memory that might be affected by the update. In that case, to prevent dirty reads we remove said item from the cache.

The location of the item, or how to store it can be thought of as a map:
START/{SENDER}/{QUERY_TYPE}/{PARAM_NAME}={PARAM_VALUE}/.../..../END

Instead of a specific sender, we can use "*" to indicate that we don't care where this request is coming from. This is particularly useful in cases where a user wants to check their shipments from multiple sources, in which case (as shown in the code), we simply store the item with the following query details:
*QUERY_TYPE=GetShipments* & *[PARAM_NAME=CustomerId]=CustomerId*

Reflection is used to match additional details about an item, for instance, we would like to access every shipment with a price of $10, in which case we simply add a new parameter:
START/{SENDER}/GetShipments/CustomerId={ID}/Price=10/END
However, the parameter name MUST match the property name in the class of the query, for instance, if the *GetShipments* query contains items of type *Shipment* then *Shipment* must have a property named Price (and also CustomerId).

