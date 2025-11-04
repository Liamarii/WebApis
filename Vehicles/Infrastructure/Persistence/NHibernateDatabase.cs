using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.Options;
using NHibernate;
using System.Reflection;
using Vehicles.Infrastructure.Configurations;

namespace Vehicles.Infrastructure.Persistence;

public interface INHibernateDatabase
{
    public NHibernate.ISession OpenSession();
}

public class NHibernateDatabase : INHibernateDatabase
{
    private readonly ISessionFactory _sessionFactory;

    public NHibernateDatabase(IOptions<DatabaseConfig> options, Assembly mappingAssembly)
    {
        string? connectionString = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Testing") ?
        Environment.GetEnvironmentVariable("TEST_CREDENTIALS") : options.Value.DefaultConnection;

        _sessionFactory = Fluently
            .Configure()
            .Database(PostgreSQLConfiguration.Standard
            .ConnectionString(connectionString))
            .Mappings(m => m.FluentMappings.AddFromAssembly(mappingAssembly))
            .BuildSessionFactory();
    }

    public NHibernate.ISession OpenSession() => _sessionFactory.OpenSession();
}
