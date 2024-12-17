using AOC24.Comuns;
using AOC24.Utils;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace AOC24.Solucoes
{
    internal class Dia15 : ISolucionador
    {
        private const char ROBO = '@';
        private const char CAIXA = 'O';
        private const char MURO = '#';
        private const char CAIXA_ESQ = '[';
        private const char CAIXA_DIR = ']';

        internal class Mapa15Largo : Mapa15
        {
            public Mapa15Largo(string input)
            {
                string inputLargo = input
                    .Replace(".", "..")
                    .Replace("#", "##")
                    .Replace("O", "[]")
                    .Replace("@", "@.");

                this.mapa = Parser.MatrizDeChars(inputLargo);

                this.posicaoRobo = this.PegaTodasOcorrenciasDe(ROBO, remove: true).Single();
            }

            protected override bool EmpurraObjeto((int x, int y) posicao, Direcao direcao)
            {
                char item = GetItem(posicao);

                if (item == MURO)
                {
                    return false;
                }

                if (item is CAIXA_ESQ or CAIXA_DIR)
                {
                    var posicao1frente = direcao.PosicaoAFrente(posicao);
                    if (direcao is Direcao.Norte or Direcao.Sul)
                    {
                        var posicao2 = (item == CAIXA_ESQ) ? (posicao.x + 1, posicao.y) : (posicao.x - 1, posicao.y);

                        var posicao2frente = direcao.PosicaoAFrente(posicao2);

                        if (EmpurraObjeto(direcao.PosicaoAFrente(posicao), direcao) && EmpurraObjeto(direcao.PosicaoAFrente(posicao2), direcao))
                        {
                            this.TrocaItem(posicao, posicao1frente);
                            this.TrocaItem(posicao2, posicao2frente);

                            return true; ;
                        }
                    } 
                    else
                    {
                        if (EmpurraObjeto(direcao.PosicaoAFrente(posicao), direcao))
                        {
                            this.TrocaItem(posicao, posicao1frente);
                            return true;
                        }
                    }
                    

                    return false;
                }

                return true;
            }
        }

        internal class Mapa15 : Mapa
        {
            protected (int x, int y) posicaoRobo;

            protected Mapa15() : base() { }

            public Mapa15(string mapa) : base(mapa) 
            {
                this.posicaoRobo = this.PegaTodasOcorrenciasDe(ROBO, remove: true).Single();
            }

            protected virtual bool EmpurraObjeto((int x, int y) posicao, Direcao direcao)
            {
                char item = GetItem(posicao);

                if (item == MURO)
                {
                    return false;
                }

                if (item == CAIXA)
                {
                    var posicaoAFrente = direcao.PosicaoAFrente(posicao);
                    if (EmpurraObjeto(posicaoAFrente, direcao))
                    {
                        this.TrocaItem(posicao, posicaoAFrente);

                        return true;
                    }

                    return false;
                }

                return true;
            }

            public virtual long SomaGPSCoordenadasCaixas()
            {
                long soma = 0;

                foreach (var caixa in this.PegaTodasOcorrenciasDe(CAIXA))
                {
                    soma += 100 * caixa.y + caixa.x;
                }

                foreach (var caixa in this.PegaTodasOcorrenciasDe(CAIXA_ESQ))
                {
                    soma += 100 * caixa.y + caixa.x;
                }

                return soma;
            }

            public void MoveRobo(Direcao direcao)
            {
                var destino = direcao.PosicaoAFrente(posicaoRobo);

                if (!EmpurraObjeto(destino, direcao))
                {
                    return;
                }

                this.posicaoRobo = destino;
            }

            public void AbreJanelaComRender(bool interativa = true)
            {
                int escala = 1080 / Math.Max(this.Largura, this.Altura);
                void DesenhaComCor((int x, int y) posicao, Color cor, bool objetoGordo = false)
                {
                    int mult_largura = (objetoGordo ? 2 : 1);
                    Raylib.DrawRectangle(posicao.x * escala, posicao.y * escala, escala * mult_largura, escala, cor);
                    Raylib.DrawRectangleLines(posicao.x * escala, posicao.y * escala, escala * mult_largura, escala, Color.Black);
                }

                Raylib.SetTraceLogLevel(TraceLogLevel.None);
                Raylib.InitWindow(this.Largura * escala, this.Altura * escala, "Dia15");
                
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
                    foreach (var caixa in this.PegaTodasOcorrenciasDe(CAIXA))
                    {
                        DesenhaComCor(caixa, Color.Brown);
                    }
                    foreach (var caixa in this.PegaTodasOcorrenciasDe(CAIXA_ESQ))
                    {
                        DesenhaComCor(caixa, Color.Brown, true);
                    }
                    foreach (var muro in muros)
                    {
                        DesenhaComCor(muro, Color.Black);
                    }

                    Raylib.EndDrawing();
                }

                Raylib.CloseWindow();
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
            var inputdivido = input.Split("\n\n");

            Mapa15Largo mapa15 = new(inputdivido[0]);
            Comandos comandos = new(inputdivido[1]);

            foreach (var direcao in comandos.direcoes)
            {
                mapa15.MoveRobo(direcao);
            }

            return mapa15.SomaGPSCoordenadasCaixas().ToString();
        }
    }
}
