using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC24.Solucoes
{
    internal class Dia4 : ISolucionador
    {
        private static readonly (int x, int y)[] Direcoes = Enumerable.Range(-1, 3)
            .SelectMany(x => Enumerable.Range(-1, 3), (x, y) => (x, y))
            .Where(dir => dir != (0, 0))
            .ToArray();

        private bool PossuiTextoNestaDirecao(List<List<char>> matrizDeTexto, string texto, int x, int y, (int x, int y) direcao)
        {
            for (int i = 0; i < texto.Length; i++)
            {
                try
                {
                    (int x, int y) deslocamento = (direcao.x * i, direcao.y * i);
                    char charTexto = texto[i];
                    char charMatriz = matrizDeTexto[y + deslocamento.y][x + deslocamento.x];
                    if (charTexto != charMatriz)
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        private int FrequenciaDePalavraNaMatriz(List<List<char>> matrizDeTexto, string palavra)
        {
            int acumulador = 0;

            for (int y = 0; y < matrizDeTexto.Count; y++)
            {
                for (int x = 0; x < matrizDeTexto[y].Count; x++)
                {
                    foreach (var direcao in Direcoes)
                    {
                        for (int i = 0; i < palavra.Length; i++)
                        {
                            if(PossuiTextoNestaDirecao(matrizDeTexto, palavra, x, y, direcao))
                            {
                                acumulador++;
                            }
                        }
                    }
                }
            }

            return acumulador;
        }

        public string SolucaoParte1(string input)
        {
            List<List<char>> entrada = Utils.Listas.ParseMatrizDeTexto(input);
            string palavra = "XMAS";

            return FrequenciaDePalavraNaMatriz(entrada, palavra).ToString();
        }

        public string SolucaoParte2(string input)
        {
            return ":D";
        }
    }
}
