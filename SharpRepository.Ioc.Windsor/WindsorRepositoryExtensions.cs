using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.Extensions.Configuration;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.Ioc.Windsor
{
    public static class WindsorRepositoryExtensions
    {
        public static void RegisterSharpRepository(this IWindsorContainer container, IConfigurationSection configurationSection, string repositoryName = null)
        {
            if (configurationSection == null)
                throw new ConfigurationErrorsException("Configuration section not found.");

            var configuration = RepositoryFactory.BuildSharpRepositoryConfiguation(configurationSection);

            container.RegisterSharpRepository(configuration, repositoryName);
        }

        public static void RegisterSharpRepository(this IWindsorContainer container, ISharpRepositoryConfiguration configuration, string repositoryName = null)
        {
            container.Register(Component.For(typeof(IRepository<>)).UsingFactoryMethod((c, t) =>
                {
                    var genericArgs = t.GenericArguments;

                    return RepositoryFactory.GetInstance(genericArgs[0], configuration, repositoryName);
                }));

            container.Register(Component.For(typeof(IRepository<,>)).UsingFactoryMethod((c, t) =>
                {
                    var genericArgs = t.GenericArguments;

                    return RepositoryFactory.GetInstance(genericArgs[0], genericArgs[1], configuration, repositoryName);
                }));

            container.Register(Component.For(typeof(ICompoundKeyRepository<>)).UsingFactoryMethod((c, t) =>
            {
                var genericArgs = t.GenericArguments;

                return RepositoryFactory.GetCompoundKeyInstance(genericArgs[0], configuration, repositoryName);
            }));

            container.Register(Component.For(typeof(ICompoundKeyRepository<,,>)).UsingFactoryMethod((c, t) =>
            {
                var genericArgs = t.GenericArguments;

                return RepositoryFactory.GetInstance(genericArgs[0], genericArgs[1], genericArgs[2], configuration, repositoryName);
            }));

            container.Register(Component.For(typeof(ICompoundKeyRepository<,,,>)).UsingFactoryMethod((c, t) =>
            {
                var genericArgs = t.GenericArguments;

                return RepositoryFactory.GetInstance(genericArgs[0], genericArgs[1], genericArgs[2], genericArgs[3], configuration, repositoryName);
            }));
        }
    }
}
