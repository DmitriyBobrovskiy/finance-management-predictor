namespace finance_management_backend.Infrastructure
{
    public class Settings
    {
        public string ConnectionString => $"Host={Host};Port={Port};Username={Username};Password={Password};Database={Database};";
        public string Host => DotNetEnv.Env.GetString("HOST");
        public string Port => DotNetEnv.Env.GetString("PORT");
        public string Username => DotNetEnv.Env.GetString("USERNAME");
        public string Password => DotNetEnv.Env.GetString("PASSWORD");
        public string Database => DotNetEnv.Env.GetString("DATABASE");
    }
}