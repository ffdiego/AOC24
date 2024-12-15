using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC24.Comuns
{
    internal enum Direcao
    {
        Oeste = -2, 
        Norte, 
        Nenhuma = 0,
        Sul, 
        Leste, 
    }

    internal static class DirecaoUtils
    {
        public static IEnumerable<Direcao> Direcoes { get => Enum.GetValues<Direcao>().Where(d => d != Direcao.Nenhuma); }

        public static Direcao Oposta(this Direcao direcao)
        {
            return (Direcao)((int)direcao * -1);
        }

        public static IEnumerable<Direcao> DirecoesPerpendiculares(this Direcao direcao)
        {
            switch (direcao)
            {
                case Direcao.Norte:
                case Direcao.Sul:
                    return [Direcao.Leste, Direcao.Oeste];
                case Direcao.Leste:
                case Direcao.Oeste:
                    return [Direcao.Norte, Direcao.Sul];
                default:
                    throw new ArgumentException();
            }
        }

        public static IEnumerable<(int x, int y)> Perpendicular(this Direcao direcao, (int x, int y) pos)
        {
            switch (direcao)
            {
                case Direcao.Norte:
                case Direcao.Sul:
                    return [(pos.x - 1, pos.y    ), (pos.x + 1, pos.y    )];
                case Direcao.Leste:
                case Direcao.Oeste:
                    return [(pos.x    , pos.y - 1), (pos.x    , pos.y + 1)];
                default:
                    throw new ArgumentException();
            }
        }

        public static (int x, int y) PosicaoAFrente(this Direcao direcao, (int x, int y) pos)
        {
            switch (direcao)
            {
                case Direcao.Norte:
                    return (pos.x, pos.y - 1);
                case Direcao.Sul:
                    return (pos.x, pos.y + 1);
                case Direcao.Leste:
                    return (pos.x + 1, pos.y);
                case Direcao.Oeste:
                    return (pos.x - 1, pos.y);
                default:
                    throw new ArgumentException();
            }
        }

        public static Direcao CharParaDirecao(char c)
        {
            switch (c)
            {
                case '^':
                    return Direcao.Norte;
                case 'v':
                    return Direcao.Sul;
                case '<':
                    return Direcao.Oeste;
                case '>':
                    return Direcao.Leste;
                default:
                    return Direcao.Nenhuma;
            }
        } 
    }
}
