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
            private Dictionary<(long, int), long> cache = [];

            private int utilizouCacheNVEzes = 0;
            
            public void InformacoesDebug()
            {
                Console.Write("Entrada: ");
                Console.WriteLine(string.Join(" ", this.pedras));
                Console.WriteLine($"Cache utilizado: {this.utilizouCacheNVEzes}/{this.cache.Count} ({this.utilizouCacheNVEzes * 1f / this.cache.Count:P2})");
            }

            public Pedras(string input)
            {
                pedras = Parser.ListaDeInts(input);
            }

            private bool TratamentoDigitosPares(long entrada, out long numero1, out long numero2)
            {
                int digitos = 0;
                long temp = entrada;
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

            private long PiscaPedra(long valor, int profundidade)
            {
                long Cacheia(long solucao)
                {
                    this.cache.TryAdd((valor, profundidade), solucao);
                    return solucao;
                }

                if (profundidade == 0)
                {
                    return 1;
                }

                if (this.cache.TryGetValue((valor, profundidade), out long saidaCache))
                {
                    utilizouCacheNVEzes++;
                    return saidaCache;
                }

                if (valor == 0)
                {
                    return Cacheia(PiscaPedra(1, profundidade - 1));
                }


                if (TratamentoDigitosPares(valor, out long numero1, out long numero2))
                {
                    return 
                        Cacheia(
                            PiscaPedra(numero1, profundidade - 1) +
                            PiscaPedra(numero2, profundidade - 1)
                        );
                }

                return Cacheia(PiscaPedra(valor * 2024, profundidade - 1));
            }

            public long PiscaPedras(int vezes)
            {
                long count = 0;

                foreach(int pedra in this.pedras)
                {
                    count += PiscaPedra(pedra, vezes);
                }

                return count;
            }
        }

        public string SolucaoParte1(string input)
        {
            Pedras pedras = new(input);

            long saida = pedras.PiscaPedras(25);

            return saida.ToString();
        }

        public string SolucaoParte2(string input)
        {
            Pedras pedras = new(input);

            long saida = pedras.PiscaPedras(75);

            return saida.ToString();
        }
    }
}
