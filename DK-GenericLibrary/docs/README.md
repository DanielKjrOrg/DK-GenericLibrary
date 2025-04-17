Package primarily made for ASP.NET Applications, but can work with any project utilizing ServiceProviders.

!Important
The solutions UpdateItem(s) methods can dynamically register changes of 1 nested collection, anything past that will need either a custom extension or to simply pass it directly into the parameter. As the collection items are part of the DbSet it will be able to update correctly.

This package provides a fully generic repository to fulfill the common CRUD actions with Entity Framework, without writing the same 8 lines 15 times per repository.

The gist of the package is, that by registering the Interface and/or class implementation, as well as the IDbContextFactory with a given context, each and every method in this package will create and dispose a new context every time.

All the (Async at least) methods has examples of how to use it in the documentation/mouseover.

How to use:
ASP.Net core

Register services

Only register the one you need, newly added Service extension to quickly register the services.

![image](https://i.imgur.com/KQny8rI.png)

It's important that the Context is registered as a IDbContextFactory as this is used to handle instantiation and disposal of context instances.

Inject repository into a service or wherever needed

![image](https://i.imgur.com/6lEsTNo.png)


Define methods and use the generic methods to retrieve what you need

Example:

![image](https://i.imgur.com/Cus7NSa.png)

Currently tested all methods and registrations of Async and regular repositories.
![image](https://i.imgur.com/nM2oDTp.png)
