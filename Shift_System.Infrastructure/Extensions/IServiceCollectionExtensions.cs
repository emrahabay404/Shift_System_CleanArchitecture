using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shift_System.Application.Interfaces;
using Shift_System.Domain.Common;
using Shift_System.Domain.Common.Interfaces;
using Shift_System.Infrastructure.Services;

namespace Shift_System.Infrastructure.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddInfrastructureLayer(this IServiceCollection services)
        {
            services.AddServices();
        }

        private static void AddServices(this IServiceCollection services)
        {
            services
                .AddTransient<IMediator, Mediator>()
                .AddTransient<IDomainEventDispatcher, DomainEventDispatcher>()
                .AddTransient<IDateTimeService, DateTimeService>()
                .AddTransient<IEmailService, EmailService>()
           //
           .AddTransient<IAuthService, AuthService>()
           .AddScoped<IFileUploadService, FileUploadService>();
        }
        //builder.Services.AddScoped<IFileUploadService, FileUploadService>();
    }
}
