namespace ExemploProjeto.Models
{
    public class Editora
    {
        public int IdEditora { get; set; }
        public string NomeEditora { get; set; }
        public ICollection<Livro> Livros { get; set; }
    }
}
