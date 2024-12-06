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
        private char GetElemento((int x, int y) pos, List<List<char>> mapa) 
        {
            return mapa.ElementAtOrDefault(pos.y)?.ElementAtOrDefault(pos.x) ?? default(char);
        }
        private void EscreverMapaNoDisco(List<List<char>> mapa)
        {
            File.WriteAllText("mapa.txt", string.Join("\n", mapa.Select(innerList => new string(innerList.ToArray()))));
        }

        private readonly (int dx, int dy)[] Direcoes =
        {
            (0, -1), (1, 0), (0, 1), (-1, 0)
        };

        private (int x, int y) EncontraPosicaoDoGuarda(List<List<char>> mapa)
        {
            for (int y = 0; y < mapa.Count; y++)
            {
                for (int x = 0;  x < mapa[y].Count; x++)
                {
                    if (mapa[y][x] == '^')
                        return (x, y);
                }
            }

            throw new Exception("Posicao inicial não encontrada");
        }

        private int QuantidadeDePosicoesDiferentes((int x, int y) posicaoInicial, List<List<char>> mapa, bool procuraLoops = false)
        {
            bool ChecaExistenciaDeLoop((int x, int y) posicaoOriginal, (int dx, int dy) direcao)
            {
                (int x, int y) posicao = (posicaoOriginal.x + direcao.dx, posicaoOriginal.y + direcao.dy);

                if (mapa.ElementAtOrDefault(posicao.y)?.ElementAtOrDefault(posicao.x) != 'X')
                {
                    return false;
                }


                while (EhPosicaoDentroDoMapa(posicao))
                {
                    if (GetElemento(posicao, mapa) == '.')
                    {
                        break;
                    }

                    if (GetElemento(posicao, mapa) == '#')
                    {
                        EscreverMapaNoDisco(mapa);
                        return true;
                    }

                    posicao.x += direcao.dx;
                    posicao.y += direcao.dy;
                }

                return false;
            }

            bool EhPosicaoDentroDoMapa((int x, int y) posicao) => (posicao.x is >= 0 and < 130) && (posicao.y is >= 0 and < 130);

            int indiceDirecao = 0;

            (int dx, int dy) getNovaDirecao(int idx) => Direcoes[idx % 4];

            (int dx, int dy) direcao = getNovaDirecao(indiceDirecao++);
            (int x, int y) posicao = posicaoInicial;

            while (EhPosicaoDentroDoMapa(posicao))
            {
                if(mapa[posicao.y][posicao.x] == '.')
                {
                    mapa[posicao.y][posicao.x] = 'X';
                }

                if (procuraLoops && 
                    ChecaExistenciaDeLoop(posicao, getNovaDirecao(indiceDirecao)) &&
                    EhPosicaoDentroDoMapa((posicao.x + direcao.dx, posicao.y + direcao.dy)))
                {
                    mapa[posicao.y + direcao.dy][posicao.x + direcao.dx] = 'O';
                    EscreverMapaNoDisco(mapa);
                }

                (int x, int y) novaPosicao = (posicao.x + direcao.dx, posicao.y + direcao.dy);

                while (GetElemento(novaPosicao, mapa) == '#')
                {
                    direcao = getNovaDirecao(indiceDirecao++);
                    novaPosicao = (posicao.x + direcao.dx, posicao.y + direcao.dy);
                }

                posicao.x = novaPosicao.x;
                posicao.y = novaPosicao.y;
            }

            char procurado = procuraLoops ? 'O' : 'X';

            return mapa.SelectMany(linha => linha).Count(c => c == procurado);
        }

        public string SolucaoParte1(string input)
        {
            return ":D";
            var mapa = Parser.ParseMatrizDeTexto(input);

            var posicaoInicial = EncontraPosicaoDoGuarda(mapa);

            return QuantidadeDePosicoesDiferentes(posicaoInicial, mapa).ToString();
        }

        public string SolucaoParte2(string input)
        {
            var mapa = Parser.ParseMatrizDeTexto(input);

            var posicaoInicial = EncontraPosicaoDoGuarda(mapa);

            return QuantidadeDePosicoesDiferentes(posicaoInicial, mapa, true).ToString();
        }
    }
}
