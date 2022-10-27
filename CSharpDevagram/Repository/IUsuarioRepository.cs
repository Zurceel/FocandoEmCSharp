using CSharpDevagram.Models;

namespace CSharpDevagram.Repository
{
	public interface IUsuarioRepository
	{
		Usuario GetUsuarioPorId(int id);
		Usuario GetUsuarioPorLoginSenha(string email, string senha);
		public void Salvar(Usuario usuario);

		public bool VerificarEmail(string email);
	}
}
