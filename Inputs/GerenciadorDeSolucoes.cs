using System;
using AOC24.Inputs;

namespace AOC24.Solucoes;

public class GerenciadorDeSolucoes
{
    private AOCClient client;
    private Dictionary<int, string> Inputs;
    public GerenciadorDeSolucoes(AOCClient client) 
    {
        this.client = client;
        this.Inputs = new();
    }

    public async Task ObtemSolucaoDoDia(int dia) 
    {
        string input = await ObtemInput(dia);

        ISolucionador solucionador = InstanciarSolucao(dia);
        string solucaoParte1 = solucionador.SolucaoParte1(input); 
        string solucaoParte2 = solucionador.SolucaoParte2(input);

        Console.WriteLine($"== Dia {dia:D2} ===");
        Console.WriteLine($"Parte1: {solucaoParte1}");
        Console.WriteLine($"Parte1: {solucaoParte2}");
        Console.WriteLine();
    }

    public async Task<string> ObtemInput(int dia) 
    {
        if (!this.Inputs.TryGetValue(dia, out var input)) 
        {
            string inputNovoParse = await this.client.GetInputAsync(dia);
            this.Inputs.Add(dia, inputNovoParse);
            return inputNovoParse;
        }
        
        return input;
    }

    public int[] DiasComSolucao() 
    {
        List<int> dias = new();
        for (int i = 1; i <= 30; i++) 
        {
            if (GetClasseSolucao(i) != null) 
            {
                dias.Add(i);
            }
        }

        return dias.ToArray();
    }

    private static ISolucionador InstanciarSolucao(int dia)
    {
        Type? type = GetClasseSolucao(dia) ?? throw new ArgumentException($"Nenhuma classe encontrada para o dia {dia}");

        if (Activator.CreateInstance(type) is ISolucionador solucao)
        {
            return solucao;
        }

        throw new InvalidOperationException($"A classe {type} não implementa ISolucao");
    }

    private static Type? GetClasseSolucao(int dia) 
    {   
        string className = $"AOC24.Solucoes.Day{dia}";

        return Type.GetType(className);
    }
}
