using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace AtlasFlugel.Api.Models;

public class PedidoDTO
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("nomeCliente")]
    public string NomeCliente { get; set; }

    [JsonProperty("emailCliente")]
    [EmailAddress]
    public string EmailCliente { get; set; }

    [JsonProperty("pago")]
    public bool Pago { get; set; }
    
    [JsonProperty("valorTotal")]
    public decimal ValorTotal { get; set; }

    [JsonProperty("itensPedido")]
    public IEnumerable<ItensPedidoDTO> ItensPedido { get; set; }
}