using System;
using System.Collections.Generic;

namespace ErpRoupas.API.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        public List<VariacaoProduto> Variacoes { get; set; } = new List<VariacaoProduto>();
    }
}