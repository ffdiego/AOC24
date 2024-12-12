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


            private void TentaExpandirRegiao(Regiao regiao, (int x, int y) pos, Direcao direcaoOrigem)
            {
                char planta = this.GetItem(pos);
                if (planta != regiao.Planta)
                {
                    return;
                }

                if (!regiao.blocos.Add(pos))
                {
                    return;
                }

                this.CoordenadaParaRegiao.Add(pos, regiao);

                foreach (var direcao in DirecaoUtils.Direcoes.Where(d => d != direcaoOrigem))
                {
                    var proximo = direcao.PosicaoAFrente(pos);
                    TentaExpandirRegiao(regiao, proximo, direcao.Oposta());
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

                TentaExpandirRegiao(regiao, (x, y), Direcao.Nenhuma);
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

            public long ValorTotalCercas() => this.Regioes.Sum(r => r.Area * r.Perimetro);

            public long ValorTotalCercasDesconto() => this.Regioes.Sum(r => r.Area * r.PerimetroDesconto);

            public MapaDia12(List<List<char>> mapa) : base(mapa) 
            {
                Regioes = [];
                CoordenadaParaRegiao = [];
            }
        }

        class Regiao
        {
            public char Planta { get; private set; }
            public int Area { get => blocos.Count; }
            public int Perimetro { get => CalculaPerimetro(); }
            public int PerimetroDesconto { get => CalculaPerimetro(true); }

            public HashSet<(int x, int y)> blocos = [];

            private int CalculaPerimetro(bool aplicaDesconto = false)
            {
                HashSet<((int x, int y) pos, Direcao direcao)> cercas = [];

                int perimetro = 0;

                foreach (var bloco in blocos)
                {
                    foreach (Direcao direcao in DirecaoUtils.Direcoes)
                    {
                        var posicaoAFrente = direcao.PosicaoAFrente(bloco);
                        if (!blocos.Contains(posicaoAFrente))
                        {
                            perimetro++;

                            if (aplicaDesconto)
                            {
                                cercas.Add((bloco, direcao));
                            }
                        }
                    }
                }

                if (aplicaDesconto)
                {
                    HashSet<((int x, int y) pos, Direcao direcao)> cercasContabilizadas = [];
                    int desconto = 0;

                    foreach(var cerca in cercas)
                    {
                        if (cercasContabilizadas.Contains(cerca))
                        {
                            continue;
                        }

                        foreach(Direcao perpendicular in cerca.direcao.DirecoesPerpendiculares())
                        {
                            (int x, int y) novaPosicao = perpendicular.PosicaoAFrente(cerca.pos);
                            while (cercas.Contains((novaPosicao, cerca.direcao)))
                            {
                                if(cercasContabilizadas.Add((novaPosicao, cerca.direcao)))
                                {
                                    Console.WriteLine($"- {this.Planta}: Descontada Cerca em {novaPosicao} -> {cerca.direcao}");
                                    desconto++;
                                }

                                novaPosicao = perpendicular.PosicaoAFrente(novaPosicao);
                            }
                        }
                    }

                    perimetro -= desconto;

                    Console.WriteLine($"{this.Planta}: {this.Area} * {perimetro} = {this.Area * perimetro}");
                }

                return perimetro;
            }

            public Regiao(char planta)
            {
                Planta = planta;
            }
        }

        public string SolucaoParte1(string input)
        {
            input = @"AAAAAA
AAAAAA
AAAAAA
AAAAAA
AAAAAA
AAAAAA".ReplaceLineEndings("\n");
            MapaDia12 mapa = new(Parser.MatrizDeChars(input));

            mapa.MapeiaRegioes();

            return mapa.ValorTotalCercas().ToString();
        }

        public string SolucaoParte2(string input)
        {
            input = @"RRRRIICCFF
RRRRIICCCF
VVRRRCCFFF
VVRCCCJFFF
VVVVCJJCFE
VVIVCCJJEE
VVIIICJJEE
MIIIIIJJEE
MIIISIJEEE
MMMISSJEEE".ReplaceLineEndings("\n");
            MapaDia12 mapa = new(Parser.MatrizDeChars(input));

            mapa.MapeiaRegioes();

            return mapa.ValorTotalCercasDesconto().ToString();
        }
    }
}
