using CSharpDevagram.Dto;
using CSharpDevagram.Models;
using CSharpDevagram.Repository;
using CSharpDevagram.Repository.Impl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSharpDevagram.Controllers
{

	[ApiController]
	[Route("api/[controller]")]
	public class UsuarioController : BaseController
	{
		private readonly ILogger<UsuarioController> _logger;

		public UsuarioController(ILogger<UsuarioController> logger, 
			IUsuarioRepository usuarioRepository) : base(usuarioRepository)
		{
			_logger = logger;
		}

		[HttpGet]
		public IActionResult ObterUsuario()
		{
			try
			{
				Usuario usuario = LerToken();

				return Ok( new UsuarioRespostaDto
				{
					Nome = usuario.Nome,
					Email = usuario.Email
				});
			}
			catch (Exception ex)
			{
				_logger.LogError("Ocorreu um ao obter o usuário: ");
				return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
				{
					Descricao = "Ocorreu um ao obter o usuário" + ex.Message,
					Status = StatusCodes.Status500InternalServerError
				});
			}
		}

		[HttpPost]
		[AllowAnonymous]
		public IActionResult CadastrarUsuario([FromBody] Usuario usuario)
		{
			try
			{
				if(usuario != null)
				{
					var erros = new List<string>();

					if(string.IsNullOrEmpty(usuario.Nome) || string.IsNullOrWhiteSpace(usuario.Nome))
					{
						erros.Add("Nome inválido");
					}

					if (string.IsNullOrEmpty(usuario.Email) || string.IsNullOrWhiteSpace(usuario.Email) || !usuario.Email.Contains("@"))
					{
						erros.Add("Email inválido");
					}

					if (string.IsNullOrEmpty(usuario.Senha) || string.IsNullOrWhiteSpace(usuario.Senha))
					{
						erros.Add("Senha inválida");
					}

					if(erros.Count > 0)
					{
						return BadRequest(new ErrorRespostaDto()
						{
							Status = StatusCodes.Status400BadRequest,
							Erros = erros
						});
					}

					usuario.Senha = Utils.Md5Utils.GerarHashMD5(usuario.Senha);
					usuario.Email = usuario.Email.ToLower();

					if (!_usuariorepository.VerificarEmail(usuario.Email))
					{
						_usuariorepository.Salvar(usuario);
					}
					else
					{
						return BadRequest(new ErrorRespostaDto()
						{
							Status = StatusCodes.Status400BadRequest,
							Descricao = "Usuário já esta cadastrado!"
						});
					}
				}

				return Ok("Usuário cadastrado com sucesso!");
			}
			catch(Exception ex)
			{
				_logger.LogError("Ocorreu um ao obter o usuário: " + ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
				{
					Descricao = "Ocorreu um ao obter o usuário",
					Status = StatusCodes.Status500InternalServerError
				});
			}
		}

	}

}
