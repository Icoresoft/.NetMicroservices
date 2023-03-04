using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Persistance
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext context,ILogger<OrderContextSeed> Logger)
        {
            if(!context.Set<Order>().Any())
            {
                context.Set<Order>().AddRange(GetPreconfiguredOrders());
                await context.SaveChangesAsync();
                Logger.LogInformation("data Seed successfully....");
            }
        }
        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>
            {
                new Order() {UserName = "coresoft", FirstName = "Mehmet", LastName = "Ozkaya", EmailAddress = "ezozkme@gmail.com", AddressLine = "Bahcelievler", Country = "Turkey", TotalPrice = 350 ,CVV="753" , CardName="Moustafa Kandil",CardNumber="123456765432",State="Riyadh",ZipCode="1334",PaymentMethod=1,Expiration=DateTime.Now.AddYears(3).ToShortDateString()}
            };
        }
    }
}
