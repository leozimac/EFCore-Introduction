using System;
using System.Collections.Generic;
using System.Linq;
using CursoEFCore.Domain;
using CursoEFCore.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CursoEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            //InserirDados();
            //InserirDadosEmMassa();
            //ConsultarDados();
            //CadastrarPedido();
            //ConsultarPedidoCarregamentoAdiantado();
            //AtualizarEntidadeCompletamente();
            //AtualizarEntidadeDesconectada();
            //AtualizarEntidadeCompletamenteDesconectada();
            //RemoverRegistro();
        }

        private static void RemoverRegistro()
        {
            using var db = new Data.ApplicationContext();
            var cliente = db.Clientes.Find(2);

            db.Clientes.Remove(cliente);
            //db.Remove(cliente);
            //db.Entry(cliente).State = EntityState.Deleted;

            db.SaveChanges();
        }

        private static void AtualizarEntidadeCompletamenteDesconectada()
        {
            using var db = new Data.ApplicationContext();

            var cliente = new Cliente
            {
                Id = 1
            };

            var clienteDesconectado = new
            {
                Nome = "Cliente totalmente desconectado",
                Telefone = "7966669999"
            };

            db.Attach(cliente);
            db.Entry(cliente).CurrentValues.SetValues(clienteDesconectado);

            db.SaveChanges();
        }

        private static void AtualizarEntidadeDesconectada()
        {
            using var db = new Data.ApplicationContext();

            var cliente = db.Clientes.Find(1);
            var clienteDesconectado = new
            {
                Nome = "Cliente Desconectado",
                Telefone = "99100055518"
            };

            db.Entry(cliente).CurrentValues.SetValues(clienteDesconectado);

            db.SaveChanges();
        }

        private static void AtualizarEntidadeParcialmente()
        {
            using var db = new Data.ApplicationContext();

            var cliente = db.Clientes.Find(1);
            cliente.Nome = "Cliente Alterado 2";

            db.SaveChanges();
        }

        private static void AtualizarEntidadeCompletamente()
        {
            using var db = new Data.ApplicationContext();
            var cliente = db.Clientes.Find(1);
            cliente.Nome = "Cliente Alterado";

            db.Update(cliente);

            db.SaveChanges();
        }

        private static void ConsultarPedidoCarregamentoAdiantado()
        {
            using var db = new Data.ApplicationContext();
            var pedidos = db
                .Pedidos
                .Include(p => p.Itens)
                .ThenInclude(p => p.Produto)
                .ToList();

            Console.WriteLine(pedidos.Count);
        }

        private static void ConsultarDados()
        {
            using var db = new Data.ApplicationContext();

            //var consultaPorSintaxe = (from c in db.Clientes where c.Id >0 select c).ToList();
            var consultaPorMetodo = db.Clientes.Where(p => p.Id > 0).ToList();

            foreach (var cliente in consultaPorMetodo)
            {
                Console.WriteLine($"Consultando Cliente: {cliente.Id}");
                db.Clientes.Find(cliente.Id);
            }
        }

        private static void CadastrarPedido()
        {
            using var db = new Data.ApplicationContext();

            var cliente = db.Clientes.FirstOrDefault();
            var produto = db.Produtos.FirstOrDefault();

            var pedido = new Pedido()
            {
                ClienteId = cliente.Id,
                IniciadoEm = DateTime.Now,
                FinalizadoEm = DateTime.Now,
                Observacao = "Pedido Teste",
                Status = StatusPedido.Analise,
                TipoFrete = TipoFrete.SemFrete,
                Itens = new List<PedidoItem>
                {
                    new PedidoItem
                    {
                        ProdutoId = produto.Id,
                        Desconto = 0,
                        Quantidade = 1,
                        Valor = produto.Valor,
                    }
                }
            };

            db.Pedidos.Add(pedido);

            db.SaveChanges();
        }

        private static void InserirDadosEmMassa()
        {
            var cliente = new Cliente
            {
                Nome = "Leonardo",
                CEP = "99988812",
                Cidade = "Ribeirao Preto",
                Estado = "SP",
                Telefone = "99100055512"
            };

            var produto = new Produto()
            {
                Descricao = "Produto teste 2",
                CodigoBarras = "1234567891241",
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Valor = 20m,
                Ativo = true,
            };

            var listaClientes = new List<Cliente>
            {
                new Cliente
                {
                    Nome = "Ana",
                    CEP = "99988812",
                    Cidade = "Ribeirao Preto",
                    Estado = "SP",
                    Telefone = "99100055514"
                },
                new Cliente
                {
                    Nome = "Pinturinha",
                    CEP = "99988812",
                    Cidade = "Ribeirao Preto",
                    Estado = "SP",
                    Telefone = "99100055513"
                }
            };

            using var db = new Data.ApplicationContext();

            //db.AddRange(produto, cliente);

            db.Clientes.AddRange(listaClientes);

            var registros = db.SaveChanges();
            Console.WriteLine($"Total Registro(s): {registros}");
        }

        private static void InserirDados()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "1234567891231",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            using var db = new Data.ApplicationContext();

            db.Produtos.Add(produto);
            //db.Set<Produto>().Add(produto);
            //db.Entry(produto).State = EntityState.Added;
            //db.Add(produto);

            var registros = db.SaveChanges();

            Console.WriteLine($"Total de Registro(s): {registros}");
        }
    }
}
