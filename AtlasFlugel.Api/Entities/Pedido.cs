using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace AtlasFlugel.Api.Entities
{
    /// <summary>
    /// Tabela de pedidos do desafio
    /// </summary>
    [Table("Pedido")]
    public partial class Pedido
    {
        public Pedido()
        {
            ItensPedidos = new HashSet<ItensPedido>();
        }

        /// <summary>
        /// Identificador do pedido
        /// </summary>
        [Key]
        [DisplayName("id")]
        public int Identity { get; set; }
        /// <summary>
        /// Nome do cliente
        /// </summary>
        [StringLength(60)]
        [Unicode(false)]
        public string NomeCliente { get; set; } = null!;
        /// <summary>
        /// E-mail do cliente
        /// </summary>
        [StringLength(60)]
        [Unicode(false)]
        public string EmailCliente { get; set; } = null!;
        /// <summary>
        /// Data de criação do registro em horário UTC unificado
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime DataCriacao { get; set; }
        /// <summary>
        /// Indica se o pedido foi pago ou não
        /// </summary>
        public bool Pago { get; set; }
        
        [InverseProperty("IdPedidoNavigation")]
        [JsonIgnore]
        public virtual ICollection<ItensPedido> ItensPedidos { get; set; }
    }
}
