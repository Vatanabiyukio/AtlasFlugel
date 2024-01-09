using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace AtlasFlugel.Api.Entities
{
    /// <summary>
    /// Tabela de itens dos pedidos
    /// </summary>
    [Table("ItensPedido")]
    public partial class ItensPedido
    {
        /// <summary>
        /// Identificador do item no pedido
        /// </summary>
        [Key]
        public int Identity { get; set; }
        /// <summary>
        /// Identificador do pedido
        /// </summary>
        [Key]
        public int IdPedido { get; set; }
        /// <summary>
        /// Identificador do produto
        /// </summary>
        [Key]
        public int IdProduto { get; set; }
        /// <summary>
        /// Indicada quantidade do item no pedido
        /// </summary>
        public int Quantidade { get; set; }

        [ForeignKey("IdPedido")]
        [InverseProperty("ItensPedidos")]
        [JsonIgnore]
        public virtual Pedido IdPedidoNavigation { get; set; } = null!;
        [ForeignKey("IdProduto")]
        [InverseProperty("ItensPedidos")]
        [JsonIgnore]
        public virtual Produto IdProdutoNavigation { get; set; } = null!;
    }
}
