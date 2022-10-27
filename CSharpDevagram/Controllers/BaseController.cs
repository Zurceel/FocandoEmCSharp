using CSharpDevagram.Models;
using CSharpDevagram.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CSharpDevagram.Controllers
{
	[Authorize]
	public class BaseController : ControllerBase
	{
		protected readonly IUsuarioRepository _usuariorepository;

		public BaseController(IUsuarioRepository usuarioRepository)
		{
			_usuariorepository = usuarioRepository;
		}

		protected Usuario LerToken()
		{
			var idUsuario = User.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).FirstOrDefault();

			if (string.IsNullOrEmpty(idUsuario))
			{
				return null;
			}
			else
			{
				return _usuariorepository.GetUsuarioPorId(int.Parse(idUsuario));
			}

		}
	}
}
