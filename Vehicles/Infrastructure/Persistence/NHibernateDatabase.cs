using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.Options;
using NHibernate;
using System.Reflection;
using Vehicles.Infrastructure.Configurations;

namespace Vehicles.Infrastructure.Persistence
{
    public interface INHibernateDatabase
    {
        public NHibernate.ISession OpenSession();
    }

    public class NHibernateDatabase(IOptions<DatabaseConfig> options, Assembly mappingAssembly) : INHibernateDatabase
    {
        private readonly ISessionFactory sessionFactory = Fluently
            .Configure()
            .Database(PostgreSQLConfiguration.Standard
            .ConnectionString(options.Value.DefaultConnection))
            .Mappings(m => m.FluentMappings.AddFromAssembly(mappingAssembly))
            .BuildSessionFactory();

        public NHibernate.ISession OpenSession() => sessionFactory.OpenSession();
    }
}
