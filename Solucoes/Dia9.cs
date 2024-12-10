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
        internal class Disco
        {
            private const int idEspaco = -1;
            private static int ultimoIdDado = -1;
            private List<int> disco;
            private Stack<(int id, int tamanho)> arquivos;

            private void SwapBloco(int a, int b) => (disco[a], disco[b]) = (disco[b], disco[a]);

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder(disco.Count);

                for (int i = 0; i < disco.Count; i++)
                {
                    int id = disco[i];
                    if (id >= 0)
                    {
                        sb.Append(id);
                    }
                    else
                    {
                        sb.Append('.');
                    }
                }

                return sb.ToString();
            }

            public void CompactaFragmentando()
            {
                int cursorA = 0;
                int cursorB = disco.Count - 1;

                while (cursorA <= cursorB)
                {
                    bool temEspacoNoA = disco[cursorA] == -1;
                    bool temArquivoNoB = disco[cursorB] > 0;

                    if (temEspacoNoA && temArquivoNoB)
                    {
                        SwapBloco(cursorA, cursorB);
                        cursorB--;
                        cursorA++;
                    } 
                    else
                    {
                        if (!temEspacoNoA) cursorA++;
                        if (!temArquivoNoB) cursorB--;
                    }
                }
            }

            public long Checksum()
            {
                long checksum = 0;

                for (int i = 0; i < disco.Count; i++)
                {
                    int idBloco = disco[i];
                    if (idBloco >= 0)
                    {
                        checksum += i * idBloco;
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
                        int id = ehEspaco ? Disco.idEspaco : ++Disco.ultimoIdDado;

                        if (!ehEspaco)
                        {
                            this.arquivos.Push((id, tamanhoBloco));
                        }
                        
                        for (int j = 0; j < tamanhoBloco; j++)
                        {
                            disco.Add(id);
                        }
                    }

                    ehEspaco = !ehEspaco;
                }
            }
        }


        public string SolucaoParte1(string input)
        {
            Disco disco = new Disco(input);

            disco.CompactaFragmentando();

            return disco.Checksum().ToString();
        }

        public string SolucaoParte2(string input)
        {
            return ":D";
        }
    }
}
