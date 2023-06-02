using LiveClinic.Registry.ServicesRegistration;
using Microsoft.AspNetCore.Builder;

WebApplication.CreateBuilder(args)
    .RegisterServices()
    .Build()
    .SetupMiddleware()
    .Run();