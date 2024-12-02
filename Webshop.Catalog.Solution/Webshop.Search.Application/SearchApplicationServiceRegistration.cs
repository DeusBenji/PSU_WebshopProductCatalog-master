
using MediatR;
using Microsoft.Extensions.DependencyInjection;

using System.Reflection;
using Webshop.Application.Contracts;
using Webshop.Application;
using Webshop.Search.Application.Contracts;

namespace Webshop.Search.Application
{
    /// <summary>
    /// Registrering af applikationstjenester for Search mikrotjenesten.
    /// </summary>
    public static class SearchApplicationServiceRegistration
    {
        public static IServiceCollection AddSearchApplicationServices(this IServiceCollection services)
        {
            // Registrer MediatR til at håndtere queries og kommandoer
            services.AddMediatR(Assembly.GetExecutingAssembly());

            // Registrer en Dispatcher til at håndtere Mediator-kald
            services.AddScoped<IDispatcher>(sp => new Dispatcher(sp.GetService<IMediator>()));

            return services;
        }
    }
}
