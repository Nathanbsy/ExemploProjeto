namespace ExemploProjeto.Models
{
    public class Emprestimo
    {
        public int IdEmprestimo { get; set; }

        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }

        public DateTime DataEmprestimo { get; set; }
        public DateTime DataDevolucao { get; set; }

        public string StatusEmprestimo { get; set; }
        public ICollection<LivroEmprestimo> LivrosEmprestimos { get; set; }
    }
}
