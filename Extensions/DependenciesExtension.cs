using API_CONDOMINIO_V2.Repositories;
using API_CONDOMINIO_V2.Repositories.Contracts;

namespace API_CONDOMINIO_V2.Extensions
{
    public static class DependenciesExtension
    {
       // public static void AddSqlConnection(this IServiceCollection services,
       //string connectionString)
       // {
       //     services.AddScoped<SqlConnection>(x
       //      => new sql(connectionString));
       // }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBlockRepository, BlockRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IResidentRepository, ResidentRepository>();
            services.AddScoped<IUnitRepository, UnitRepository>();
        }


        public static void AddServices(this IServiceCollection services)
        {
            //services.AddTransient<IDeliveryFeeService, DeliveryFeeService>();
        }
    }
}
