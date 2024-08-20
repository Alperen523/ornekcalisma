using Autofac;
using Autofac.Extras.NLog;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using UzmanCrm.CrmService.Application.Abstractions;

namespace UzmanCrm.CrmService.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        //Service must be end with this value
        private const string ServiceEndName = "Service";

        /// <summary>
        /// Registers the application services.
        /// </summary>
        /// <param name="builder">Autofac ContainerBuilder</param>
        /// <returns>Added assembly with container</returns>
        public static ContainerBuilder RegisterApplicationServicesAPI(ContainerBuilder builder)
        {
            var assemblyList = BuildManager.GetReferencedAssemblies();
            var allAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToArray();

            // If use project for api use bottom line 
            builder.RegisterApiControllers(allAssemblies);

            //Service Assembly not interface
            var assemblies = allAssemblies.Where(_ => _.FullName.Contains(ServiceEndName) && _.GetTypes() != null && _.GetTypes()
              .Where(t => typeof(IApplicationService).IsAssignableFrom(t) && !t.IsInterface)
              .Any()).ToArray();

            builder.RegisterAssemblyTypes(assemblies).Where(t => t.Name.EndsWith(ServiceEndName)).AsImplementedInterfaces();
            builder.RegisterType<MemoryCache>().As<IMemoryCache>().SingleInstance();
            return builder;
        }

        /// <summary>
        /// Registers the application services.
        /// </summary>
        /// <param name="builder">Autofac ContainerBuilder</param>
        /// <returns>Added assembly with container</returns>
        public static ContainerBuilder RegisterApplicationServicesUI(ContainerBuilder builder)
        {
            try
            {
                var assemblyList = BuildManager.GetReferencedAssemblies();
                var allAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToArray();
                // If use project for mvc use bottom line
                builder.RegisterControllers(allAssemblies);

                //Service Assembly not interface
                //Service Assembly not interface
                var assemblies = allAssemblies.Where(_ => _ != null && _.FullName.Contains(ServiceEndName) && _.GetTypes() != null).ToArray();
                assemblies = allAssemblies.Where(_ => _.FullName.Contains(ServiceEndName) && _.GetTypes() != null && _.GetTypes().Where(t => typeof(IApplicationService).IsAssignableFrom(t)).Any()).ToArray();
                //var assemblies = allAssemblies.Where(_ => _.FullName.Contains(ServiceEndName) && _.GetTypes() != null && _.GetTypes()
                //  .Where(t => typeof(IApplicationService).IsAssignableFrom(t) && !t.IsInterface)
                //  .Any()).ToArray();

                builder.RegisterAssemblyTypes(assemblies).Where(t => t.Name.EndsWith(ServiceEndName)).AsImplementedInterfaces();
                builder.RegisterType<MemoryCache>().As<IMemoryCache>().SingleInstance();
                builder.RegisterGeneric(typeof(OptionsManager<>)).As(typeof(IOptions<>)).SingleInstance();
                builder.RegisterGeneric(typeof(OptionsManager<>)).As(typeof(IOptionsSnapshot<>)).InstancePerLifetimeScope();
                builder.RegisterGeneric(typeof(OptionsMonitor<>)).As(typeof(IOptionsMonitor<>)).SingleInstance();
                builder.RegisterGeneric(typeof(OptionsFactory<>)).As(typeof(IOptionsFactory<>));
                builder.RegisterGeneric(typeof(OptionsCache<>)).As(typeof(IOptionsMonitorCache<>)).SingleInstance();
                builder.RegisterFilterProvider();
                
            }
            catch (ReflectionTypeLoadException ex)
            {
                var loaderExceptions = ex.LoaderExceptions;
                foreach (var exception in loaderExceptions)
                {
                    Console.WriteLine(exception.Message);
                }
            }
            return builder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ContainerBuilder RegisterAutoMapperProfile(ContainerBuilder builder)
        {
            var assemblyList = BuildManager.GetReferencedAssemblies();
            var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            return builder.RegisterAutoMapper(allAssemblies);
        }

        ///<summary>
        ///RegisterNLog nlog register.
        ///</summary>
        ///<param name="builder">ContainerBuilder</param>
        ///<returns></returns>
        public static ContainerBuilder RegisterNLog(ContainerBuilder builder)
        {
            var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            builder.RegisterModule<NLogModule>();

            return builder;
        }
    }
}
