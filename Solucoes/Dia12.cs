using AOC24.Comuns;
using AOC24.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC24.Solucoes
{
    internal class Dia12 : ISolucionador
    {
        class MapaDia12 : Mapa
        {
            public List<Regiao> Regioes;
            public Dictionary<(int x, int y), Regiao> CoordenadaParaRegiao;


            private void TentaExpandirRegiao(Regiao regiao, int x, int y, Direcao direcaoOrigem)
            {
                char planta = this.GetItem(x, y);
                if (planta != regiao.Planta)
                {
                    return;
                }

                if (!regiao.blocoList.Add((x, y))) 
                {
                    return;
                }

                this.CoordenadaParaRegiao.Add((x, y), regiao);

                if (direcaoOrigem != Direcao.Norte)
                {
                    TentaExpandirRegiao(regiao, x, y - 1, Direcao.Sul);
                }
                if (direcaoOrigem != Direcao.Leste)
                {
                    TentaExpandirRegiao(regiao, x + 1, y, Direcao.Oeste);
                }
                if (direcaoOrigem != Direcao.Sul)
                {
                    TentaExpandirRegiao(regiao, x, y + 1, Direcao.Norte);
                }
                if (direcaoOrigem != Direcao.Oeste)
                {
                    TentaExpandirRegiao(regiao, x - 1, y, Direcao.Leste);
                }

            }

            private void CriaRegiao(int x, int y)
            {
                if (CoordenadaParaRegiao.ContainsKey((x, y)))
                {
                    return;
                }

                char planta = this.GetItem(x, y);
                Regiao regiao = new(planta);

                this.Regioes.Add(regiao);

                TentaExpandirRegiao(regiao, x, y, Direcao.Nenhuma);
            }

            public void MapeiaRegioes()
            {
                for (int y = 0; y < this.mapa.Count; y++)
                {
                    for (int x = 0; x < this.mapa[0].Count; x++)
                    {
                        CriaRegiao(x, y);
                    }
                }
            }

            public long ValorTotalCercas()
            {
                return this.Regioes.Sum(r => r.ValorCerca());
            }

            public MapaDia12(List<List<char>> mapa) : base(mapa) 
            {
                Regioes = [];
                CoordenadaParaRegiao = [];
            }
        }

        class Regiao
        {
            public char Planta { get; private set; }
            public int Area { get => blocoList.Count; }
            public int Perimetro { get => CalculaPerimetro(); }
            private int perimetro;
            public HashSet<(int x, int y)> blocoList = [];

            public long ValorCerca() => this.Area * this.Perimetro;

            private int CalculaPerimetro()
            {
                if (this.perimetro != -1) 
                {
                    return this.perimetro;
                }

                int perimetro = 0;

                foreach (var bloco in blocoList)
                {
                    if (!blocoList.Contains((bloco.x + 1, bloco.y     ))) perimetro++;
                    if (!blocoList.Contains((bloco.x - 1, bloco.y     ))) perimetro++;
                    if (!blocoList.Contains((bloco.x    , bloco.y + 1 ))) perimetro++;
                    if (!blocoList.Contains((bloco.x    , bloco.y - 1 ))) perimetro++;
                }

                this.perimetro = perimetro;
                return perimetro;
            }

            public Regiao(char planta)
            {
                Planta = planta;
                this.perimetro = -1;
            }
        }

        public string SolucaoParte1(string input)
        {
//            input = @"RRRRIICCFF
//RRRRIICCCF
//VVRRRCCFFF
//VVRCCCJFFF
//VVVVCJJCFE
//VVIVCCJJEE
//VVIIICJJEE
//MIIIIIJJEE
//MIIISIJEEE
//MMMISSJEEE".ReplaceLineEndings("\n");

            MapaDia12 mapa = new(Parser.MatrizDeChars(input));

            mapa.MapeiaRegioes();

            return mapa.ValorTotalCercas().ToString();
        }

        public string SolucaoParte2(string input)
        {
            return 0.ToString();
        }
    }
}
