namespace ExemploProjeto.Models
{
    public class LivroEmprestimo
    {
        public int IdLivroEmprestimo { get; set; }

        public int IdLivro { get; set; }
        public Livro Livro { get; set; }  // Chave estrangeira

        public int IdEmprestimo { get; set; }
        public Emprestimo Emprestimo { get; set; }
    }
}
