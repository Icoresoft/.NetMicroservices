using Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistance;
using Ordering.Application.Models;
using Ordering.Infrastructure.Mail;
using Ordering.Infrastructure.Persistance;
using Ordering.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure
{
    public static class DI
    {
        public static IServiceCollection AddInfrstructureServices(this IServiceCollection services, IConfiguration configuration,string ConStr)
        {

            services.AddDbContext<OrderContext>(options =>
                options.UseSqlServer(ConStr)
            ) ;
            //todo: comment this line and check if code running or not
            //required for MedaitR ( for REVIEW )
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

            services.AddScoped<IOrderRepository,OrderRepository>();

            services.Configure<EmailSettings>(c=>configuration.GetSection(nameof(EmailSettings)));

            services.AddSingleton<IEmailService,EmailService>();

            return services;
        }
    }
}
