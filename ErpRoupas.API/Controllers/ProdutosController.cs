using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ErpRoupas.API.Data;
using ErpRoupas.API.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ErpRoupas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProdutosController(AppDbContext context) => _context = context;

        public class CadastroProdutoDto {
            public string Nome { get; set; } = string.Empty;
            public string CodigoBarras { get; set; } = string.Empty;
            public decimal Preco { get; set; }
            public int EstoqueInicial { get; set; }
            public List<string> Cores { get; set; } = new();
            public List<string> Tamanhos { get; set; } = new();
        }

        [HttpPost]
public async Task<IActionResult> Cadastrar([FromBody] CadastroProdutoDto dados) {
    var p = new Produto { Nome = dados.Nome };
    // Usamos o EAN/CodigoBarras vindo do front-end
    string baseSku = string.IsNullOrWhiteSpace(dados.CodigoBarras) ? dados.Nome.ToUpper() : dados.CodigoBarras;
    
    foreach (var cor in dados.Cores) {
        foreach (var tam in dados.Tamanhos) {
            p.Variacoes.Add(new VariacaoProduto {
                CodigoBarras = dados.CodigoBarras, // <--- Esta é a coluna que estava dando erro
                Cor = cor, Tamanho = tam, Preco = dados.Preco,
                Estoque = dados.EstoqueInicial,
                Sku = $"{baseSku}-{cor}-{tam}".ToUpper()
            });
        }
    }
    _context.Produtos.Add(p);
    await _context.SaveChangesAsync();
    return Ok();
}

        [HttpGet]
        public async Task<IActionResult> Listar() => Ok(await _context.Produtos.Include(p => p.Variacoes).ToListAsync());

        [HttpPost("baixa-estoque")]
        public async Task<IActionResult> DarBaixa([FromBody] dynamic dados) {
            string sku = dados.GetProperty("sku").GetString();
            int qtd = dados.GetProperty("quantidade").GetInt32();
            
            var v = await _context.VariacoesProdutos.FirstOrDefaultAsync(x => x.Sku == sku.ToUpper() || x.CodigoBarras == sku);
            if (v == null || v.Estoque < qtd) return BadRequest(new { mensagem = "SKU não encontrado ou estoque insuficiente!" });
            
            v.Estoque -= qtd;
            await _context.SaveChangesAsync();
            return Ok(new { mensagem = "Venda realizada!" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarProduto(int id) {
            var p = await _context.Produtos.FindAsync(id);
            if(p != null) { _context.Produtos.Remove(p); await _context.SaveChangesAsync(); }
            return Ok();
        }

        [HttpDelete("variacao/{id}")]
        public async Task<IActionResult> DeletarVariacao(int id) {
            var v = await _context.VariacoesProdutos.FindAsync(id);
            if(v != null) { _context.VariacoesProdutos.Remove(v); await _context.SaveChangesAsync(); }
            return Ok();
        }
    }
}