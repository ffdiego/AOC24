using System;
using System.Diagnostics;
using AOC24.Solucoes;

namespace AOC24.Utils;

public class GerenciadorDeSolucoes
{
    private AOCClient client;
    private Dictionary<int, string> Inputs;
    public GerenciadorDeSolucoes(AOCClient client)
    {
        this.client = client;
        Inputs = new();
    }

    public async Task ObtemSolucaoDoDia(int dia)
    {
        string input = await ObtemInput(dia);

        ISolucionador solucionador = InstanciarSolucao(dia);

        Stopwatch swParte1 = Stopwatch.StartNew();
        string solucaoParte1 = solucionador.SolucaoParte1(input);
        swParte1.Stop();

        Stopwatch swParte2 = Stopwatch.StartNew();
        string solucaoParte2 = solucionador.SolucaoParte2(input);
        swParte2.Stop();

        Console.WriteLine($"=== Dia {dia:D2} ===");
        Console.WriteLine($"Parte1: {solucaoParte1} ({swParte1.ElapsedMilliseconds}ms)");
        Console.WriteLine($"Parte2: {solucaoParte2} ({swParte2.ElapsedMilliseconds}ms)");
        Console.WriteLine();
    }

    public async Task<string> ObtemInput(int dia)
    {
        if (!Inputs.TryGetValue(dia, out var input))
        {
            string inputNovoParse = await client.GetInputAsync(dia);
            Inputs.Add(dia, inputNovoParse);
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
        string className = $"AOC24.Solucoes.Dia{dia}";

        return Type.GetType(className);
    }
}
