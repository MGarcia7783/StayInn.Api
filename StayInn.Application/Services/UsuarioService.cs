
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StayInn.Application.DTOs.Usuario;
using StayInn.Application.Interfaces.Persistence;
using StayInn.Application.Interfaces.Service;
using StayInn.Application.Response;
using StayInn.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StayInn.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public UsuarioService(IUsuarioRepository repository, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IConfiguration config)
        {
            _repository = repository;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _config = config;
        }

        #region Métodos Privados

        private async Task<UsuarioDto> MapearUsuarioDTOAsync(ApplicationUser usuario)
        {
            var roles = await _userManager.GetRolesAsync(usuario);

            return new UsuarioDto
            {
                Id = usuario.Id,
                NombreCompleto = usuario.NombreCompleto,
                Email = usuario.Email!,
                Rol = roles.FirstOrDefault() ?? "",
                PhoneNumber = usuario.PhoneNumber ?? "",
                Activo = usuario.Activo
            };
        }

        private static void ValidarResultado(IdentityResult resultado, string mensajeError)
        {
            if (!resultado.Succeeded)
            {
                var errores = string.Join(" | ", resultado.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"{mensajeError}: '{errores}'");
            }
        }

        private string GenerarToken(ApplicationUser usuario, string rol)
        {
            // Leer configuración de variables de entorno
            var key = _config["JWT_KEY"]
                ?? throw new Exception("JWT_KEY no está configurada en el entorno.");
            var issuer = _config["JWT_ISSUER"] ?? "StayInnApi";
            var audience = _config["JWT_AUDIENCE"] ?? "StayInnApiUser";


            // Convertir la clave secreta a bytes
            // Esto es necesario para crear la firma del token
            var keyBytes = Encoding.ASCII.GetBytes(key);


            // Crear claims (información que incluirá el token)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id),
                new Claim(ClaimTypes.Name, usuario.NombreCompleto ?? string.Empty),
                new Claim(ClaimTypes.Email, usuario.UserName ?? string.Empty),
                new Claim(ClaimTypes.Role, rol),
                new Claim("Activo", usuario.Activo.ToString())
            };


            // Preparar el descriptor del token
            // Aquí se define la información del token: claims, expiración, issuer, audience y firma
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),           // Claim que incluirá el token
                Expires = DateTime.UtcNow.AddDays(7),           // El token expira en 7 días
                Issuer = issuer,                                // Quién emite el token
                Audience = audience,                            // A quien va dirigido el token
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(keyBytes),         // Clave secreta en bytes
                    SecurityAlgorithms.HmacSha256Signature)     // Algoritmo de firma HMAC SHA256
            };

            // Generar el token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Devolver el token como string
            return tokenHandler.WriteToken(token);
        }

        #endregion

        public async Task CambiarEstadoAsync(string id, bool activo)
        {
            var usuario = await _repository.ObtenerPorIdAsync(id);
            if (usuario == null)
                throw new KeyNotFoundException("Usuario no encontrado.");

            usuario.Activo = activo;
            await _userManager.UpdateAsync(usuario);
        }

        public async Task<int> ContarAsycn()
        {
            return await _repository.ContarAsync();
        }

        public async Task<LoginRespuestaUsuarioDto> LoginAsync(UsuarioLoginDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "El campo es requerido.");

            var usuario = await _userManager.FindByEmailAsync(dto.Email);

            if (usuario == null)
                throw new UnauthorizedAccessException("Usuario no registrado.");

            if (!usuario.Activo)
                throw new UnauthorizedAccessException("La cuenta esta suspendida.");

            if (!await _userManager.CheckPasswordAsync(usuario, dto.Password))
                throw new UnauthorizedAccessException("Contraseña incorrecta. Verifique por favor.");

            // Mapear el usuario a DTO
            var usuarioDto = await MapearUsuarioDTOAsync(usuario);

            return new LoginRespuestaUsuarioDto
            {
                Usuario = usuarioDto,
                Token = GenerarToken(usuario, usuarioDto.Rol)
            };
        }

        public async Task<UsuarioDto> ObtenerUsuarioPorIdAsync(string id)
        {
            var usuario = await _repository.ObtenerPorIdAsync(id);
            if (usuario == null)
                throw new KeyNotFoundException("Usuario no encontrado.");

            return await MapearUsuarioDTOAsync(usuario);
        }

        public async Task<ICollection<UsuarioDto>> ObtenerUsuariosAsync(int pagina, int tamanoPagina)
        {
            var usuarios = await _repository.ObtenerTodosAsync(pagina, tamanoPagina);
            var lista = new List<UsuarioDto>();

            foreach (var usuario in usuarios)
            {
                lista.Add(await MapearUsuarioDTOAsync(usuario));
            }

            return lista;
        }

        public async Task<UsuarioDto> RegistrarUsuario(UsuarioRegistroDto dto)
        {
            if(dto == null) 
                throw new ArgumentNullException(nameof(dto), "El campo es requerido.");

            // Validar si existe el email
            var existeEmail = await _userManager.FindByEmailAsync(dto.Email);
            if (existeEmail != null)
                throw new InvalidOperationException("El email ya esta registrado.");

            // Validar el rol
            var rolExiste = await _roleManager.RoleExistsAsync(dto.Rol);
            if (!rolExiste)
            {
                var nuevoRol = new IdentityRole(dto.Rol);
                var resultadoRol = await _roleManager.CreateAsync(nuevoRol);
                ValidarResultado(resultadoRol, "Error al crear el rol");
            }

            // Mapear el DTO a la entidad ApplicationUser
            var usuario = _mapper.Map<ApplicationUser>(dto);
            usuario.EmailConfirmed = true;
            usuario.PhoneNumberConfirmed = true;

            // Crear el usuario con el UserManager
            var usuarioCreado = await _userManager.CreateAsync(usuario, dto.Password);
            ValidarResultado(usuarioCreado, "Error al crear el usuario");

            // Asignar el rol al usuario
            var rolAsignado = await _userManager.AddToRoleAsync(usuario, dto.Rol);
            ValidarResultado(rolAsignado, "Error al asignar el rol al usuario");

            // Retornar el usuario creado como DTO
            return await MapearUsuarioDTOAsync(usuario);
        }
    }
}
