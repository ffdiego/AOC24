using Microsoft.Extensions.DependencyInjection;
using AOC24.Inputs;
using AOC24.Solucoes;


internal class Program
{
    private static void Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton<AOCClient>()
            .AddSingleton<GerenciadorDeSolucoes>()
            .AddSingleton<Application>()
            .BuildServiceProvider();
        
        var app = serviceProvider.GetRequiredService<Application>();
        app.Run(args[0]);
    }
}

internal class Application 
{
    private readonly GerenciadorDeSolucoes gerenciadorDeSolucoes;

    public Application(GerenciadorDeSolucoes gerenciadorDeInput) 
    {
        this.gerenciadorDeSolucoes = gerenciadorDeInput;
    }

    public void Run(string arg) 
    {
        Console.WriteLine("AOC'24");

        if (string.IsNullOrWhiteSpace(arg)) 
        {
            foreach(int dia in this.gerenciadorDeSolucoes.DiasComSolucao()) 
            {
                this.gerenciadorDeSolucoes.ObtemSolucaoDoDia(dia).GetAwaiter().GetResult();
            }
            return;
        } 

        int diaArg = int.Parse(arg);
        this.gerenciadorDeSolucoes.ObtemSolucaoDoDia(diaArg).GetAwaiter().GetResult();
    }

}