using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestWebApi.Data;
using TestWebApi.Models;

namespace TestWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var connection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ProductContext>(options => options.UseSqlServer(connection));


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ProductContext>();
                context.Database.Migrate();

                if (!context.Products.Any())
                {
                    context.Products.AddRange(
                        new Product { Name = "Redbull", Price = 3.4 },
                        new Product { Name = "Cola", Price = 1.5 },
                        new Product { Name = "Pringless", Price = 4.6 }
                        );

                    context.SaveChanges();
                }
            }

            app.MapGet("/products", async (ProductContext db) => await db.Products.ToListAsync());
            app.MapPost("/products", async (Product product, ProductContext db) =>
            {
                db.Products.Add(product);
                await db.SaveChangesAsync();
                return Results.Ok();
            });

            app.MapControllers();

            app.Run();
        }
    }
}