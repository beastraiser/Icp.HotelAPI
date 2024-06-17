using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Icp.HotelAPI.ServiciosCompartidos.LoginService.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Icp.HotelAPI.Controllers.UsuariosController.DTO;

namespace Icp.HotelAPI.ServiciosCompartidos.LoginService
{
    public class LoginService : ILoginService
    {
        private readonly FCT_ABR_11Context context;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public LoginService(FCT_ABR_11Context context,
            IMapper mapper,
            IConfiguration configuration)
        {
            this.context = context;
            this.mapper = mapper;
            this.configuration = configuration;
        }
        
        public RespuestaAutenticacionDTO ConstruirToken(List<Claim> claims)
        {
            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwtkey"]));
            var credenciales = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.Now.AddHours(1);

            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiracion, signingCredentials: credenciales);

            return new RespuestaAutenticacionDTO()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expire = expiracion
            };
        }

        public string HashContrasenya(string contrasenya)
        {
            if (IsPasswordHashed(contrasenya))
            {
                return contrasenya;
            }
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(contrasenya));
                var hashedString = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                return hashedString;
            }
        }

        public bool VerificarContrasenya(string contrasenya, string hash)
        {
            var nuevoHash = HashContrasenya(contrasenya);
            return nuevoHash == hash;
        }

        private bool IsPasswordHashed(string password)
        {
            return password.Length == 64;
        }
    }
}
