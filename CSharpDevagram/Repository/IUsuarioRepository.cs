using CSharpDevagram.Models;

namespace CSharpDevagram.Repository
{
	public interface IUsuarioRepository
	{
		Usuario GetUsuarioPorLoginSenha(string email, string senha);
		public void Salvar(Usuario usuario);

		public bool VerificarEmail(string email);
	}
}
