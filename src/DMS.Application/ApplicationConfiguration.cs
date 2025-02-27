using System.Reflection;
using AutoMapper;
using DMS.Application.Behaviours;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DMS.Application;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        IEnumerable<Type> mapperTypes = AssemblyReference.Assembly
            .GetTypes()
            .Where(type =>
                typeof(IMapper).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

        services.AddAutoMapper(config =>
        {
            foreach (Type mapperType in mapperTypes)
            {
                MethodInfo createMapMethod = mapperType.GetMethod("CreateMap", BindingFlags.Public | BindingFlags.Static);
                createMapMethod?.Invoke(null, [config]);
            }
        });

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(AssemblyReference.Assembly);
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingPipelineBehaviour<,>));
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(RequestLoggingPipelineBehaviour<,>));
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehaviour<,>));
        });

        services
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssembly(AssemblyReference.Assembly);

        return services;
    }
}
