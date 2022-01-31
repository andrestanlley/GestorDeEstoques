using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace GestorDeEstoques
{
    internal class Program
    {
        enum Menu { Listar = 1, Adicionar, Remover, Entrada, Saída, Sair}

        static List<IEstoque> produtos = new List<IEstoque>();
        static void Main(string[] args)
        {
            Carregar();
            bool finalizar = false;
            while (!finalizar)
            {
                Console.WriteLine("Sistema de estoque");
                Console.WriteLine("\n1 - Listar\n2 - Adicionar\n3 - Remover\n4 - Entrada\n5 - Saída\n6 - Sair");
                Console.Write("\nSelecione: ");
                int opcaoSelecionada = int.Parse(Console.ReadLine());
                Menu opt = (Menu)opcaoSelecionada;



                switch (opt)
                {
                    case Menu.Listar:
                        Listagem();
                        break;
                    case Menu.Adicionar:
                        Cadastro();
                        break;
                    case Menu.Remover:
                        Remover();
                        break;
                    case Menu.Entrada:
                        Entrada();
                        break;
                    case Menu.Saída:
                        Saida();
                        break;
                    case Menu.Sair:
                        finalizar = true;
                        break;
                    default:
                        finalizar = true;
                        break;
                }
                Console.Clear();
            }
        }

        static void Listagem()
        {
            Console.Clear();
            if(produtos.Count > 0)
            {
                Console.WriteLine("Lista de produtos");
                int id = 0;
                foreach (IEstoque produto in produtos)
                {
                    Console.WriteLine("=================\n");
                    Console.WriteLine($"ID: {id}");
                    produto.Exibir();
                    id++;
                }
            }
            else
            {
                Console.WriteLine("Nenhum produto cadastrado.\n");
            }
            Console.WriteLine("Enter para retornar");
            Console.ReadLine();
        }

        static void Cadastro()
        {
            Console.WriteLine("\nCadastro de produto:");
            Console.WriteLine("1 - Produto Físico\n2 - Ebook\n3 - Curso");
            Console.Write("Selecione: ");
            int escolhaInt = int.Parse(Console.ReadLine());
            Console.Clear();

            switch (escolhaInt)
            {
                case 1:
                    CadastrarPFisico();
                    break;
                case 2:
                    CadastrarEbook();
                    break;
                case 3:
                    CadastrarCurso();
                    break;
                default:
                    Console.WriteLine("Opção invalida, Enter para voltar ao Menu.");
                    Console.ReadLine();
                    break;
            }
        }
        static void CadastrarPFisico()
        {
            Console.WriteLine("Cadastrando produto físico");
            Console.Write("Nome: ");
            string nome = Console.ReadLine();
            Console.Write("Preço: ");
            float preco = float.Parse(Console.ReadLine());
            Console.Write("Frete: ");
            float frete = float.Parse(Console.ReadLine());

            ProdutoFisico pf = new ProdutoFisico(nome, preco, frete);
            produtos.Add(pf);
            Salvar();
        }

        static void CadastrarEbook()
        {
            Console.WriteLine("Cadastrando ebook");
            Console.Write("Nome: ");
            string nome = Console.ReadLine();
            Console.Write("Preço: ");
            float preco = float.Parse(Console.ReadLine());
            Console.Write("Autor: ");
            string autor = Console.ReadLine();

            Ebook eb = new Ebook(nome, preco, autor);
            produtos.Add(eb);
            Salvar();
        }

        static void CadastrarCurso()
        {
            Console.WriteLine("Cadastrando curso");
            Console.Write("Nome: ");
            string nome = Console.ReadLine();
            Console.Write("Preço: ");
            float preco = float.Parse(Console.ReadLine());
            Console.Write("Autor: ");
            string autor = Console.ReadLine();

            Curso cs = new Curso(nome, preco, autor);
            produtos.Add(cs);
            Salvar();
        }

        static void Remover()
        {
            Listagem();
            Console.WriteLine("Qual id deseja remover?");
            int id = int.Parse(Console.ReadLine());
            if(id >= 0 && id < produtos.Count)
            {
                produtos.RemoveAt(id);
                Salvar();
            }
            else
            {
                Console.WriteLine("Você informou um ID invalido.");
            }
        }

        static void Entrada()
        {
            Listagem();
            Console.Write("Digite o ID do elemento para adicionar entrada: ");
            int id = int.Parse(Console.ReadLine());
            if(id >= 0 && id < produtos.Count)
            {
                produtos[id].AdicionarEntrada();
                Salvar();
            }
        }

        static void Saida()
        {
            Listagem();
            Console.Write("Digite o ID do elemento para adicionar saída: ");
            int id = int.Parse(Console.ReadLine());
            if (id >= 0 && id < produtos.Count)
            {
                produtos[id].AdicionarSaida();
                Salvar();
            }
        }

        static void Salvar()
        {
            FileStream stream = new FileStream("products.dat",FileMode.OpenOrCreate);
            BinaryFormatter encoder = new BinaryFormatter();

            encoder.Serialize(stream, produtos);
            stream.Close();
        }

        static void Carregar()
        {
            FileStream stream = new FileStream("products.dat", FileMode.OpenOrCreate);
            BinaryFormatter encoder = new BinaryFormatter();

            try
            {
                produtos = (List<IEstoque>)encoder.Deserialize(stream);
                if(produtos == null)
                {
                    produtos = new List<IEstoque>();
                }
            }
            catch (Exception e)
            {
                produtos = new List<IEstoque>();
            }

            stream.Close();
            
        }
    }
}
