// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Reflection;
using Skoruba.Duende.IdentityServer.Admin.EntityFramework.Configuration.Configuration;
using SqlMigrationAssembly = LiveAuth.Admin.EntityFramework.SqlServer.Helpers.MigrationAssembly;
using SqliteMigrationAssembly = LiveAuth.Admin.EntityFramework.SqlLite.Helpers.MigrationAssembly;

namespace LiveAuth.Admin.Configuration.Database
{
    public static class MigrationAssemblyConfiguration
    {
        public static string GetMigrationAssemblyByProvider(DatabaseProviderConfiguration databaseProvider)
        {
            return databaseProvider.ProviderType switch
            {
                DatabaseProviderType.SqlServer => typeof(SqlMigrationAssembly).GetTypeInfo().Assembly.GetName().Name,
                _ => typeof(SqliteMigrationAssembly).GetTypeInfo().Assembly.GetName().Name,
            };
        }
    }
}







