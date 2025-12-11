using System;
using System.Globalization;

namespace SistemaEstoque
{   
    // Classe que representa um produto no estoque
    
    public class Produto
    {
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public decimal Preco { get; set; }
                
        // Construtor da classe Produto
       
        public Produto(string nome, int quantidade, decimal preco)
        {
            Nome = nome;
            Quantidade = quantidade;
            Preco = preco;
        }

        
       // Converte o produto para formato de arquivo (CSV)
        
        public string ToFileFormat()
        {
            // Usa ponto como separador decimal e InvariantCulture
            return $"{Nome},{Quantidade},{Preco.ToString(CultureInfo.InvariantCulture)}";
        }
                
        // Cria um produto a partir de uma linha do arquivo
      
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

        // Exibe as informações do produto formatadas
        
        public override string ToString()
        {
            return $"Produto: {Nome} | Quantidade: {Quantidade} | Preço: R$ {Preco:F2}";
        }
    }
}