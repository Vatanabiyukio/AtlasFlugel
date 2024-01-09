using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace AtlasFlugel.Api.Entities
{
    /// <summary>
    /// Tabela de produtos do desafio
    /// </summary>
    [Table("Produto")]
    public partial class Produto
    {
        public Produto()
        {
            ItensPedidos = new HashSet<ItensPedido>();
        }

        /// <summary>
        /// Identificador do produto
        /// </summary>
        [Key]
        public int Identity { get; set; }
        /// <summary>
        /// Nome do produto
        /// </summary>
        [StringLength(20)]
        [Unicode(false)]
        public string NomeProduto { get; set; } = null!;
        /// <summary>
        /// Valor do produto
        /// </summary>
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Valor { get; set; }

        [InverseProperty("IdProdutoNavigation")]
        [JsonIgnore]
        public virtual ICollection<ItensPedido> ItensPedidos { get; set; }
    }
}
