using System;
using System.Globalization;

namespace SistemaEstoque
{
    /// <summary>
    /// Classe que representa um produto no estoque
    /// </summary>
    public class Produto
    {
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public decimal Preco { get; set; }

        /// <summary>
        /// Construtor da classe Produto
        /// </summary>
        /// <param name="nome">Nome do produto</param>
        /// <param name="quantidade">Quantidade em estoque</param>
        /// <param name="preco">Preço unitário</param>
        public Produto(string nome, int quantidade, decimal preco)
        {
            Nome = nome;
            Quantidade = quantidade;
            Preco = preco;
        }

        /// <summary>
        /// Converte o produto para formato de arquivo (CSV)
        /// </summary>
        /// <returns>String no formato CSV</returns>
        public string ToFileFormat()
        {
            // Usa ponto como separador decimal e InvariantCulture
            return $"{Nome},{Quantidade},{Preco.ToString(CultureInfo.InvariantCulture)}";
        }

        /// <summary>
        /// Cria um produto a partir de uma linha do arquivo
        /// </summary>
        /// <param name="linha">Linha do arquivo no formato CSV</param>
        /// <returns>Produto criado ou null se inválido</returns>
        public static Produto FromFileFormat(string linha)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(linha))
                {
                    return null;
                }

                string[] partes = linha.Split(',');

                if (partes.Length != 3)
                {
                    return null;
                }

                string nome = partes[0].Trim();

                if (!int.TryParse(partes[1].Trim(), out int quantidade))
                {
                    return null;
                }

                // Usa InvariantCulture para ler o preço (formato com ponto)
                if (!decimal.TryParse(partes[2].Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out decimal preco))
                {
                    return null;
                }

                return new Produto(nome, quantidade, preco);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Exibe as informações do produto formatadas
        /// </summary>
        /// <returns>String formatada do produto</returns>
        public override string ToString()
        {
            return $"Produto: {Nome} | Quantidade: {Quantidade} | Preço: R$ {Preco:F2}";
        }
    }
}