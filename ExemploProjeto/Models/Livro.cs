namespace ExemploProjeto.Models
{
    public class Livro
    {
        public int IdLivro { get; set; }
        public string NomeLivro { get; set; }

        public int IdGenero { get; set; }
        public Genero Genero { get; set; }

        public int IdAutor { get; set; }
        public Autor Autor { get; set; }

        public int IdEditora { get; set; }
        public Editora Editora { get; set; }

        public string EdicaoLivro { get; set; }
        public string StatusLivro { get; set; }
    }
}
