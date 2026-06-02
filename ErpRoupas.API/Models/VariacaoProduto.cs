using System;

namespace ErpRoupas.API.Models
{
    public class VariacaoProduto
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; } // Chave estrangeira ligando ao Pai
        
        public string Sku { get; set; } = string.Empty; 
        public string Tamanho { get; set; } = string.Empty; 
        public string Cor { get; set; } = string.Empty; 
        
        public int Estoque { get; set; } 
        public decimal Preco { get; set; } 

        // Mudamos o nome aqui para o C# não confundir a classe com a propriedade
        public Produto? ProdutoPai { get; set; }
        public string CodigoBarras { get; set; } = string.Empty;
    }
}