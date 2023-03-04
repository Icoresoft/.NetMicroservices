using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Behaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application
{
    public static class DI
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //Automapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            //Fluent Validation
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            //MediatR
            services.AddMediatR(Assembly.GetExecutingAssembly());

            //MediatR Pipeline Behaviours 
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            //add your custom DI 
            return services;
        }
    }
}
