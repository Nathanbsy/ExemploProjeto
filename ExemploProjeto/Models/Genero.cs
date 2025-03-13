namespace ExemploProjeto.Models
{
    public class Genero
    {
        public int IdGenero { get; set; }
        public string NomeGenero { get; set; }
        public ICollection<Livro> Livros { get; set; }
    }
}
