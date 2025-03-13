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
    public class EmprestimosController : Controller
    {
        private readonly BibliotecaDBContext _context;

        public EmprestimosController(BibliotecaDBContext context)
        {
            _context = context;
        }

        // GET: Emprestimos
        public IActionResult Index()
        {
            var emprestimos = _context.Emprestimos
                    .Include(e => e.Usuario)
                    .Include(e => e.LivrosEmprestimos)
                    .ThenInclude(le => le.Livro)
                    .ToList();

            // Verifica se algum empréstimo está atrasado
            ViewBag.TemAtraso = emprestimos.Any(e => e.StatusEmprestimo == "Ativo" && e.DataDevolucao < DateTime.Now);

            return View(emprestimos);
        }

        // GET: Emprestimos/Details/5
        public IActionResult Details(int? id)
        {
            var emprestimo = _context.Emprestimos
                 .Include(e => e.Usuario)
                 .Include(e => e.LivrosEmprestimos)
                 .ThenInclude(le => le.Livro)
                 .ThenInclude(l => l.Autor) // Inclui Autor
                 .Include(e => e.LivrosEmprestimos)
                 .ThenInclude(le => le.Livro)
                 .ThenInclude(l => l.Editora) // Inclui Editora
                 .FirstOrDefault(e => e.IdEmprestimo == id);

            if (emprestimo == null)
            {
                return NotFound();
            }

            return View(emprestimo);
        }

        // GET: Emprestimos/Create
        public IActionResult Create()
        {
            ViewBag.Usuarios = _context.Usuarios.ToList();
            ViewBag.Livros = _context.Livros.Where(l => l.StatusLivro == "Disponível").ToList();
            return View();
        }

        // POST: Emprestimos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int idUsuario, List<int> livrosSelecionados)
        {
            if (idUsuario == 0 || livrosSelecionados == null || livrosSelecionados.Count == 0)
            {
                ModelState.AddModelError("", "Usuário e pelo menos um livro devem ser selecionados.");
                return RedirectToAction("Create");
            }

            var emprestimo = new Emprestimo
            {
                IdUsuario = idUsuario,
                DataEmprestimo = DateTime.Now,
                DataDevolucao = DateTime.Now.AddDays(7), // Adicionando 7 dias para devolução
                StatusEmprestimo = "Ativo"
            };

            _context.Emprestimos.Add(emprestimo);
            _context.SaveChanges();

            foreach (var livroId in livrosSelecionados)
            {
                _context.LivrosEmprestimos.Add(new LivroEmprestimo
                {
                    IdEmprestimo = emprestimo.IdEmprestimo,
                    IdLivro = livroId
                });

                var livro = _context.Livros.Find(livroId);
                livro.StatusLivro = "Emprestado";
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }



        // GET: Emprestimos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emprestimo = await _context.Emprestimos.FindAsync(id);
            if (emprestimo == null)
            {
                return NotFound();
            }
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", emprestimo.IdUsuario);
            return View(emprestimo);
        }

        // POST: Emprestimos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEmprestimo,IdUsuario,DataEmprestimo,DataDevolucao,StatusEmprestimo")] Emprestimo emprestimo)
        {
            //ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", emprestimo.IdUsuario);
            if (id != emprestimo.IdEmprestimo)
            {
                return NotFound();
            }

            
                try
                {
                    _context.Update(emprestimo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmprestimoExists(emprestimo.IdEmprestimo))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            
            
            //return View(emprestimo);
        }

        // GET: Emprestimos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emprestimo = await _context.Emprestimos
                .Include(e => e.Usuario)
                .FirstOrDefaultAsync(m => m.IdEmprestimo == id);
            if (emprestimo == null)
            {
                return NotFound();
            }

            return View(emprestimo);
        }

        // POST: Emprestimos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var emprestimo = await _context.Emprestimos.FindAsync(id);
            if (emprestimo != null)
            {
                _context.Emprestimos.Remove(emprestimo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmprestimoExists(int id)
        {
            return _context.Emprestimos.Any(e => e.IdEmprestimo == id);
        }
        public IActionResult Devolver(int id)
        {
            var emprestimo = _context.Emprestimos
                .Include(e => e.LivrosEmprestimos)
                .ThenInclude(le => le.Livro)
                .FirstOrDefault(e => e.IdEmprestimo == id);

            if (emprestimo == null)
            {
                return NotFound();
            }

            // Atualiza o status do empréstimo para "Finalizado"
            emprestimo.StatusEmprestimo = "Finalizado";

            // Atualiza o status dos livros para "Disponível"
            foreach (var livroEmprestimo in emprestimo.LivrosEmprestimos)
            {
                var livro = _context.Livros.Find(livroEmprestimo.IdLivro);
                if (livro != null)
                {
                    livro.StatusLivro = "Disponível";
                }
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
