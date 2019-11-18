namespace finance_management_backend.Infrastructure
{
    public class Settings
    {
        public string ConnectionString => DotNetEnv.Env.GetString("CONNECTION_STRING");
    }
}