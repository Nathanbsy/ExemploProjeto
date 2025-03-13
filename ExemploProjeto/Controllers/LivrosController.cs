using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExemploProjeto.Data;
using ExemploProjeto.Models;

namespace ExemploProjeto.Controllers
{
    public class LivrosController : Controller
    {
        private readonly BibliotecaDBContext _context;

        public LivrosController(BibliotecaDBContext context)
        {
            _context = context;
        }

        // GET: Livros
        public async Task<IActionResult> Index()
        {
            var livros = await _context.Livros
                .Include(l => l.Autor)
                .Include(l => l.Editora)
                .Include(l => l.Genero)
                .ToListAsync();

            return View(livros);
        }

        // GET: Livros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _context.Livros
                .Include(l => l.Autor)
                .Include(l => l.Editora)
                .Include(l => l.Genero)
                .FirstOrDefaultAsync(m => m.IdLivro == id);
            if (livro == null)
            {
                return NotFound();
            }

            return View(livro);
        }

        // GET: Livros/Create
        public IActionResult Create()
        {
            ViewData["IdAutor"] = new SelectList(_context.Autores, "IdAutor", "IdAutor");
            ViewData["IdEditora"] = new SelectList(_context.Editoras, "IdEditora", "IdEditora");
            ViewData["IdGenero"] = new SelectList(_context.Generos, "IdGenero", "IdGenero");
            return View();
        }

        // POST: Livros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdLivro,NomeLivro,IdGenero,IdAutor,IdEditora,EdicaoLivro,StatusLivro")] Livro livro)
        {
            
                _context.Add(livro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));


            ViewData["IdAutor"] = new SelectList(_context.Autores, "IdAutor", "IdAutor", livro.IdAutor);
            ViewData["IdEditora"] = new SelectList(_context.Editoras, "IdEditora", "IdEditora", livro.IdEditora);
            ViewData["IdGenero"] = new SelectList(_context.Generos, "IdGenero", "IdGenero", livro.IdGenero);
            
        }

        // GET: Livros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _context.Livros.FindAsync(id);
            if (livro == null)
            {
                return NotFound();
            }
            ViewData["IdAutor"] = new SelectList(_context.Autores, "IdAutor", "NomeAutor", livro.IdAutor);
            ViewData["IdEditora"] = new SelectList(_context.Editoras, "IdEditora", "NomeEditora", livro.IdEditora);
            ViewData["IdGenero"] = new SelectList(_context.Generos, "IdGenero", "NomeGenero", livro.IdGenero);
            return View(livro);
        }

        // POST: Livros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // Método POST: Atualiza os dados de um livro existente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdLivro,NomeLivro,IdGenero,IdAutor,IdEditora,EdicaoLivro,StatusLivro")] Livro livro)
        {
            if (id != livro.IdLivro) // Se o ID do livro não corresponder ao ID fornecido, retorna erro 404
            {
                return NotFound();
            }

            try
            {
                _context.Update(livro); // Atualiza o livro no contexto
                await _context.SaveChangesAsync(); // Salva as alterações no banco de dados
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LivroExists(livro.IdLivro)) // Se o livro não existir mais no banco, retorna erro 404
                {
                    return NotFound();
                }
                else
                {
                    throw; // Caso contrário, lança a exceção
                }
            }
            return RedirectToAction(nameof(Index)); // Redireciona para a lista de livros
        }

        // GET: Livros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _context.Livros
                .Include(l => l.Autor)
                .Include(l => l.Editora)
                .Include(l => l.Genero)
                .FirstOrDefaultAsync(m => m.IdLivro == id);
            if (livro == null)
            {
                return NotFound();
            }

            return View(livro);
        }

        // POST: Livros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var livro = await _context.Livros.FindAsync(id);
            if (livro != null)
            {
                _context.Livros.Remove(livro);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LivroExists(int id)
        {
            return _context.Livros.Any(e => e.IdLivro == id);
        }
    }
}
