# Readme is a work in progress
# Now officially hosted publicly https://www.nuget.org/packages/DK.GenericLibrary/1.0.3
# How to use
### ASP.Net core
Register services

Only register the one you need

![image](https://github.com/DanielKjr/DK-NuGet-Library/assets/93762921/4e77a819-21c2-4d00-b14e-f2ef101d1b30)

It's important that the Context is registered as a IDbContextFactory as this is used to handle instantiation and disposal of context instances.

Inject repository into a service or wherever needed

![image](https://github.com/DanielKjr/DK-NuGet-Library/assets/93762921/ffd68d26-a8ca-468f-af30-4a51c559b231)

Define methods and use the generic methods to retrieve what you need

Example:

![image](https://github.com/DanielKjr/DK-NuGet-Library/assets/93762921/a1adddd3-fd64-415e-ac45-f6ec1327edfe)
