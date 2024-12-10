using Microsoft.Extensions.DependencyInjection;
using AOC24.Utils;


internal class Program
{
    private static async Task Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton<AOCClient>()
            .AddSingleton<GerenciadorDeSolucoes>()
            .AddSingleton<Application>()
            .BuildServiceProvider();
        
        var app = serviceProvider.GetRequiredService<Application>();
        await app.Run(args.FirstOrDefault());
    }
}

internal class Application 
{
    private readonly GerenciadorDeSolucoes gerenciadorDeSolucoes;

    public Application(GerenciadorDeSolucoes gerenciadorDeInput) 
    {
        this.gerenciadorDeSolucoes = gerenciadorDeInput;
    }

    public async Task Run(string? arg) 
    {
        Console.WriteLine("AOC'24");
        List<int> diasARodar = [];

        if (string.IsNullOrWhiteSpace(arg)) 
        {
            int diaMaisRecente = this.gerenciadorDeSolucoes.DiasComSolucao().Max();

            diasARodar.Add(diaMaisRecente);
        } 

        else if (arg == "all")
        {
            diasARodar.AddRange(this.gerenciadorDeSolucoes.DiasComSolucao());
        }

        foreach(int dia in diasARodar)
        {
            await this.gerenciadorDeSolucoes.ObtemSolucaoDoDia(dia);
        }
    }

}