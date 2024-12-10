using AOC24.Comuns;
using AOC24.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AOC24.Solucoes
{
    internal class Dia10 : ISolucionador
    {
        internal class Trilha
        {
            private readonly Mapa mapa;

            public Trilha(Mapa mapa)
            {
                this.mapa = mapa;
            }

            private IEnumerable<Vector3> PontosVizinhosSubida(Vector3 origem)
            {
                Vector3 GetItem(float x, float y) => new(x, y, this.mapa.GetItemInt((int)x, (int)y));

                List<Vector3> pontos =
                [
                    GetItem(origem.X + 1, origem.Y    ),
                GetItem(origem.X - 1, origem.Y    ),
                GetItem(origem.X    , origem.Y + 1),
                GetItem(origem.X    , origem.Y - 1),
            ];

                pontos = pontos.Where(p => p.Z == origem.Z + 1).ToList();

                return pontos
                    .Where(p => p.Z == origem.Z + 1);
            }

            private void DFS_Parte1(Vector3 origem, HashSet<(int, int)> hs)
            {
                if (origem.Z == 9)
                {
                    hs.Add(((int)origem.X, (int)origem.Y));
                    return;
                }

                foreach (Vector3 ponto in PontosVizinhosSubida(origem))
                {
                    DFS_Parte1(ponto, hs);
                }

            }

            private int DFS_Parte2(Vector3 origem)
            {
                if (origem.Z == 9)
                {
                    return 1;
                }

                int soma = 0;

                foreach (Vector3 ponto in PontosVizinhosSubida(origem))
                {
                    soma += DFS_Parte2(ponto);
                }

                return soma;
            }

            public int PontuacaoTotalParte1()
            {
                int soma = 0;

                foreach (var (x, y) in this.mapa.PegaTodasOcorrenciasDe('0'))
                {
                    HashSet<(int, int)> NovesAlcancadosDaqui = [];
                    DFS_Parte1(new Vector3(x, y, 0), NovesAlcancadosDaqui);
                    soma += NovesAlcancadosDaqui.Count;
                }

                return soma;
            }

            public int RatingParte2()
            {
                int soma = 0;

                foreach (var (x, y) in this.mapa.PegaTodasOcorrenciasDe('0'))
                {
                    soma += DFS_Parte2(new Vector3(x, y, 0));
                }

                return soma;
            }

        }

        public string SolucaoParte1(string input)
        {
            Mapa mapa = new(Parser.MatrizDeChars(input));
            Trilha trilha = new(mapa);

            return trilha.PontuacaoTotalParte1().ToString();
        }

        public string SolucaoParte2(string input)
        {
            Mapa mapa = new(Parser.MatrizDeChars(input));
            Trilha trilha = new(mapa);

            return trilha.RatingParte2().ToString();
        }
    }
}
