using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using Vehicles.Data.Mappings;

namespace Vehicles.Infrastructure
{
    public static class NHibernateHelper
    {
        private static ISessionFactory? sessionFactory;

        public static ISessionFactory GetSessionFactory()
        {
            sessionFactory ??= Fluently.Configure()
                    .Database(PostgreSQLConfiguration.Standard
                    .ConnectionString("Host=localhost;Database=vehiclesdb;Username=postgres;Password=postgres"))
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<VehicleMap>())
                    .BuildSessionFactory();
            return sessionFactory;
        }

        public static NHibernate.ISession OpenSession()
        {
            return GetSessionFactory().OpenSession();
        }
    }
}