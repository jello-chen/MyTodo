using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;

namespace MyTodo
{
    public static class WebHostBuilderExtension
    {
        public static IWebHostBuilder UseCmdUrl(this IWebHostBuilder webHostBuilder, string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();
            String ip = string.IsNullOrWhiteSpace(config["ip"]) ? "127.0.0.1" : config["ip"];
            String port = string.IsNullOrWhiteSpace(config["port"]) ? "5000" : config["port"];
            return webHostBuilder.UseUrls($"http://{ip}:{port}");
        }
    }
}
