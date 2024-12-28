using System;
using AOC24.Comuns;

namespace AOC24.Solucoes;

public class Dia21 : ISolucionador
{
    internal class Keypad : Mapa 
    {
        public Keypad(string input) : base(input)
        {
            MapeiaMenoresDistanciasEntreBotoes();
        }

        private int EncontraMelhorCaminho(char botaoPartida, char botaoDestino) 
        {
            Posicao posicaoInicial = this.PegaTodasOcorrenciasDe(botaoPartida).Single();

            HashSet<Posicao> visitados = [];
            PriorityQueue<(Posicao, int custo), int> lugaresPravisitar = new();
            lugaresPravisitar.Enqueue((posicaoInicial, 0), 0);

            while (lugaresPravisitar.Count > 0)
            {
                (Posicao posicao, int custo) = lugaresPravisitar.Dequeue();

                if (GetItem(posicao) == botaoDestino) 
                {
                    return custo;
                }

                foreach (Direcao direcao in DirecaoUtils.Direcoes) 
                {
                    Posicao nova = direcao.PosicaoAFrente(posicao);

                    if (this.GetItem(nova) != default && visitados.Add(nova)) 
                    {
                        lugaresPravisitar.Enqueue((nova, custo + 1), custo + 1);
                    }
                }  
            }

            return -1;
        }

        private void MapeiaMenoresDistanciasEntreBotoes() 
        {
            Dictionary<(char partida, char destino), int> caminhosOtimizados = [];

            IEnumerable<char> botoes = this.mapa
                .SelectMany(x => x.Distinct())
                .Where(x => x != vazio);

            foreach (char botaoPartida in botoes) 
            {
                foreach (char botaoDestino in botoes.Where(b => b != botaoPartida)) 
                {
                    caminhosOtimizados.Add((botaoPartida, botaoDestino), EncontraMelhorCaminho(botaoPartida, botaoDestino));
                }
            }
        }
    }
    public string SolucaoParte1(string input)
    {
        throw new NotImplementedException();
    }

    public string SolucaoParte2(string input)
    {
        throw new NotImplementedException();
    }
}
