using System.Text.RegularExpressions;

namespace AOC24.Solucoes;

internal class Dia14 : ISolucionador
{
    internal class Robo
    {
        public (int x, int y) PosicaoInicial;
        public (int x, int y) Velocidade;

        public (int x, int y) SimulaPassos(int passos, int alturaMapa, int larguraMapa)
        {
            int Wrap(int i, int limit)
            {
                int wrapped = i % limit;
                return (wrapped >= 0) ? wrapped : (limit - wrapped); 
            }
            int x = PosicaoInicial.x + Velocidade.x * passos;
            int y = PosicaoInicial.y + Velocidade.y * passos;

            (int xWrapped, int yWrapped) = (Wrap(x, larguraMapa), Wrap(y, alturaMapa));

            return (xWrapped, yWrapped);
        }
    }

    public static List<Robo> GeraRobos(string input)
    {
        List<Robo> robos = [];
        foreach (var linha in input.Split('\n'))
        {
            const string pattern = @"-?\d+";
            Regex regex = new Regex(pattern, RegexOptions.Compiled);
            var matches = regex.Matches(linha).Select(m => int.Parse(m.Value)).ToArray();

            robos.Add(new Robo()
            {
                PosicaoInicial = (matches[0], matches[1]),
                Velocidade = (matches[2], matches[3])
            });
        }
        return robos;
    }

    public string SolucaoParte1(string input)
    {
        input = @"p=0,4 v=3,-3
p=6,3 v=-1,-3
p=10,3 v=-1,2
p=2,0 v=2,-1
p=0,0 v=1,3
p=3,0 v=-2,-2
p=7,6 v=-1,-3
p=3,0 v=-1,-2
p=9,3 v=2,3
p=7,3 v=-1,2
p=2,4 v=2,-3
p=9,5 v=-3,-3".ReplaceLineEndings("\n");
        List<Robo> robos = GeraRobos(input);
        List<(int x, int y)> coordenadasComRobos = [];
        
        const int altura = 7;
        const int largura = 11;
        const int passos = 100;
        const int corteQuadranteX = largura / 2;
        const int corteQuadranteY = altura / 2;

        foreach (var robo in robos)
        {
            coordenadasComRobos.Add(robo.SimulaPassos(passos, altura, largura));
            Console.WriteLine(coordenadasComRobos[0]);
        }

        int q1 = coordenadasComRobos.Count(c => c.x < corteQuadranteX && c.y < corteQuadranteY);
        int q2 = coordenadasComRobos.Count(c => c.x > corteQuadranteX && c.y < corteQuadranteY);
        int q3 = coordenadasComRobos.Count(c => c.x < corteQuadranteX && c.y > corteQuadranteY);
        int q4 = coordenadasComRobos.Count(c => c.x > corteQuadranteX && c.y > corteQuadranteY);

        return $"{q1}*{q2}*{q3}*{q4}={q1*q2*q3*q4} (count={coordenadasComRobos.Count})";
    }

    public string SolucaoParte2(string input)
    {
        return "-1";
    }
}