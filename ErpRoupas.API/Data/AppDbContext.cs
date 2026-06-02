using Microsoft.EntityFrameworkCore;
using ErpRoupas.API.Models;

namespace ErpRoupas.API.Data; // Sem chaves aqui, evita erros de fechamento!

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Produto> Produtos { get; set; }
    public DbSet<VariacaoProduto> VariacoesProdutos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<VariacaoProduto>()
            .Property(v => v.Preco)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<VariacaoProduto>()
            .HasOne(v => v.ProdutoPai)
            .WithMany(p => p.Variacoes)
            .HasForeignKey(v => v.ProdutoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}