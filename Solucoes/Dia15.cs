using AOC24.Comuns;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AOC24.Solucoes.Dia14;

namespace AOC24.Solucoes
{
    internal class Dia15 : ISolucionador
    {
        private const char ROBO = '@';
        private const char CAIXA = 'O';
        private const char MURO = '#';

         internal class Mapa15 : Mapa
        {
            protected (int x, int y) posicaoRobo;
            protected HashSet<(int x, int y)> caixas = [];

            protected Mapa15() : base() { }

            public Mapa15(string mapa) : base(mapa) 
            {
                this.posicaoRobo = this.PegaTodasOcorrenciasDe(ROBO, remove: true).Single();
                this.caixas = this.PegaTodasOcorrenciasDe(CAIXA, remove: true);
            }

            private bool PossoEmpurrarCaixa((int x, int y) posicao, Direcao direcao, out (int x, int y) posicaoFimTrilha)
            {
                do
                {
                    posicao = direcao.PosicaoAFrente(posicao);
                } while (caixas.Contains(posicao));

                posicaoFimTrilha = posicao;

                return GetItem(posicaoFimTrilha) == vazio;
            }

            public void AbreJanelaComRender(bool interativa = true)
            {
                int escala = 1080 / Math.Max(this.Largura, this.Altura) / 3 * 2;
                void DesenhaComCor((int x, int y) posicao, Color cor)
                {
                    Raylib.DrawRectangle(posicao.x * escala, posicao.y * escala, escala, escala, cor);
                    Raylib.DrawRectangleLines(posicao.x * escala, posicao.y * escala, escala, escala, Color.Black);
                }
                Raylib.SetTraceLogLevel(TraceLogLevel.None);
                Raylib.InitWindow(this.Altura * escala, this.Altura * escala, "Dia15");
                var muros = this.PegaTodasOcorrenciasDe(MURO);

                while (!Raylib.WindowShouldClose())
                {
                    if (interativa)
                    {
                        if (Raylib.IsKeyPressed(KeyboardKey.Up))
                        {
                            MoveRobo(Direcao.Norte);
                        }
                        else if (Raylib.IsKeyPressed(KeyboardKey.Down))
                        {
                            MoveRobo(Direcao.Sul);
                        }
                        else if (Raylib.IsKeyPressed(KeyboardKey.Right))
                        {
                            MoveRobo(Direcao.Leste);
                        }
                        else if (Raylib.IsKeyPressed(KeyboardKey.Left))
                        {
                            MoveRobo(Direcao.Oeste);
                        }
                    }

                    Raylib.GetFrameTime();
                    Raylib.BeginDrawing();
                    Raylib.ClearBackground(Color.White);

                    DesenhaComCor(posicaoRobo, Color.Blue);
                    foreach (var caixa in caixas)
                    {
                        DesenhaComCor(caixa, Color.Brown);
                    }
                    foreach (var muro in muros)
                    {
                        DesenhaComCor(muro, Color.Black);
                    }

                    Raylib.EndDrawing();
                }

                Raylib.CloseWindow();
            }

            public long SomaGPSCoordenadasCaixas()
            {
                long soma = 0;

                foreach (var caixa in caixas)
                {
                    soma += 100 * caixa.y + caixa.x;
                }

                return soma;
            }

            public bool MoveRobo(Direcao direcao)
            {
                var destino = direcao.PosicaoAFrente(posicaoRobo);

                if (this.GetItem(destino) == MURO)
                {
                    return false;
                }

                if (caixas.Contains(destino))
                {
                    if (!PossoEmpurrarCaixa(destino, direcao, out var posicaoLivreFimTrilha))
                    {
                        return false;
                    }

                    this.caixas.Remove(destino);
                    this.caixas.Add(posicaoLivreFimTrilha);
                }

                this.posicaoRobo = destino;

                return true;
            }
        }

        internal class Comandos
        {
            public List<Direcao> direcoes;
            public Comandos(string input)
            {
                direcoes = [];
                foreach (char c in input)
                {
                    Direcao direcao = DirecaoUtils.CharParaDirecao(c);
                    if (direcao != Direcao.Nenhuma)
                    {
                        direcoes.Add(direcao);
                    }
                }
            }
        }

        public string SolucaoParte1(string input)
        {
            var inputdivido = input.Split("\n\n");

            Mapa15 mapa15 = new(inputdivido[0]);
            Comandos comandos = new(inputdivido[1]);

            foreach (var direcao in comandos.direcoes)
            {
                mapa15.MoveRobo(direcao);
            }

            return mapa15.SomaGPSCoordenadasCaixas().ToString();
        }

        public string SolucaoParte2(string input)
        {
            throw new NotImplementedException();
        }
    }
}
