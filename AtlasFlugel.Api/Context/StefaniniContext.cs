using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using AtlasFlugel.Api.Entities;

namespace AtlasFlugel.Api.Context
{
    public partial class StefaniniContext : DbContext
    {
        public StefaniniContext()
        {
        }

        public StefaniniContext(DbContextOptions<StefaniniContext> options)
            : base(options)
        {
            // Garante que o banco de dados seja criado se não existir
            Database.EnsureCreated();
        }

        public virtual DbSet<ItensPedido> ItensPedidos { get; set; } = null!;
        public virtual DbSet<Pedido> Pedidos { get; set; } = null!;
        public virtual DbSet<Produto> Produtos { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItensPedido>(entity =>
            {
                entity.HasKey(e => new { e.Identity, e.IdPedido, e.IdProduto })
                    .HasName("ItensPedido_pk")
                    .IsClustered(false);

                entity.HasComment("Tabela de itens dos pedidos");

                entity.HasIndex(e => new { e.Identity, e.IdPedido, e.IdProduto }, "ItensPedido_Identity_IdPedido_IdProduto_uindex")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.Identity)
                    .ValueGeneratedOnAdd()
                    .HasComment("Identificador do item no pedido");

                entity.Property(e => e.IdPedido).HasComment("Identificador do pedido");

                entity.Property(e => e.IdProduto).HasComment("Identificador do produto");

                entity.Property(e => e.Quantidade).HasComment("Indicada quantidade do item no pedido");

                entity.HasOne(d => d.IdPedidoNavigation)
                    .WithMany(p => p.ItensPedidos)
                    .HasForeignKey(d => d.IdPedido)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ItensPedido_Pedido_Identity_fk");

                entity.HasOne(d => d.IdProdutoNavigation)
                    .WithMany(p => p.ItensPedidos)
                    .HasForeignKey(d => d.IdProduto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ItensPedido_Produto_Identity_fk");
            });

            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.HasKey(e => e.Identity)
                    .HasName("Pedido_pk");

                entity.HasComment("Tabela de pedidos do desafio");

                entity.Property(e => e.Identity).HasComment("Identificador do pedido");

                entity.Property(e => e.DataCriacao)
                    .HasDefaultValueSql("(getutcdate())")
                    .HasComment("Data de criação do registro em horário UTC unificado");

                entity.Property(e => e.EmailCliente).HasComment("E-mail do cliente");

                entity.Property(e => e.NomeCliente).HasComment("Nome do cliente");

                entity.Property(e => e.Pago).HasComment("Indica se o pedido foi pago ou não");
            });

            modelBuilder.Entity<Produto>(entity =>
            {
                entity.HasKey(e => e.Identity)
                    .HasName("Produto_pk");

                entity.HasComment("Tabela de produtos do desafio");

                entity.Property(e => e.Identity).HasComment("Identificador do produto");

                entity.Property(e => e.NomeProduto).HasComment("Nome do produto");

                entity.Property(e => e.Valor)
                    .HasDefaultValueSql("((0.00))")
                    .HasComment("Valor do produto");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
