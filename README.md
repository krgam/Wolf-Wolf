# Wolf-Wolf
TicketSails technical task for Wolf&amp;Wolf

I tried to use the already given solution structure, so there are three applications that run as independent processes: TicketSales.Core.Processor (console app for command/event handlers), TicketSales.Admin (web api with swagger UI) & TicketSales.User (web api with swagger UI).

Because different processes are involved, I couldn't use the InMemory EF database so I decided to go with a centralized Sqlite database that will be created upon first run at C:\TickectSalesDB (path is in appsettings.josn). For the same reason, I couldn't use InMemory MassTransit message bus but I had to configure reale RabbitMQ. I didn't succeed to make it work on my local machine - had a problem with adding vhost and new user permissions (it's also in appsettings.josn) via RabbitMQ CLI and management web portal.

It can be easily switched/refactored to be everything in one process so InMemory components (DB & MessageBroker) can be used for testing - hope we can discuss this in a technical interview.

UnitTests are done for some representative classes, the same way should be done for all others.
