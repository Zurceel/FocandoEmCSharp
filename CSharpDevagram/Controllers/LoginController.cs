using CSharpDevagram.Dto;
using CSharpDevagram.Models;
using CSharpDevagram.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSharpDevagram.Controllers
{

	[ApiController]
	[Route("api/[controller]")]
	public class LoginController : ControllerBase
	{

		private readonly ILogger<LoginController> _logger;

		public LoginController(ILogger<LoginController> logger)
		{
			_logger = logger;
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
					string email = "gabriel@gmail.com";
					string senha = "Senha123";

					if(loginrequisicao.Email == email && loginrequisicao.Senha == senha)
					{

						Usuario usuario = new Usuario()
						{
							Email = loginrequisicao.Email,
							Id = 123,
							Nome = "Gabriel da Cruz"
						};


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
