
using Just_GettingStarted_Jwt_Api.Helper;
using Just_GettingStarted_Jwt_Api.Models;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;

namespace Just_GettingStarted_Jwt_Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            // Add services to the container.
            builder.Services.Configure<AuthMongoDBSettings>(builder.Configuration.GetSection("AuthMongoDBSettings"));
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

            // Bind settings and register MongoClient as a Singleton
            builder.Services.AddSingleton<IMongoClient>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<AuthMongoDBSettings>>().Value;
                try
                {
                    return new MongoClient(settings.ConnectionString);
                }
                catch (MongoConfigurationException)
                {
                    throw new Exception("Configuration Problem");
                }
            });

            // Register IMongoDatabase as Scoped
            builder.Services.AddScoped(sp => {
                var settings = sp.GetRequiredService<IOptions<AuthMongoDBSettings>>().Value;
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase(settings.Database);
            });

            builder.Services.AddScoped<IAuthService, AuthMongoDBRepo>();
            builder.Services.AddTransient<ICryptHelper, BCryptHasher>();
            builder.Services.AddTransient<ITokenHelper, TokenHandler>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            // builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
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
