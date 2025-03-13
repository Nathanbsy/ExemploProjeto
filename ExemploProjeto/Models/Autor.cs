namespace ExemploProjeto.Models
{
    public class Autor
    {
        public int IdAutor { get; set; }
        public string NomeAutor { get; set; }
        public IEnumerable<Livro> Livros { get; set; }
    }
}
