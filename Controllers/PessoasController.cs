using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgendaTelefonica.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgendaTelefonica.Controllers
{
    public class PessoasController : Controller
    {
        private readonly Contexto _contexto;

        public PessoasController(Contexto contexto)
        {
            _contexto = contexto;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _contexto.Pessoas.ToListAsync());
        }

        [HttpGet]
        public IActionResult CriarPessoa()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CriarPessoa(Pessoa pessoa)
        {
            if (ModelState.IsValid) {
                _contexto.Add(pessoa);
                await _contexto.SaveChangesAsync();
                TempData["ContatoNovo"] = $"Contato com o nome {pessoa.Nome} incluído com sucesso !";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(pessoa);
            }            
        }

        [HttpGet]
        public async Task<IActionResult> AtualizarPessoa(int pessoaId)
        {
            Pessoa pessoa = await _contexto.Pessoas.FindAsync(pessoaId);

            if (pessoa == null)
                return NotFound();
            return View(pessoa);
        }
        [HttpPost]
        public async Task<IActionResult> AtualizarPessoa(Pessoa pessoa)
        {
            if (ModelState.IsValid)
            {
                _contexto.Update(pessoa);
                await _contexto.SaveChangesAsync();
                TempData["ContatoAtualizado"] = $"Contato {pessoa.Nome} atualizado com sucesso !";
                return RedirectToAction(nameof(Index));
            }
            return View(pessoa);
        }

        [HttpPost]
        public async Task<JsonResult> ExcluirPessoa(int pessoaId)
        {
            Pessoa pessoa = await _contexto.Pessoas.FindAsync(pessoaId);
            _contexto.Pessoas.Remove(pessoa);
            await _contexto.SaveChangesAsync();
            TempData["ContatoExcluido"] = $"Contato {pessoa.Nome} excluído com sucesso !";
            return Json(true);
        }
    }
}
