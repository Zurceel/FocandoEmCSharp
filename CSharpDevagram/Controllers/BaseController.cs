using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSharpDevagram.Controllers
{
	[Authorize]
	public class BaseController : ControllerBase
	{
		
	}
}
