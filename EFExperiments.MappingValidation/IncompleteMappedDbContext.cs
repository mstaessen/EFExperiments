using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using Xunit;

namespace EFExperiments.MappingValidation
{
    public class IncompleteMappedDbContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
        }
    }

    public class MyEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
    }

    public class MyEntityMappingConfiguration : EntityTypeConfiguration<MyEntity>
    {
        public MyEntityMappingConfiguration()
        {
            ToTable("my_entity");
            Property(x => x.Id).HasColumnName("Id");
            Property(x => x.Name).HasColumnName("Name");

        }
    }

    public class ValidationFacts
    {
        [Fact]
        public void ShouldFindNotMappedEmailProperty()
        {
            
            var entityTypeConfigurations = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.Implements(typeof(EntityTypeConfiguration<>))).ToArray();
            var tableConfigurations = GetTableConfigurations(entityTypeConfigurations).ToArray();


        }

        private static IEnumerable<TableConfiguration> GetTableConfigurations(Type[] entityTypeConfigurations)
        {
            foreach (var configurationType in entityTypeConfigurations) {
                var instance = Activator.CreateInstance(configurationType);
                var configurationProperty = configurationType.GetProperty("Configuration", BindingFlags.NonPublic | BindingFlags.Instance);
                var internalConfiguration = configurationProperty.GetValue(instance);
                yield return CreateTableConfiguration(internalConfiguration);
            }
        }

        private static TableConfiguration CreateTableConfiguration(object configurationInstance)
        {
            var entityTypeConfigurationType = Type.GetType("System.Data.Entity.ModelConfiguration.Configuration.Types.EntityTypeConfiguration, EntityFramework", true);
            if (configurationInstance.GetType() != entityTypeConfigurationType) {
                throw new InvalidOperationException("configuration is not an EntityTypeConfiguration");
            }
            return new TableConfiguration(configurationInstance);
        }
    }

    public class TableConfiguration
    {
        public TableConfiguration(object configurationInstance)
        {
            TableName = GetPropertyValue<string>(configurationInstance, "TableName");
            SchemaName = GetPropertyValue<string>(configurationInstance, "SchemaName");
            ClrType = GetPropertyValue<Type>(configurationInstance, "ClrType");
        }

        public string SchemaName { get; }

        public string TableName { get; }

        public Type ClrType { get; set; }

        public IReadOnlyCollection<PropertyConfiguration> Properties { get; }

        public IReadOnlyCollection<PropertyConfiguration> KeyProperties { get; }

        private static T GetPropertyValue<T>(object configurationInstance, string propertyName)
        {
            var property = configurationInstance.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return (T) property.GetValue(configurationInstance);
        }

        public class PropertyConfiguration
        {
            public string ColumnName { get; }

        }
    }

    

    public static class TypeExtensions
    {
        public static IEnumerable<Type> GetImplementedTypes(this Type type)
        {
            return type.GetBaseTypes().Concat(type.GetInterfaces());
        }

        public static IEnumerable<Type> GetBaseTypes(this Type type)
        {
            if (type == null) {
                throw new ArgumentNullException(nameof(type));
            }
            var loopType = type;
            while (loopType != null) {
                yield return loopType;
                loopType = loopType.BaseType;
            }
        }

        public static bool Implements(this Type type, Type otherType)
        {
            return type.Implements(otherType, true);
        }

        public static bool Implements(this Type type, Type otherType, bool ignoreGenerics)
        {
            var implementedTypes = type.GetImplementedTypes();
            foreach (var implementedType in implementedTypes) {
                if (otherType == implementedType) {
                    return true;
                }

                if (otherType.IsGenericType 
                    && !otherType.IsConstructedGenericType 
                    && implementedType.IsGenericType 
                    && implementedType.IsConstructedGenericType 
                    && ignoreGenerics) {
                    var constructedOtherType = otherType.MakeGenericType(implementedType.GetGenericArguments());
                    if (implementedType.IsAssignableFrom(constructedOtherType)) {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
