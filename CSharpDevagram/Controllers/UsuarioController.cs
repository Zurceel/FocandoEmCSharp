using CSharpDevagram.Dto;
using CSharpDevagram.Models;
using CSharpDevagram.Repository;
using CSharpDevagram.Repository.Impl;
using CSharpDevagram.Services;
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
		public IActionResult CadastrarUsuario([FromForm] UsuarioRequisicaoDto usuariodto)
		{
			try
			{



				if(usuariodto != null)
				{
					var erros = new List<string>();

					if(string.IsNullOrEmpty(usuariodto.Nome) || string.IsNullOrWhiteSpace(usuariodto.Nome))
					{
						erros.Add("Nome inválido");
					}

					if (string.IsNullOrEmpty(usuariodto.Email) || string.IsNullOrWhiteSpace(usuariodto.Email) || !usuariodto.Email.Contains("@"))
					{
						erros.Add("Email inválido");
					}

					if (string.IsNullOrEmpty(usuariodto.Senha) || string.IsNullOrWhiteSpace(usuariodto.Senha))
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

					CosmicService cosmicService = new CosmicService();

					Usuario usuario = new Usuario()
					{
						Email = usuariodto.Email,
						Senha = usuariodto.Senha,
						Nome = usuariodto.Nome,
						FotoPerfil = cosmicService.EnviarImagem(new ImagemDto { Imagem = usuariodto.FotoPerfil, Nome = usuariodto.Nome.Replace(" ","")})
					};



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
