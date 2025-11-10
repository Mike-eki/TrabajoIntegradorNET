using System;
using Microsoft.Data.SqlClient;

namespace ADO.NET
{
    public sealed class DbConnectionFactory
    {
        private readonly string _masterConnectionString;
        private readonly string _appConnectionString;

        public DbConnectionFactory(string appConnectionString)
        {
            if (string.IsNullOrWhiteSpace(appConnectionString))
                throw new ArgumentException("Connection string is required.", nameof(appConnectionString));

            var builder = new SqlConnectionStringBuilder(appConnectionString);
            if (string.IsNullOrWhiteSpace(builder.InitialCatalog))
                throw new ArgumentException("Initial Catalog is required.", nameof(appConnectionString));

            // Conexión a la base de datos de la aplicación
            _appConnectionString = builder.ConnectionString;

            // Conexión a 'master' para operaciones de creación de DB
            builder.InitialCatalog = "master";
            _masterConnectionString = builder.ConnectionString;
        }

        public SqlConnection CreateMaster() => new SqlConnection(_masterConnectionString);
        public SqlConnection CreateApp() => new SqlConnection(_appConnectionString);
    }
}
