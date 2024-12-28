using System;
using System.Runtime.Intrinsics.X86;
using AOC24.Comuns;

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
        public Dictionary<(char partida, char destino), Direcao[]> caminhosOtimizados;

        public Keypad(string input) : base(input)
        {
            caminhosOtimizados = MapeiaMenoresDistanciasEntreBotoes();
        }

        public string DigitaSequencia(string sequencia)
        {
            char botaoAtual = botaoA;

            List<char> botoesNecessarios = [];
            
            foreach (char tecla in sequencia)
            {
                botoesNecessarios.AddRange(CaminhoEntreBotoes(botaoAtual, tecla));
                botaoAtual = tecla;
            }

            return string.Concat(botoesNecessarios.ToArray());
        }

        private IEnumerable<char> CaminhoEntreBotoes(char botaoAtual, char botaoDestino)
        {
            Direcao[] passos = this.caminhosOtimizados[(botaoAtual, botaoDestino)];
            IEnumerable<char> setas = passos.Select(p => p.ParaChar()).Append(botaoA);

            return setas;
        }

        private Direcao[] EncontraMelhorCaminho(char botaoPartida, char botaoDestino) 
        {
            Posicao posicaoInicial = this.PegaTodasOcorrenciasDe(botaoPartida).Single();

            HashSet<Posicao> visitados = [];
            PriorityQueue<(Posicao, List<Direcao>, int custo), int> lugaresPravisitar = new();
            lugaresPravisitar.Enqueue((posicaoInicial, [], 0), 0);

            while (lugaresPravisitar.Count > 0)
            {
                (Posicao posicao, List<Direcao> caminho, int custo) = lugaresPravisitar.Dequeue();

                if (GetItem(posicao) == botaoDestino) 
                {
                    return [.. caminho];
                }

                foreach (Direcao direcao in DirecaoUtils.Direcoes) 
                {
                    Posicao nova = direcao.PosicaoAFrente(posicao);
                    char botaoNaPosicao = this.GetItem(nova);

                    if (botaoNaPosicao != default && botaoNaPosicao != this.vazio && visitados.Add(nova)) 
                    {
                        List<Direcao> novoCaminho = [.. caminho];
                        novoCaminho.Add(direcao);
                        lugaresPravisitar.Enqueue((nova, novoCaminho, custo + 1), custo + 1);
                    }
                }  
            }

            return [];
        }
        private Dictionary<(char partida, char destino), Direcao[]> MapeiaMenoresDistanciasEntreBotoes() 
        {
            Dictionary<(char partida, char destino), Direcao[]> caminhosOtimizados = [];

            IEnumerable<char> botoes = this.mapa
                .SelectMany(x => x.Distinct())
                .Where(x => x != vazio);

            foreach (char botaoPartida in botoes) 
            {
                foreach (char botaoDestino in botoes.Where(b => b != botaoPartida)) 
                {
                    caminhosOtimizados.Add((botaoPartida, botaoDestino), EncontraMelhorCaminho(botaoPartida, botaoDestino));
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
        var seq1 = numpad.DigitaSequencia(codigo);
        var seq2 = dirpad.DigitaSequencia(seq1);
        var seq3 = dirpad.DigitaSequencia(seq2);

        int valor = int.Parse(codigo.Replace("A", ""));
        Console.WriteLine($"{seq3.Length} * {valor} = {seq3.Length * valor}");

        return seq3.Length * valor;
    }

    public string SolucaoParte1(string input)
    {
        input = @"029A
980A
179A
456A
379A";
        int complexidade = 0;

        foreach (string codigo in input.Split(Environment.NewLine))
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
