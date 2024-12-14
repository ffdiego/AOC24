using Raylib_cs;
using System.Text.RegularExpressions;

namespace AOC24.Solucoes;

internal class Dia14 : ISolucionador
{
    const int passos = 100;

    const int largura = 101;
    const int altura = 103;
    const int corteQuadranteX = largura / 2;
    const int corteQuadranteY = altura / 2;

    internal class Robo
    {
        public (int x, int y) PosicaoInicial;
        public (int x, int y) Velocidade;

        public (int x, int y) SimulaPassos(int passos, int alturaMapa, int larguraMapa)
        {
            int Wrap(int i, int limit)
            {
                int wrapped = i % limit;

                if (wrapped < 0)
                {
                    wrapped += limit;
                }

                return (wrapped >= 0) ? wrapped : (limit - wrapped - 1); 
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

    public static void ImprimeRobos(int passo, List<(int x, int y)> robos, int altura, int largura)
    {
        const int escala = 4;
        Raylib.InitWindow(largura * escala, altura * escala, $"Dia14: Posição no segundo {passo}");

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.White);
            
            foreach (var robo in robos) 
            {
                Raylib.DrawRectangle(robo.x * escala, robo.y * escala, escala, escala, Color.Green);
            }

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }

    public static long FatorSeguranca(List<(int x, int y)> robos)
    {
        long q1 = robos.Count(c => c.x < corteQuadranteX && c.y < corteQuadranteY);
        long q2 = robos.Count(c => c.x > corteQuadranteX && c.y < corteQuadranteY);
        long q3 = robos.Count(c => c.x < corteQuadranteX && c.y > corteQuadranteY);
        long q4 = robos.Count(c => c.x > corteQuadranteX && c.y > corteQuadranteY);

        return q1 * q2 * q3 * q4;
    }

    public string SolucaoParte1(string input)
    {
        List<Robo> robos = GeraRobos(input);
        List<(int x, int y)> listaRobos = [];

        foreach (var robo in robos)
        {
            listaRobos.Add(robo.SimulaPassos(passos, altura, largura));
        }

        long resposta = FatorSeguranca(listaRobos);

        return resposta.ToString();
    }

    public string SolucaoParte2(string input)
    {
        List<Robo> robos = GeraRobos(input);
        List<(int x, int y)> listaRobos = [];

        int passos = 0;

        (int passos, long fs) menorFS = (0, long.MaxValue);
        (int passos, long fs) maiorFS = (0, 0);
        while (passos < (altura * largura))
        {
            listaRobos.Clear();
            foreach (var robo in robos)
            {
                listaRobos.Add(robo.SimulaPassos(passos, altura, largura));
            }

            long fs = FatorSeguranca(listaRobos);

            if (fs > maiorFS.fs)
            {
                maiorFS = (passos, fs);
            } 
            else if (fs < menorFS.fs)
            {
                menorFS = (passos, fs);
            }

            passos++;
        }

        listaRobos.Clear();
        foreach (var robo in robos)
        {
            listaRobos.Add(robo.SimulaPassos(menorFS.passos, altura, largura));
        }
        ImprimeRobos(passos, listaRobos, altura, largura);


        return menorFS.passos.ToString();
    }
}