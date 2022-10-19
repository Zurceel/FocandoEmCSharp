using CSharpDevagram.Dto;
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
				throw new ArgumentException("Erro ao preencher dados");
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
