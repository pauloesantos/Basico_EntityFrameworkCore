using System;
using Microsoft.EntityFrameworkCore;
using Curso.Data;
using System.Linq;
using Curso.Domain;
using Curso.ValueObjects;
using System.Collections.Generic;

namespace CursoEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            using var db = new ApplicationContext();

            //db.Database.Migrate();
            var existe = db.Database.GetPendingMigrations().Any();
            if (existe)
            {
                //executa algo caso exista migração pendente
            }

            //Console.WriteLine("Hello World!");

            //InserirDados();
            //InserirDadosMassa();
            //ConsultaDados();
            //CadastrarPedidos();
            //ConsultaPedidoCarregamentoAdiantado();
            //AtualizaDados();
            RemoveRegistro();
        }
        private static void RemoveRegistro()
        {
            using var db = new ApplicationContext();

            //var cliente = db.Clientes.Find(4);

            // desconectado
            var cliente = new Cliente { Id = 3 };

            //db.Clientes.Remove(cliente);
            db.Remove(cliente);
            //db.Entry(cliente).State = EntityState.Deleted;

            db.SaveChanges();

        }
        private static void AtualizaDados()
        {
            using var db = new ApplicationContext();

            var cliente = db.Clientes.Find(4);
            //cliente.Nome = "Precilina Bussi";
            //atualiza todos o campos da tabela            
            //db.Clientes.Update(cliente);
            //informa de EF de forma explicita alterar todos os campos 
            //db.Entry(cliente).State = EntityState.Modified;

            //metodo desconectado
            var clienteDeconectado = new
            {
                Nome = "Desconectado",
                Telefone = "190605941"
            };
            db.Entry(cliente).CurrentValues.SetValues(clienteDeconectado);

            //somente o savechanges monitora e atualiza só o campo alterado
            db.SaveChanges();


        }
        private static void ConsultaPedidoCarregamentoAdiantado()
        {
            using var db = new ApplicationContext();

            //var pedidos = db.Pedidos.Include("Itens").ToList();
            var pedidos = db
                .Pedidos
                .Include(p => p.Itens)
                    .ThenInclude(p => p.Produto)
                .ToList();

            Console.WriteLine(pedidos.Count);
        }
        private static void CadastrarPedidos()
        {
            using var db = new ApplicationContext();

            var cliente = db.Clientes.FirstOrDefault();
            var produto = db.Produtos.FirstOrDefault();

            var pedido = new Pedido
            {
                ClienteId = cliente.Id,
                IniciandoEm = DateTime.Now,
                FinalizadoEm = DateTime.Now,
                Observacao = "Pedido de Teste",
                Status = StatusPedido.Analise,
                TipoFrete = TipoFrete.SemFrete,
                Itens = new List<PedidoItem>
                {
                    new PedidoItem
                    {
                        ProdutoId = produto.Id,
                        Desconto = 0,
                        Quantidade = 1,
                        Valor = 10,
                    }
                }
            };
            db.Pedidos.Add(pedido);

            db.SaveChanges();
        }


        private static void ConsultaDados()
        {
            using var db = new ApplicationContext();
            //var consultaPorSintaxe = (from c in db.Clientes where c.Id > 0 select c).ToList();
            var consultaPorMetodo = db.Clientes
                .Where(p => p.Id > 0)
                .OrderBy(p => p.Id)
                .ToList();
            foreach (var cliente in consultaPorMetodo)
            {
                Console.WriteLine($"Consultando Cliente:{cliente.Id}");
                //db.Clientes.Find(cliente.Id);
                // db.Clientes.FirstOrDefault(p => p.Id = cliente.Id);
            }
        }
        private static void InserirDadosMassa()
        {
            var produto = new Produto
            {
                Descricao = "Teste",
                CodigoBarras = "1234567891234",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };
            var cliente = new Cliente
            {
                Nome = "Paulo",
                CEP = "12092000",
                Cidade = "Taubaté",
                Estado = "SP",
                Telefone = "001580678"
            };

            using var db = new ApplicationContext();
            db.AddRange(produto, cliente);

            var registros = db.SaveChanges();
            Console.WriteLine($"Total registro(s) = {registros}");

        }

        private static void InserirDados()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "1234567891234",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            using var db = new ApplicationContext();
            //db.Produtos.Add(produto);
            //db.Set<Produto>().Add(produto);
            //db.Entry(produto).State = EntityState.Added;
            db.Add(produto);

            var registros = db.SaveChanges();
            Console.WriteLine($"Total registro(s) = {registros}");

        }
    }
}
