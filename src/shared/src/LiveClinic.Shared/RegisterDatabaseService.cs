using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LiveClinic.Shared
{
    public static class RegisterInfrastructureService
    {
        public static IServiceCollection AddSqliteDatabase<T>(this IServiceCollection services,
            IConfiguration configuration) where T : DbContext
        {
            var connectionString = configuration.GetConnectionString("LiveConnection");
            var connection = new SqliteConnection(connectionString);
            connection.Open();
            services.AddDbContext<T>(x => x.UseSqlite(connection));
            return services;
        }
    }
}