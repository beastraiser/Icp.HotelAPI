using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Icp.HotelAPI.Servicios.CategoriasService;
using Icp.HotelAPI.Servicios.CategoriasService.Interfaces;
using Icp.HotelAPI.Servicios.ClientesUsuariosService;
using Icp.HotelAPI.Servicios.ClientesUsuariosService.Interfaces;
using Icp.HotelAPI.Servicios.ReservasService;
using Icp.HotelAPI.Servicios.ReservasService.Interfaces;
using Icp.HotelAPI.ServiciosCompartidos.AlmacenadorArchivosLocal.Interfaces;
using Icp.HotelAPI.ServiciosCompartidos.AlmacenadorArchivosLocalService;
using Icp.HotelAPI.ServiciosCompartidos.LoginService;
using Icp.HotelAPI.ServiciosCompartidos.LoginService.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json.Serialization;

namespace Icp.HotelAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            // Limpia los mapeos por defecto de los claim, para asi poder acceder a su contenido
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //Configuración del almacenador de archivos local
            services.AddTransient<IAlmacenadorArchivos, AlmacenadorArchivosLocal>();
            services.AddHttpContextAccessor();

            // Configuración de AutoMapper
            services.AddAutoMapper(typeof(Startup));

            // Configuración de la conexión
            services.AddDbContext<FCT_ABR_11Context>(options => options.UseSqlServer(Configuration.GetConnectionString("HomeConnection")));

            services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles).AddNewtonsoftJson();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["jwtkey"])),
                    ClockSkew = TimeSpan.Zero
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ADMIN", policy => policy.RequireClaim("ADMIN"));
                options.AddPolicy("RECEPCION", policy => policy.RequireClaim("RECEPCION", "ADMIN"));
                options.AddPolicy("CLIENTE", policy => policy.RequireClaim("CLIENTE", "RECEPCION", "ADMIN"));
            });

            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IClienteUsuarioService, ClientesUsuariosService>();
            services.AddScoped<IReservaService, ReservasService>();
            services.AddScoped<ICategoriaService, CategoriasService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            // Para que la api pueda servir contenido estatico
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

