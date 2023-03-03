using System.Reflection;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace MusicHub.Tests
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // ReSharper disable CheckNamespace, InconsistentNaming

        }

        [TestFixture]
        public class Test_001_000_001
        {
            private static readonly Assembly CurrentAssembly = typeof(StartUp).Assembly;

            [Test]
            public void ValidateModel()
            {
                var context = GetType("MusicHubDbContext");

                var dbSetData = new[]
                {
            new DbSetInfo("Songs", "Song", "Id Name Duration CreatedOn Genre AlbumId Album WriterId Writer Price SongPerformers".Split()),
            new DbSetInfo("Writers", "Writer", "Id Name Pseudonym Songs".Split()),
            new DbSetInfo("Producers","Producer", "Id Name Pseudonym PhoneNumber Albums".Split()),
            new DbSetInfo("Performers", "Performer", "Id FirstName LastName Age NetWorth PerformerSongs".Split()),
            new DbSetInfo("Albums", "Album", "Id Name ReleaseDate Price ProducerId Producer Songs".Split()),
            new DbSetInfo("SongsPerformers", "SongPerformer", "SongId Song PerformerId Performer".Split()),
        };

                foreach (var dbSetInfo in dbSetData)
                {
                    ValidateDbSet(context, dbSetInfo);
                }
            }

            private static void ValidateDbSet(Type context, DbSetInfo info)
            {
                var expectedDbSetType = GetDbSetType(info.EntityType);

                AssertCollectionIsOfType(context, info.DbSetName, expectedDbSetType);

                var modelType = GetType(info.EntityType);

                foreach (var property in info.Properties)
                {
                    var propertyType = GetPropertyByName(modelType, property);

                    var errorMessage = $"{modelType.Name}.{property} property does not exist!";
                    Assert.IsNotNull(propertyType, errorMessage);
                }
            }

            private static PropertyInfo GetPropertyByName(Type type, string propName)
            {
                var properties = type.GetProperties();

                var firstOrDefault = properties.FirstOrDefault(p => p.Name == propName);
                return firstOrDefault;
            }

            private static void AssertCollectionIsOfType(Type type, string propertyName, Type collectionType)
            {
                var property = GetPropertyByName(type, propertyName);

                var errorMessage = string.Format($"{type.Name}.{propertyName} property not found!");

                Assert.IsNotNull(property, errorMessage);

                Assert.That(collectionType.IsAssignableFrom(property.PropertyType));
            }

            private static Type GetDbSetType(string modelName)
            {
                var modelType = GetType(modelName);

                var dbSetType = typeof(DbSet<>).MakeGenericType(modelType);
                return dbSetType;
            }

            private static Type GetType(string typeName)
            {
                var modelType = CurrentAssembly
                    .GetTypes()
                    .FirstOrDefault(t => t.Name == typeName);

                Assert.IsNotNull(modelType, $"{typeName} not found!");

                return modelType;
            }

            private class DbSetInfo
            {
                public DbSetInfo(string dbSetName, string entityType, IEnumerable<string> properties)
                {
                    this.DbSetName = dbSetName;
                    this.EntityType = entityType;
                    this.Properties = properties;
                }

                public string DbSetName { get; }

                public string EntityType { get; }

                public IEnumerable<string> Properties { get; }
            }
        }
    }
}
