﻿using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.WindowsAzure.Storage;
using System.Configuration;
using System.Web.Mvc;
using Vincente.Azure.Tables;
using Vincente.Cached;
using Vincente.Data.Interfaces;
using Vincente.Data.Tables;

namespace WebApp.App_Start
{
    public class AutofaqConfig
    {
        public static void Configure()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();
            RegisterRepositories(builder);
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private static void RegisterRepositories(ContainerBuilder builder)
        {
            var azureConnectionString = ConfigurationManager.AppSettings["azureConnectionString"];
            var azureStorageAccount = CloudStorageAccount.Parse(azureConnectionString);
            var azureTableClient = azureStorageAccount.CreateCloudTableClient();

            builderRegisterCardTableWithCache(builder, azureTableClient);
            builderRegisterTimeEntryTableWithCache(builder, azureTableClient);

            builder.RegisterType<CardsWithTime>();
            builder.RegisterType<Housekeeping>();
            builder.RegisterType<InvoiceData>();
            builder.RegisterType<TimeEntriesByMonth>();
        }

        private static void builderRegisterCardTableWithCache(ContainerBuilder builder, Microsoft.WindowsAzure.Storage.Table.CloudTableClient azureTableClient)
        {
            builder.RegisterType<CardTable>()
                .Named<ICardRead>("AzureCardTable")
                .WithParameter("table", azureTableClient.GetTableReference("Cards"));

            builder.RegisterType<CachedCardTable>()
                .As<ICardRead>()
                .WithParameter(Autofac.Core.ResolvedParameter.ForNamed<ICardRead>("AzureCardTable"));
        }

        private static void builderRegisterTimeEntryTableWithCache(ContainerBuilder builder, Microsoft.WindowsAzure.Storage.Table.CloudTableClient azureTableClient)
        {
            builder.RegisterType<TimeEntryTable>()
                .Named<ITimeEntryRead>("AzureTimeEntryTable")
                .WithParameter("table", azureTableClient.GetTableReference("TimeEntries"));

            builder.RegisterType<CachedTimeEntryTable>()
                .As<ITimeEntryRead>()
                .WithParameter(Autofac.Core.ResolvedParameter.ForNamed<ITimeEntryRead>("AzureTimeEntryTable"));
        }
    }
}