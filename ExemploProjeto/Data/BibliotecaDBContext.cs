
using Microsoft.EntityFrameworkCore;
using ExemploProjeto.Models;



namespace ExemploProjeto.Data
{
    public class BibliotecaDBContext : DbContext
    {
        public BibliotecaDBContext(DbContextOptions<BibliotecaDBContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Editora> Editoras { get; set; }
        public DbSet<Livro> Livros { get; set; }
        public DbSet<Emprestimo> Emprestimos { get; set; }
        public DbSet<LivroEmprestimo> LivrosEmprestimos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Definir o nome correto das tabelas
            modelBuilder.Entity<Livro>().ToTable("tblivro");
            modelBuilder.Entity<Usuario>().ToTable("tbusuario");
            modelBuilder.Entity<Genero>().ToTable("tbgenero");
            modelBuilder.Entity<Autor>().ToTable("tbautor");
            modelBuilder.Entity<Editora>().ToTable("tbeditora");
            modelBuilder.Entity<Emprestimo>().ToTable("tbemprestimo");
            modelBuilder.Entity<LivroEmprestimo>().ToTable("tblivroemprestimo");

            // Definição das chaves primárias
            modelBuilder.Entity<Usuario>().HasKey(u => u.IdUsuario);
            modelBuilder.Entity<Genero>().HasKey(g => g.IdGenero);
            modelBuilder.Entity<Autor>().HasKey(a => a.IdAutor);
            modelBuilder.Entity<Editora>().HasKey(e => e.IdEditora);
            modelBuilder.Entity<Livro>().HasKey(l => l.IdLivro);
            modelBuilder.Entity<Emprestimo>().HasKey(emp => emp.IdEmprestimo);
            modelBuilder.Entity<LivroEmprestimo>().HasKey(le => le.IdLivroEmprestimo);

            // Relacionamento Livro -> Genero (1:N)
            modelBuilder.Entity<Livro>()
                .HasOne(l => l.Genero)
                .WithMany(g => g.Livros)
                .HasForeignKey(l => l.IdGenero)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento Livro -> Autor (1:N)
            modelBuilder.Entity<Livro>()
                .HasOne(l => l.Autor)
                .WithMany(a => a.Livros)
                .HasForeignKey(l => l.IdAutor)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento Livro -> Editora (1:N)
            modelBuilder.Entity<Livro>()
                .HasOne(l => l.Editora)
                .WithMany(e => e.Livros)
                .HasForeignKey(l => l.IdEditora)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento Emprestimo -> Usuario (1:N)
            modelBuilder.Entity<Emprestimo>()
                .HasOne(e => e.Usuario)
                .WithMany()
                .HasForeignKey(e => e.IdUsuario)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento LivroEmprestimo -> Livro (N:N através da tabela associativa)
            modelBuilder.Entity<LivroEmprestimo>()
                .HasOne(le => le.Livro)
                .WithMany()
                .HasForeignKey(le => le.IdLivro)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento LivroEmprestimo -> Emprestimo (N:N através da tabela associativa)
            modelBuilder.Entity<LivroEmprestimo>()
                .HasOne(le => le.Emprestimo)
                .WithMany(e => e.LivrosEmprestimos)
                .HasForeignKey(le => le.IdEmprestimo)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
