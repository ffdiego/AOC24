using System;
using System.Runtime.Intrinsics.X86;
using AOC24.Comuns;
using Raylib_cs;

namespace AOC24.Solucoes;

public class Dia21 : ISolucionador
{
    private const string NUMPAD =
        """
        789
        456
        123
        .0A
        """;
    private const string DIRPAD =
        """
        .^A
        <v>
        """;

    internal class Keypad : Mapa 
    {
        private const char botaoA = 'A';
        public Dictionary<(char partida, char destino), List<List<Direcao>>> caminhosOtimizados;

        public Keypad(string input) : base(input)
        {
            caminhosOtimizados = MapeiaMenoresDistanciasEntreBotoes();
        }

        public string GeraSequenciaTeclas(string sequencia)
        {
            IEnumerable<char> CaminhoEntreBotoes(char botaoAtual, char botaoDestino)
            {
                List<List<Direcao>> passos = this.caminhosOtimizados[(botaoAtual, botaoDestino)];
                IEnumerable<char> setas = passos[0].Select(p => p.ParaChar()).Append(botaoA);

                return setas;
            }

            char botaoAtual = botaoA;

            List<char> botoesNecessarios = [];
            
            foreach (char tecla in sequencia)
            {
                botoesNecessarios.AddRange(CaminhoEntreBotoes(botaoAtual, tecla));
                botaoAtual = tecla;
            }

            return string.Concat(botoesNecessarios.ToArray());
        }

        public string DigitaSequencia(string sequencia)
        {
            Posicao posicao = PegaTodasOcorrenciasDe(botaoA).Single();

            List<char> botoesPressionados = [];

            foreach (char tecla in sequencia)
            {
                if (tecla == botaoA)
                {
                    botoesPressionados.Add(GetItem(posicao));
                    continue;
                }

                Direcao direcao = DirecaoUtils.CharParaDirecao(tecla);
                posicao = direcao.PosicaoAFrente(posicao);
            }

            return string.Concat(botoesPressionados.ToArray());
        }

        private List<List<Direcao>> EncontraMelhoresCaminho(char botaoPartida, char botaoDestino) 
        {
            Posicao posicaoInicial = this.PegaTodasOcorrenciasDe(botaoPartida).Single();

            PriorityQueue<(Posicao, List<Direcao>, HashSet<Posicao>, int custo), int> lugaresPravisitar = new();
            lugaresPravisitar.Enqueue((posicaoInicial, [], [posicaoInicial], 0), 0);

            (List<List<Direcao>> caminhos, int custo) melhorSolucao = ([], int.MaxValue);

            while (lugaresPravisitar.Count > 0)
            {
                (Posicao posicao, List<Direcao> caminho, HashSet<Posicao> visitados, int custo) = lugaresPravisitar.Dequeue();

                if (GetItem(posicao) == botaoDestino) 
                {
                    if (custo < melhorSolucao.custo)
                    {
                        melhorSolucao.custo = custo;
                        melhorSolucao.caminhos = [caminho];
                    }
                    else if (custo == melhorSolucao.custo)
                    {
                        melhorSolucao.caminhos.Add(caminho);
                    }

                    continue;
                }

                foreach (Direcao direcao in DirecaoUtils.Direcoes) 
                {
                    Posicao nova = direcao.PosicaoAFrente(posicao);
                    char botaoNaPosicao = this.GetItem(nova);

                    if (botaoNaPosicao == default || botaoNaPosicao == this.vazio || visitados.Contains(nova))
                    {
                        continue;
                    }

                    int novoCusto = custo + 1 + (caminho.LastOrDefault() != direcao ? 1000 : 0);

                    lugaresPravisitar.Enqueue((nova, [.. caminho, direcao], [.. visitados, nova], novoCusto), novoCusto);
                }  
            }

            return melhorSolucao.caminhos;
        }
        private Dictionary<(char partida, char destino), List<List<Direcao>>> MapeiaMenoresDistanciasEntreBotoes() 
        {
            Dictionary<(char partida, char destino), List<List<Direcao>>> caminhosOtimizados = [];

            IEnumerable<char> botoes = this.mapa
                .SelectMany(x => x.Distinct())
                .Where(x => x != vazio);

            foreach (char botaoPartida in botoes) 
            {
                foreach (char botaoDestino in botoes.Where(b => b != botaoPartida)) 
                {
                    caminhosOtimizados.Add((botaoPartida, botaoDestino), EncontraMelhoresCaminho(botaoPartida, botaoDestino));
                }

                caminhosOtimizados.Add((botaoPartida, botaoPartida), []);
            }

            return caminhosOtimizados;
        }
    }

    private Keypad numpad = new(NUMPAD);
    private Keypad dirpad = new(DIRPAD);

    public int CalculaComplexidade(string codigo) 
    {
        var seq1 = numpad.GeraSequenciaTeclas(codigo);
        var seq2 = dirpad.GeraSequenciaTeclas(seq1);
        var seq3 = dirpad.GeraSequenciaTeclas(seq2);

        int valor = int.Parse(codigo.Replace("A", ""));

        Console.WriteLine(codigo);
        Console.WriteLine($"{seq3.Length} * {valor} = {seq3.Length * valor}");
        Console.WriteLine();

        return seq3.Length * valor;
    }

    public string SolucaoParte1(string input)
    {
        input = @"379A";
        int complexidade = 0;

        foreach (string codigo in input.ReplaceLineEndings(Environment.NewLine).Split(Environment.NewLine))
        {
            complexidade += CalculaComplexidade(codigo);
        }

        return complexidade.ToString();
    }

    public string SolucaoParte2(string input)
    {
        throw new NotImplementedException();
    }
}
