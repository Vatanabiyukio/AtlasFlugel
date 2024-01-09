using Newtonsoft.Json;

namespace AtlasFlugel.Api.Models;

public class ItensPedidoDTO
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("idProduto")]
    public int IdProduto { get; set; }
    
    [JsonProperty("nomeProduto")]
    public string NomeProduto { get; set; }
    
    [JsonProperty("valorUnitario")]
    public decimal ValorUnitario { get; set; }
    
    [JsonProperty("quantidade")]
    public int Quantidade { get; set; }
}