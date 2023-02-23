using Core.Data;
using System.Data.Common;

namespace Core.Options
{
    public class NpgsqlConnectionOptions
    {
        public string  ConnectionString { get; set; }
        public DbSettings   DbSettings { get; set; }
    }
}
