using CodeSnippetRunner.CodeExecutorMicroservice.Models;
using CodeSnippetRunner.CodeExecutorMicroservice.Services;
using CodeSnippetRunner.CodeExecutorMicroservice.Services.Interfaces;
using RabbitMQ.Client;

namespace CodeSnippetRunner.CodeExecutorMicroservice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;
            
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddTransient<ICodeExecutionService, CodeExecutionService>();
            builder.Services.AddTransient<IBlobStorageService, BlobStorageService>();
            builder.Services.Configure<RabbitMQConfiguration>(builder.Configuration.GetSection("RabbitMQConfiguration"));
            builder.Services.AddSingleton<IConnection>(serviceProvider =>
            {
                var factory = new ConnectionFactory()
                {
                    UserName = configuration["RabbitMQConfiguration:Username"],  
                    Password = configuration["RabbitMQConfiguration:Password"],
                    HostName = configuration["RabbitMQConfiguration:Hostname"],
                    VirtualHost = configuration["RabbitMQConfiguration:VirtualHost"],
                    Port = int.Parse(configuration["RabbitMQConfiguration:Port"])
                };
                return factory.CreateConnection();
            });
            builder.Services.AddSingleton<IRabbitMQListenerService, RabbitMQListenerService>();
            builder.Services.AddSingleton<IRabbitMQResponseService, RabbitMQResponseService>();
            builder.Services.AddHostedService<RabbitMQListenerHostedService>();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}