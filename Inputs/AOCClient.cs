using System;
using System.Net;

namespace AOC24.Inputs;

public class AOCClient : IDisposable
{
    private const string envSessao = "aoc24-session";
    private const string baseURI = "https://adventofcode.com/2024/";
    private string pastaCacheInputs;
    private HttpClient httpClient;
    public AOCClient() 
    {
        CookieContainer cookieContainer = new CookieContainer();
        cookieContainer.Add(
            new Uri(baseURI),
            new Cookie("session", Environment.GetEnvironmentVariable(envSessao) ?? throw new ArgumentNullException("PATH:envSessao"))
        );

        HttpClientHandler httpClientHandler= new HttpClientHandler() 
        {
            CookieContainer = cookieContainer
        };

        this.httpClient= new HttpClient(httpClientHandler) 
        {
            BaseAddress = new Uri(baseURI)
        };

        this.pastaCacheInputs = Path.Combine(Directory.GetCurrentDirectory(), "cacheinputs/");
        if (!Directory.Exists(this.pastaCacheInputs)) 
        {
            Directory.CreateDirectory(this.pastaCacheInputs);
        }
    }

    public async Task<string> GetInputAsync(int dia, bool forcaRefreshNoCache = false) 
    {
        string nomeArquivoCache = $"{pastaCacheInputs}/{dia}.txt"; 

        if (File.Exists(nomeArquivoCache) && !forcaRefreshNoCache) 
        {
            return File.ReadAllText(nomeArquivoCache);
        }

        var resposta = await this.httpClient.GetAsync($"day/{dia}/input");

        if (!resposta.IsSuccessStatusCode) 
        {
            throw new Exception($"Falha ao obter o input do dia {dia}");
        }

        string respostaConteudo = await resposta.Content.ReadAsStringAsync();

        File.WriteAllText(nomeArquivoCache, respostaConteudo);

        return respostaConteudo;
    }

    public void Dispose() 
    {
        this.httpClient.Dispose();
    }
}
