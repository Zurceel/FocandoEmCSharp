using CSharpDevagram.Models;

namespace CSharpDevagram.Repository
{
	public interface IUsuarioRepository
	{
		public void Salvar(Usuario usuario);

		public bool VerificarEmail(string email);
	}
}
