using AOC24.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC24.Solucoes
{
    internal class Dia6 : ISolucionador
    {
        internal static class Objetos
        {
            public const char Obstaculo = '#';
            public const char InicioGuarda = '^';
        }

        internal class Mapa
        {
            private List<List<char>> mapa;
            public int Tamanho { get; private set; }
            public char GetElementoDoMapa((int x, int y) pos)
            {
                return this.mapa.ElementAtOrDefault(pos.y)?.ElementAtOrDefault(pos.x) ?? default;
            }

            public (int x, int y) EncontraPosicaoDoGuarda()
            {
                for (int y = 0; y < this.mapa.Count; y++)
                {
                    for (int x = 0; x < this.mapa[y].Count; x++)
                    {
                        if (mapa[y][x] == Objetos.InicioGuarda)
                            return (x, y);
                    }
                }

                throw new Exception("Posicao inicial não encontrada");
            }

            public Mapa(List<List<char>> mapa)
            {
                this.mapa = mapa;
                this.Tamanho = mapa.Count;
            }
        }

        internal class Guarda
        {
            private readonly Mapa mapa;
            private (int x, int y) posicao;
            private int indexDirecao;
            private readonly HashSet<((int, int), (int, int))> estados;
            private (int dx, int dy) direcaoAtual => direcoes[this.indexDirecao % 4];
            private readonly (int dx, int dy)[] direcoes =
            {
            (0, -1), (1, 0), (0, 1), (-1, 0)
        };

            private char OlharPraFrente()
            {
                var pos = (posicao.x + direcaoAtual.dx, posicao.y + direcaoAtual.dy);

                if (PosicaoObstaculoHipotetico.HasValue && PosicaoObstaculoHipotetico == pos)
                {
                    return Objetos.Obstaculo;
                }

                return mapa.GetElementoDoMapa(pos);
            }

            public (int x, int y)? PosicaoObstaculoHipotetico { get; set; }

            private bool AndarPraFrente()
            {
                this.posicao = (posicao.x + direcaoAtual.dx, posicao.y + direcaoAtual.dy);
                return this.estados.Add((this.posicao, this.direcaoAtual));
            }

            private void Virar()
            {
                this.indexDirecao++;
            }

            private bool EstouDentroDoMapa()
            {
                return (posicao.x >= 0 && posicao.x < mapa.Tamanho) && (posicao.y >= 0 && posicao.y < mapa.Tamanho);
            }

            public bool Step()
            {
                while (OlharPraFrente() == Objetos.Obstaculo)
                {
                    this.Virar();
                }

                return this.AndarPraFrente();
            }

            public int QuantidadeDeEspacos()
            {
                while (EstouDentroDoMapa())
                {
                    this.Step();
                }

                return estados.DistinctBy(e => e.Item1).Count();
            }

            public bool EstouNumLoop()
            {
                while (EstouDentroDoMapa())
                {
                    if (!this.Step())
                        return true;
                }

                return false;
            }

            public Guarda(Mapa mapa, (int x, int y) posicao, int indexDirecao)
            {
                this.mapa = mapa;
                this.posicao = posicao;
                this.indexDirecao = indexDirecao;
                this.estados = [];
            }

            public Guarda(Guarda guarda)
            {
                this.mapa = guarda.mapa;
                this.posicao = guarda.posicao;
                this.indexDirecao = guarda.indexDirecao;
                this.estados = [];
            }
        }

        public string SolucaoParte1(string input)
        {
            var mapaTxt = Parser.MatrizDeChars(input);

            Mapa mapa = new(mapaTxt);

            Guarda guarda = new Guarda(mapa, mapa.EncontraPosicaoDoGuarda(), 0);

            return guarda.QuantidadeDeEspacos().ToString();
        }

        public string SolucaoParte2(string input)
        {
            var mapaTxt = Parser.MatrizDeChars(input);
            
            Mapa mapa = new(mapaTxt);
            var posicaoInicial = mapa.EncontraPosicaoDoGuarda();

            int loops = 0;

            for (int x = 0; x < mapa.Tamanho; x++)
            {
                Parallel.For(0, mapa.Tamanho, (y) =>
                {
                    if ((x,y) == posicaoInicial)
                    {
                        return;
                    }

                    Guarda guarda = new Guarda(mapa, posicaoInicial, 0);

                    guarda.PosicaoObstaculoHipotetico = (x, y);

                    if (guarda.EstouNumLoop())
                    {
                        loops++;
                    }
                });
            }

            return loops.ToString();
        }
    }
}
