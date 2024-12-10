using AOC24.Utils;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC24.Solucoes
{
    internal class Dia9 : ISolucionador
    {
        internal interface IBloco
        {
            int Tamanho { get; }
        };

        internal class Arquivo : IBloco
        {
            static private int id = 0;
            public int Id { get; private set; }
            public int Tamanho { get; }

            public Arquivo(int tamanho)
            {
                this.Id = Arquivo.id++;
                Tamanho = tamanho;
            }
        }

        internal class Espaco : IBloco
        {
            public int Tamanho { get; }

            public Espaco(int tamanho)
            {
                Tamanho = tamanho;
            }

        }

        internal class Disco
        {
            private List<IBloco> disco;
            private Stack<Arquivo> arquivos;

            private bool SwapBloco(int a, int b)
            {
                if (disco.ElementAtOrDefault(a) == null || disco.ElementAtOrDefault(b) == null)
                {
                    return false;
                }

                IBloco blocoA = disco[a];
                IBloco blocoB = disco[b];

                disco[a] = blocoB;
                disco[b] = blocoA;

                return true;
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder(disco.Count);

                for (int i = 0; i < disco.Count;)
                {
                    IBloco bloco = disco[i];
                    if (bloco is Arquivo arquivo)
                    {
                        sb.Append(arquivo.Id);
                    }
                    else
                    {
                        sb.Append('.');
                    }

                    i++;
                }

                return sb.ToString();
            }

            public void Fragmenta()
            {
                while (arquivos.Count > 0)
                {
                    Arquivo arquivo = arquivos.Pop();

                    int cursorDesce = disco.Count - 1;
                    int cursorSobe = 0;

                    int tamanhoArmazenadoArquivo = 0;
                    while (tamanhoArmazenadoArquivo < arquivo.Tamanho)
                    {
                        while (disco[cursorDesce] != arquivo)
                        {
                            if (cursorDesce <= 0)
                            {
                                return;
                            }

                            cursorDesce--;
                        }

                        while (disco[cursorSobe] is not Espaco)
                        {
                            if (cursorSobe >= cursorDesce || cursorSobe >= disco.Count - 1)
                            {
                                return;
                            }

                            cursorSobe++;
                        }

                        if (!SwapBloco(cursorSobe, cursorDesce))
                        {
                            return;
                        }

                        cursorDesce--;
                        tamanhoArmazenadoArquivo++;
                    }
                }
            }

            public long Checksum()
            {
                long checksum = 0;

                for (int i = 0; i < disco.Count; i++)
                {
                    if (disco[i] is Arquivo arquivo)
                    {
                        checksum += i * arquivo.Id;
                    }
                }

                return checksum;
            }

            public Disco(string input)
            {
                this.disco = [];
                this.arquivos = [];

                var listaParseada = Parser.ListaDeInts(input);

                bool ehEspaco = false;
                foreach (int tamanhoBloco in listaParseada)
                {
                    if (tamanhoBloco > 0)
                    {
                        IBloco bloco = ehEspaco ? new Espaco(tamanhoBloco) : new Arquivo(tamanhoBloco);

                        if (bloco is Arquivo arquivo)
                        {
                            this.arquivos.Push(arquivo);
                        }
                        
                        for (int j = 0; j < tamanhoBloco; j++)
                        {
                            disco.Add(bloco);
                        }
                    }

                    ehEspaco = !ehEspaco;
                }
            }
        }


        public string SolucaoParte1(string input)
        {
            Disco disco = new Disco(input);

            disco.Fragmenta();

            return disco.Checksum().ToString();
        }

        public string SolucaoParte2(string input)
        {
            return ":D";
        }
    }
}
