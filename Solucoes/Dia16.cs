using AOC24.Comuns;
using AOC24.Utils;
using Raylib_cs;

namespace AOC24.Solucoes
{
    internal class Dia16 : ISolucionador
    {
        internal class Mapa16 : Mapa
        {
            private const char INICIO = 'S';
            private const char FIM = 'E';
            private const char MURO = '#';
            private readonly Posicao inicio;
            private readonly Posicao fim;

            private HashSet<Posicao> visitados;
            private HashSet<(Posicao pos, int custo)> visitadosRender = [];

            public void AbreJanelaComRender()
            {
                int escala = (int)((1080 / Math.Max(this.Largura, this.Altura)) * (double)0.8);
                void DesenhaComCor((int x, int y) posicao, Color cor, string? texto = null)
                {
                    Raylib.DrawRectangle(posicao.x * escala, posicao.y * escala, escala, escala, cor);
                    Raylib.DrawRectangleLines(posicao.x * escala, posicao.y * escala, escala, escala, Color.Black);
                    if (texto != null) Raylib.DrawText(texto, posicao.x * escala +14, posicao.y * escala + 14, 14, Color.Black);
                }

                Raylib.SetTraceLogLevel(TraceLogLevel.None);
                Raylib.InitWindow(this.Largura * escala, this.Altura * escala, "Dia16");
                

                var muros = this.PegaTodasOcorrenciasDe(MURO);

                while (!Raylib.WindowShouldClose())
                {
                    Raylib.BeginDrawing();
                    Raylib.ClearBackground(Color.White);
  
                    foreach (var muro in muros)
                    {
                        DesenhaComCor(muro, Color.Black);
                    }
                    foreach (var vis in visitadosRender.ToList())
                    {
                        DesenhaComCor(vis.pos, Color.Green, vis.custo.ToString());
                    }

                    DesenhaComCor(inicio, Color.Blue);
                    DesenhaComCor(fim, Color.Red);

                    Raylib.EndDrawing();
                }

                Raylib.CloseWindow();
            }

            public int MinimoPassosAteSaida()
            { 
                PriorityQueue<(Posicao, Direcao, int custo), int> lugaresPraVisitar = new();
                DirecaoUtils.Direcoes.ToList().ForEach(d => lugaresPraVisitar.Enqueue((inicio, d, 0), 0));

                visitados = [inicio];


                while (lugaresPraVisitar.Count > 0)
                {
                    var (posicaoAtual, direcaoAtual, custoAtual) = lugaresPraVisitar.Dequeue();

                    if (GetItem(posicaoAtual) == FIM)
                    {
                        return custoAtual;
                    }

                    foreach (Direcao direcao in DirecaoUtils.Direcoes)
                    {
                        Posicao proximaPosicao = direcao.PosicaoAFrente(posicaoAtual);

                        if (GetItem(proximaPosicao) != MURO && !visitados.Contains(proximaPosicao))
                        {
                            int custo = custoAtual + 1;
                            if (direcaoAtual != direcao) 
                            {
                                custo += 1000;
                            }

                            lugaresPraVisitar.Enqueue((proximaPosicao, direcao, custo), custo);
                            visitados.Add(proximaPosicao);
                            visitadosRender.Add((proximaPosicao, custo));
                            Thread.Sleep(200);
                        }
                    }
                }

                return -1;
            }
            
            public Mapa16(string input)
            {
                this.mapa = Parser.MatrizDeChars(input);
                this.GetItem(new Posicao(2, 3));

                this.inicio = this.PegaTodasOcorrenciasDe(INICIO).Single();
                this.fim = this.PegaTodasOcorrenciasDe(FIM).Single();
                this.visitados = [];
            }            
        }

        public string SolucaoParte1(string input)
        {
            input = @"###############
#.......#....E#
#.#.###.#.###.#
#.....#.#...#.#
#.###.#####.#.#
#.#.#.......#.#
#.#.#####.###.#
#...........#.#
###.#.#####.#.#
#...#.....#.#.#
#.#.#.###.#.#.#
#.....#...#.#.#
#.###.#.#.#.#.#
#S..#.....#...#
###############";
            Mapa16 mapa16 = new(input);
            Thread renderThread = new Thread(() => mapa16.AbreJanelaComRender());
            renderThread.Start();

            return mapa16.MinimoPassosAteSaida().ToString();
        }

        public string SolucaoParte2(string input)
        {
            throw new NotImplementedException();
        }
    }
}
