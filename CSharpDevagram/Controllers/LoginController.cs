using CSharpDevagram.Dto;
using CSharpDevagram.Models;
using CSharpDevagram.Repository;
using CSharpDevagram.Services;
using CSharpDevagram.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSharpDevagram.Controllers
{

	[ApiController]
	[Route("api/[controller]")]
	public class LoginController : ControllerBase
	{

		private readonly ILogger<LoginController> _logger;
		private readonly IUsuarioRepository _usuarioRepository;

		public LoginController(ILogger<LoginController> logger, IUsuarioRepository usuarioRepository)
		{
			_logger = logger;
			_usuarioRepository = usuarioRepository;
		}

		[HttpPost]
		[AllowAnonymous]

		public IActionResult EfetuarLogin([FromBody] LoginRequisicaoDto loginrequisicao)
		{
			try
			{
				if(!String.IsNullOrEmpty(loginrequisicao.Email) && !String.IsNullOrEmpty(loginrequisicao.Senha) && 
					!String.IsNullOrWhiteSpace(loginrequisicao.Email) && !String.IsNullOrWhiteSpace(loginrequisicao.Senha))
				{

					Usuario usuario = _usuarioRepository.GetUsuarioPorLoginSenha(loginrequisicao.Email.ToLower(), Md5Utils.GerarHashMD5(loginrequisicao.Senha));

					if(usuario != null)
					{
						return Ok(new LoginRespostaDto()
						{
							Email = usuario.Email,
							Nome = usuario.Nome,
							Token = TokenService.CriarToken(usuario)

						});
					}
					else
					{
						return BadRequest(new ErrorRespostaDto()
						{
							Descricao = "E-mail ou senha inválido",
							Status = StatusCodes.Status400BadRequest
						});
					}
				}
				else
				{
					return BadRequest(new ErrorRespostaDto()
					{
						Descricao = "Campos de login não foram preenchidos corretamente.",
						Status = StatusCodes.Status400BadRequest
					});
				}
			}
			catch(Exception ex)
			{
				_logger.LogError("Ocorreu um erro ao fazer login: " + ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
				{
					Descricao = "Ocorreu um erro ao fazer login",
					Status = StatusCodes.Status500InternalServerError
				});
			}

		}



	}
}
