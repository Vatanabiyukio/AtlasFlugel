using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AtlasFlugel.Api.Context;
using AtlasFlugel.Api.Entities;
using AtlasFlugel.Api.Models;

namespace AtlasFlugel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly StefaniniContext _context;
        private readonly ILogger<PedidoController> _logger;

        public PedidoController(StefaniniContext context, ILogger<PedidoController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtém a lista de pedidos com detalhes.
        /// </summary>
        /// <returns>Lista de pedidos com informações detalhadas.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PedidoDTO>>> GetPedidos()
        {
            try
            {
                var pedidosCompletos = await _context.Pedidos
                    .Include(pedido => pedido.ItensPedidos)
                    .ThenInclude(itensPedido => itensPedido.IdProdutoNavigation)
                    .ToListAsync();

                if (!pedidosCompletos.Any())
                {
                    return NotFound();
                }

                var pedidosDtos = pedidosCompletos.Select(pedido => new PedidoDTO
                {
                    Id = pedido.Identity,
                    NomeCliente = pedido.NomeCliente,
                    EmailCliente = pedido.EmailCliente,
                    Pago = pedido.Pago,
                    ValorTotal = pedido.ItensPedidos.Sum(itensPedido =>
                        itensPedido.IdProdutoNavigation.Valor * itensPedido.Quantidade),
                    ItensPedido = pedido.ItensPedidos.Select(itensPedido => new ItensPedidoDTO
                    {
                        Id = itensPedido.Identity,
                        Quantidade = itensPedido.Quantidade,
                        IdProduto = itensPedido.IdProduto,
                        NomeProduto = itensPedido.IdProdutoNavigation.NomeProduto,
                        ValorUnitario = itensPedido.IdProdutoNavigation.Valor
                    }).ToList()
                }).ToList();

                _logger.LogInformation("Obtida a lista de pedidos com detalhes.");
                return pedidosDtos;
            }
            catch (Exception)
            {
                _logger.LogError("Não foi possível obter a lista de pedidos com detalhes.");
                return Problem();
            }
        }

        /// <summary>
        /// Obtém detalhes de um pedido pelo ID.
        /// </summary>
        /// <param name="id">ID do pedido.</param>
        /// <returns>Detalhes do pedido.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoDTO>> GetPedido(int id)
        {
            try
            {
                var pedido = await _context.Pedidos
                    .Include(p => p.ItensPedidos)
                    .ThenInclude(itensPedido => itensPedido.IdProdutoNavigation)
                    .FirstOrDefaultAsync(p => p.Identity == id);

                if (pedido == null)
                {
                    _logger.LogInformation($"Pedido com ID {id} não encontrado.");
                    return NotFound();
                }

                var pedidoDto = new PedidoDTO
                {
                    Id = pedido.Identity,
                    NomeCliente = pedido.NomeCliente,
                    EmailCliente = pedido.EmailCliente,
                    Pago = pedido.Pago,
                    ValorTotal = pedido.ItensPedidos.Sum(itensPedido => itensPedido.IdProdutoNavigation.Valor * itensPedido.Quantidade),
                    ItensPedido = pedido.ItensPedidos.Select(itensPedido => new ItensPedidoDTO
                    {
                        Id = itensPedido.Identity,
                        IdProduto = itensPedido.IdProduto,
                        NomeProduto = itensPedido.IdProdutoNavigation.NomeProduto,
                        ValorUnitario = itensPedido.IdProdutoNavigation.Valor,
                        Quantidade = itensPedido.Quantidade
                    }).ToList()
                };

                _logger.LogInformation($"Obtido detalhes para o pedido com ID {id}.");
                return pedidoDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao obter detalhes do pedido com ID {id}. Erro: {ex.Message}");
                return Problem();
            }
        }


        /// <summary>
        /// Atualiza os detalhes de um pedido existente.
        /// </summary>
        /// <param name="id">ID do pedido a ser atualizado.</param>
        /// <param name="pedido">Dados atualizados do pedido.</param>
        /// <returns>Código de status indicando o resultado da operação.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPedido(int id, Pedido pedido)
        {
            try
            {
                if (id != pedido.Identity)
                {
                    _logger.LogWarning($"ID {id} no caminho não corresponde ao ID {pedido.Identity} no corpo da requisição.");
                    return BadRequest();
                }

                _context.Entry(pedido).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Pedido com ID {id} atualizado com sucesso.");
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PedidoExists(id))
                {
                    _logger.LogWarning($"Pedido com ID {id} não encontrado.");
                    return NotFound();
                }

                _logger.LogError($"Erro de concorrência ao tentar atualizar o pedido com ID {id}.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao tentar atualizar o pedido com ID {id}. Erro: {ex.Message}");
                return Problem();
            }
        }

        /// <summary>
        /// Cria um novo pedido.
        /// </summary>
        /// <param name="pedido">Dados do novo pedido.</param>
        /// <returns>O novo pedido criado.</returns>
        [HttpPost]
        public async Task<ActionResult<Pedido>> PostPedido(Pedido pedido)
        {
            try
            {
                if (!_context.Pedidos.Any())
                {
                    _logger.LogError("Entity set 'StefaniniContext.Pedidos' é nulo.");
                    return Problem("Entity set 'StefaniniContext.Pedidos' é nulo.");
                }

                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Novo pedido criado com ID {pedido.Identity}.");
                return CreatedAtAction("GetPedido", new { id = pedido.Identity }, pedido);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao tentar criar um novo pedido. Erro: {ex.Message}");
                return Problem();
            }
        }

        /// <summary>
        /// Exclui um pedido existente.
        /// </summary>
        /// <param name="id">ID do pedido a ser excluído.</param>
        /// <returns>Código de status indicando o resultado da operação.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePedido(int id)
        {
            try
            {
                if (!_context.Pedidos.Any())
                {
                    _logger.LogWarning("Entity set 'StefaniniContext.Pedidos' é nulo.");
                    return NotFound();
                }

                var pedido = await _context.Pedidos.FindAsync(id);
                if (pedido == null)
                {
                    _logger.LogInformation($"Pedido com ID {id} não encontrado.");
                    return NotFound();
                }

                _context.Pedidos.Remove(pedido);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Pedido com ID {id} excluído com sucesso.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao tentar excluir o pedido com ID {id}. Erro: {ex.Message}");
                return Problem();
            }
        }

        /// <summary>
        /// Verifica se um pedido com o ID fornecido existe.
        /// </summary>
        /// <param name="id">ID do pedido a ser verificado.</param>
        /// <returns>True se o pedido existir; False, caso contrário.</returns>
        private bool PedidoExists(int id)
        {
            try
            {
                if (_context.Pedidos.Any()) return _context.Pedidos.Any(e => e.Identity == id);
                _logger.LogWarning("Entity set 'StefaniniContext.Pedidos' é nulo.");
                return false;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao verificar a existência do pedido com ID {id}. Erro: {ex.Message}");
                return false;
            }        }
    }
}
