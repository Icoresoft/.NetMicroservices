using Core.Data;
using Core.Data.Attributes;
using Core.Options;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Reflection;

namespace Core.Extensions
{
    public static class Extensions
    {
        public static void MigratePGDatabase<TContext>(this WebApplication app, int? retries = 0)
        {

            int retriesCount = retries.Value;
            //Request services from the asp.net core DI 
            using (var scope = app.Services.CreateScope())
            { 
                var ServiceProvider = scope.ServiceProvider;
                var logger=ServiceProvider.GetRequiredService <ILogger<TContext>>();
                try
                {
                    logger.Log(LogLevel.Information, "== Migration Started ==");
                    
                    var con = ServiceProvider.GetRequiredService<NpgsqlConnection>();

                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand
                    {
                        Connection = con,
                    };

                    cmd.CommandText = " drop table if exists coupon2 ";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "SELECT EXISTS(SELECT FROM pg_tables WHERE schemaname = 'public' AND tablename = 'coupon2');";
                    var exists=Convert.ToBoolean( cmd.ExecuteScalar());
                    if(!exists)
                    {
                        cmd.CommandText = "create table coupon2(id serial primary key,ProductCode text not null) ";
                        cmd.ExecuteNonQuery();
                        //seeding data
                        cmd.CommandText = "insert into coupon2 (id,ProductCode) values (100,'p100')";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "insert into coupon2 (id,ProductCode) values (200,'p200')";
                        cmd.ExecuteNonQuery();

                    }
                    con.Close();
                    logger.Log(LogLevel.Information, "== Migration Completed ==");
                }
                catch (NpgsqlException ex)
                {
                    logger.Log(LogLevel.Error, ex.Message);
                    if(retriesCount > 0)
                    {
                        retriesCount--;
                        MigratePGDatabase<TContext>(app, retriesCount);
                    }
                }
            }
        }
        public static IServiceCollection AddNpgsqlConnection(this IServiceCollection services)
        {
            //add scoped object
            services.AddScoped(ServiceProvider =>
            {
                var dbSettings = ServiceProvider.GetRequiredService<IDbSettings>();
                NpgsqlConnection con = new(dbSettings.ConnectionString);
                return con;
            });

            return services;
        }
        public static Dictionary<string, string> AsPropertyValuePair(this Object obj)
        {
            Dictionary<string, string> Dict = new Dictionary<string, string>();
            Type t = obj.GetType();
            Console.WriteLine("Type is: {0}", t.Name);
            PropertyInfo[] props = t.GetProperties();

            foreach (var prop in props)
            {
                bool IsQIgnore = Attribute.IsDefined(prop, typeof(QueryIgnoreAttribute));
                if (!IsQIgnore)
                    Dict.Add(prop.Name, Convert.ToString(prop.GetValue(obj)));
            }
            return Dict;
        }
        public static string AsQueryFields(this Dictionary<string, string> dict)
        {
            return string.Join(",", dict.Keys);
            //return dict.Select(e => e.Key).ToArray<string>().Join(",".ToCharArray());
        }
        public static string AsQueryValues(this Dictionary<string, string> dict)
        {
            return "'" + string.Join("','", dict.Values) + "'";

            //return dict.Select(e => e.Key).ToArray<string>().Join(",".ToCharArray());
        }
        public static string AsQueryKeyValuePair(this Dictionary<string, string> dict)
        {
            var data = dict.Select(i => i.Key + "='" + i.Value + "'");
            return string.Join(" , ", data);
            //return dict.Select(e => e.Key).ToArray<string>().Join(",".ToCharArray());
        }
    }
}
