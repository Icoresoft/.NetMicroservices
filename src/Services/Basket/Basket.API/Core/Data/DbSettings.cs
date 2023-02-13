namespace Core.Data
{
    public class DbSettings : IDbSettings
    {

        public string ServerName { get; set; }
        public int PortNo { get; set; }
        public string DbName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    
        public string ConnectionString => $"{ServerName}:{PortNo}";

    }
}
