using Azure.Data.Tables;
using Azure.Storage.Queues;
using Azure_Storage_Account.Services;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Azure;

namespace Azure_Storage_Account
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var ConnectionString = builder.Configuration["AzureStorage:ConnectionString"];

            // Add services to the container.
            builder.Services.AddScoped<ITableStorageService, TableStorageService>();
            builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();
            builder.Services.AddScoped<IQueueStorageService, QueueStorageService>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddAzureClients(b => {
                b.AddClient<QueueClient, QueueClientOptions>((_, _, _) =>
                {
                    return new QueueClient(ConnectionString,
                                    builder.Configuration["AzureStorage:QueueName"],
                                    new QueueClientOptions()
                                    {
                                        MessageEncoding = QueueMessageEncoding.Base64
                                    });
                });

                b.AddClient<TableClient, TableClientOptions>((_, _, _) =>
                {
                    return new TableClient(ConnectionString,
                                    builder.Configuration["AzureStorage:TableName"]);
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
