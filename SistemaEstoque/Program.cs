using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;

namespace SistemaEstoque
{
    class Program
    {
        // Nome do arquivo para persistência
        private static string nomeArquivo = "estoque.txt";

        static void Main(string[] args)
        {
            // Configura cultura para formato brasileiro
            CultureInfo.CurrentCulture = new CultureInfo("pt-BR");

            Console.WriteLine("📦 === SISTEMA DE GERENCIAMENTO DE ESTOQUE === 📦\n");

            // Menu principal
            while (true)
            {
                ExibirMenu();
                string opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        InserirProduto();
                        break;
                    case "2":
                        ListarProdutos();
                        break;
                    case "3":
                        LimparArquivo();
                        break;
                    case "4":
                        Console.WriteLine("\n✅ Sistema encerrado!");
                        return;
                    default:
                        Console.WriteLine("\n❌ Opção inválida! Tente novamente.\n");
                        break;
                }

                Console.WriteLine("\nPressione qualquer tecla para continuar...");
                Console.ReadKey();
                Console.Clear();
            }
        }
      
        // Exibe o menu de opções
       
        static void ExibirMenu()
        {
            Console.WriteLine("=== MENU DE OPÇÕES ===");
            Console.WriteLine("1 - Inserir Produto");
            Console.WriteLine("2 - Listar Produtos");
            Console.WriteLine("3 - Limpar Arquivo");
            Console.WriteLine("4 - Sair");
            Console.Write("\nEscolha uma opção: ");
        }

        // Insere um novo produto no sistema
      
        static void InserirProduto()
        {
            Console.WriteLine("\n=== INSERIR PRODUTO ===");

            // Verifica quantos produtos já existem no arquivo
            List<Produto> produtosExistentes = LerProdutosDoArquivo();
            if (produtosExistentes.Count >= 5)
            {
                Console.WriteLine("❌ Limite de produtos atingido!");
                return;
            }

            try
            {
                // Solicita dados do produto
                Console.Write("Nome do produto: ");
                string nome = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(nome))
                {
                    Console.WriteLine("❌ Nome do produto não pode estar vazio!");
                    return;
                }

                Console.Write("Quantidade em estoque: ");
                if (!int.TryParse(Console.ReadLine(), out int quantidade))
                {
                    Console.WriteLine("❌ Quantidade inválida!");
                    return;
                }

                if (quantidade < 0)
                {
                    Console.WriteLine("❌ Quantidade não pode ser negativa!");
                    return;
                }

                Console.Write("Preço unitário (R$): ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal preco))
                {
                    Console.WriteLine("❌ Preço inválido!");
                    return;
                }

                if (preco < 0)
                {
                    Console.WriteLine("❌ Preço não pode ser negativo!");
                    return;
                }

                // Cria o produto
                Produto novoProduto = new Produto(nome, quantidade, preco);

                // Salva no arquivo
                if (SalvarProdutoNoArquivo(novoProduto))
                {
                    Console.WriteLine("\n✅ Produto inserido com sucesso!");
                }
                else
                {
                    Console.WriteLine("\n❌ Erro ao salvar produto!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro inesperado: {ex.Message}");
            }
        }
        /// Lista todos os produtos cadastrados
    
        static void ListarProdutos()
        {
            Console.WriteLine("\n=== PRODUTOS CADASTRADOS ===");

            // Lê produtos do arquivo
            List<Produto> produtos = LerProdutosDoArquivo();

            if (produtos.Count == 0)
            {
                Console.WriteLine("Nenhum produto cadastrado.");
                return;
            }

            // Exibe todos os produtos
            for (int i = 0; i < produtos.Count; i++)
            {
                Console.WriteLine(produtos[i].ToString());
            }

            Console.WriteLine($"\nTotal de produtos: {produtos.Count}");
        }
        // Limpa o arquivo de estoque
     
        static void LimparArquivo()
        {
            Console.WriteLine("\n=== LIMPAR ARQUIVO ===");
            Console.Write("Tem certeza que deseja limpar todos os produtos? (S/N): ");
            string resposta = Console.ReadLine().ToUpper();

            if (resposta == "S" || resposta == "SIM")
            {
                try
                {
                    if (File.Exists(nomeArquivo))
                    {
                        File.Delete(nomeArquivo);
                        Console.WriteLine("✅ Arquivo limpo com sucesso!");
                    }
                    else
                    {
                        Console.WriteLine("ℹ️ Arquivo já está vazio ou não existe.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Erro ao limpar arquivo: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("❌ Operação cancelada.");
            }
        }
        /// Salva um produto no arquivo
     
        static bool SalvarProdutoNoArquivo(Produto produto)
        {
            try
            {
                // Usa AppendText para não sobrescrever o arquivo
                using (StreamWriter writer = File.AppendText(nomeArquivo))
                {
                    writer.WriteLine(produto.ToFileFormat());
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao salvar no arquivo: {ex.Message}");
                return false;
            }
        }

        /// Lê todos os produtos do arquivo
     
        static List<Produto> LerProdutosDoArquivo()
        {
            List<Produto> produtos = new List<Produto>();

            try
            {
                // Verifica se o arquivo existe
                if (!File.Exists(nomeArquivo))
                {
                    return produtos;
                }

                // Lê todas as linhas do arquivo
                string[] linhas = File.ReadAllLines(nomeArquivo);

                foreach (string linha in linhas)
                {
                    if (!string.IsNullOrWhiteSpace(linha))
                    {
                        Produto produto = Produto.FromFileFormat(linha);
                        if (produto != null)
                        {
                            produtos.Add(produto);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao ler arquivo: {ex.Message}");
            }

            return produtos;
        }
    }
}