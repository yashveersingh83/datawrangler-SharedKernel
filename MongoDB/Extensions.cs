using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using SharedKernel.Settings;
using StackExchange.Redis;
using System;

namespace SharedKernel.MongoDB
{
    public static class Extensions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services)
        {
            // Register MongoDB serializers for Guid and DateTimeOffset
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));


            services.AddSingleton(serviceProvider =>
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                var logger = serviceProvider.GetService<ILogger<MongoDbSettings>>();
                var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

                var mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();

                if (mongoDbSettings == null)
                {
                    throw new ArgumentNullException(nameof(MongoDbSettings), "MongoDB settings are missing in the configuration.");
                }
                var conString = $"mongodb+srv://yashveersingh83:Ruchi123@yashmongocluster.whtjz12.mongodb.net/?retryWrites=true&w=majority&appName=YashMongoCluster";
                var mongoClient = new MongoClient(conString);
                var database = mongoClient.GetDatabase(serviceSettings.ServiceName);

                if (logger != null)
                {
                    logger.LogInformation("Connected to MongoDB at {ConnectionString}", conString);
                }
                else
                {
                    Console.WriteLine($"Connected to MongoDB at {conString}");
                }

                return database;
            });

            
         

            return services;
        }

        public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName)
    where T : IEntity
        {
            services.AddSingleton<IRepository<T>>(serviceProvider =>
            {
                var database = serviceProvider.GetService<IMongoDatabase>();
                return new MongoRepository<T>(database, collectionName);
            });

            return services;
        }

        public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            // Get Redis settings from appsettings.json
            var redisSettings = configuration.GetSection(nameof(RedisCacheSettings)).Get<RedisCacheSettings>();

            if (redisSettings == null || string.IsNullOrEmpty(redisSettings.ConnectionString))
            {
                throw new ArgumentNullException("Redis connection string is missing in the configuration.");
            }

            
            // Register Redis connection multiplexer as a singleton
            services.AddSingleton<IConnectionMultiplexer>(sp =>
                ConnectionMultiplexer.Connect($"{redisSettings.ConnectionString}"));

            // Register Redis Cache Service
            services.AddScoped<IRedisCacheService, RedisCacheService>();

            return services;
        }

    }
}