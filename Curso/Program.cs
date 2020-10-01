using System;
using Microsoft.EntityFrameworkCore;
using Curso.Data;
using System.Linq;
using Curso.Domain;
using Curso.ValueObjects;

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
                //
            }

            Console.WriteLine("Hello World!");

            InserirDados();
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
