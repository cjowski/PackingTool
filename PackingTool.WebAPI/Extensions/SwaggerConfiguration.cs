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
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            return services;
        }

        public static IApplicationBuilder AddSwaggerUI(
            this IApplicationBuilder app
        )
        {
            app.UseMiddleware<Middleware.SwaggerAuthenticateMiddleware>();
            app.UseSwagger();
            app.UseSwaggerUI(setup =>
            {
                setup.DefaultModelsExpandDepth(-1);
            });

            return app;
        }
    }
}
