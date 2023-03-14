using Microsoft.OpenApi.Models;

namespace PackingTool.WebAPI.Extensions
{
    internal static class SwaggerConfiguration
    {
        public static IServiceCollection AddSwagger(
            this IServiceCollection services
        )
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = ParameterLocation.Header,
                    Description = "Basic Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "basic"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            return services;
        }

        public static IApplicationBuilder AddSwaggerUI(
            this IApplicationBuilder app
        )
        {
            //app.UseMiddleware<Middleware.SwaggerAuthenticateMiddleware>();
            app.UseSwagger();
            app.UseSwaggerUI(setup =>
            {
                setup.DefaultModelsExpandDepth(-1);
            });

            return app;
        }
    }
}
