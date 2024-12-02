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

        if (string.IsNullOrWhiteSpace(arg)) 
        {
            foreach(int dia in this.gerenciadorDeSolucoes.DiasComSolucao()) 
            {
                await this.gerenciadorDeSolucoes.ObtemSolucaoDoDia(dia);
            }
            return;
        } 

        int diaArg = int.Parse(arg);
        await this.gerenciadorDeSolucoes.ObtemSolucaoDoDia(diaArg);
    }

}