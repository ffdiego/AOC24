using AOC24.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC24.Solucoes
{
    internal class Dia11 : ISolucionador
    {
        internal class Pedras
        {
            private List<int> pedras;
            private Dictionary<(int, int), int> solucoes;
            private int utilizouCacheNVEzes = 0;

            public Pedras(string input)
            {
                pedras = Parser.ListaDeInts(input);
                solucoes = [];
            }

            private bool TratamentoDigitosPares(int entrada, out int numero1, out int numero2)
            {
                int digitos = 0;
                int temp = entrada;
                while (temp > 0)
                {
                    temp /= 10;
                    digitos++;
                }

                numero1 = 0;
                numero2 = 0;

                if (digitos % 2 != 0)
                {
                    return false;
                }

                int divisor = (int)Math.Pow(10, digitos / 2);

                numero1 = entrada / divisor;
                numero2 = entrada % divisor;

                return true;
            }

            private int PiscaPedra(int valor, int profundidade)
            {
                int Cacheia(int solucao)
                {
                    this.solucoes.TryAdd((valor, profundidade), solucao);
                    return solucao;
                }

                if (profundidade == 0)
                {
                    return 1;
                }

                if (this.solucoes.TryGetValue((valor, profundidade), out int saidaCache))
                {
                    utilizouCacheNVEzes++;
                    return saidaCache;
                }

                if (valor == 0)
                {
                    return Cacheia(PiscaPedra(1, profundidade - 1));
                }


                if (TratamentoDigitosPares(valor, out int numero1, out int numero2))
                {
                    return 
                        Cacheia(
                            PiscaPedra(numero1, profundidade - 1) +
                            PiscaPedra(numero2, profundidade - 1)
                        );
                }

                return Cacheia(PiscaPedra(valor * 2024, profundidade - 1));
            }

            public int PiscaPedras(int vezes)
            {
                int count = 0;

                foreach(int pedra in this.pedras)
                {
                    count += PiscaPedra(pedra, vezes);
                }

                Console.Write("Entrada: ");
                Console.WriteLine(string.Join(" ", this.pedras));
                Console.WriteLine($"Cache utilizado: {this.utilizouCacheNVEzes}/{this.solucoes.Count} ({this.utilizouCacheNVEzes*1f/this.solucoes.Count:P2})");

                return count;
            }
        }

        public string SolucaoParte1(string input)
        {
            Pedras pedras = new(input);

            int saida = pedras.PiscaPedras(75);

            return saida.ToString();
        }

        public string SolucaoParte2(string input)
        {
            return ":D";
        }
    }
}
