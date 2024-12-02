using System;
using System.Collections.Generic;

namespace AOC24.Solucoes;

public class Day2 : ISolucionador
{
    private readonly int[] diferencasAceitaveis = { 1, 2, 3 };
    private bool ListaSegura(List<int> lista) 
    {
        bool ehCrescente = lista[0] < lista[1];
        bool seguro = true;

        for (int i = 0; (i < lista.Count() - 1) && seguro; i++)
        {
            int diferenca = lista[i + 1] - lista[i];

            bool NaoRespeitaCrescente = (diferenca > 0) != ehCrescente;
            bool NaoRespeitaLimite = !diferencasAceitaveis.Contains(Math.Abs(diferenca));

            if (NaoRespeitaCrescente || NaoRespeitaLimite)
            {
                seguro = false;
            }
        }

        return seguro;
    }

    public string SolucaoParte1(string input)
    {
        var listaDeListas = Utils.Listas.ParseListaDeListas(input);

        int listasSeguras = 0;

        foreach (var lista in listaDeListas)
        {
            if (ListaSegura(lista))
            {
                listasSeguras++;
            } 
        }

        return listasSeguras.ToString();
    }

    public string SolucaoParte2(string input)
    {
        var listaDeListas = Utils.Listas.ParseListaDeListas(input);

        int listasSeguras = 0;

        foreach (var lista in listaDeListas)
        {
            if (ListaSegura(lista))
            {
                listasSeguras++;
            }
            else
            {
                IEnumerable<List<int>> listasComUmElementoAmenos =
                    lista.Select((_, index) =>
                        lista.Where((_, i) => i != index).ToList()
                        );

                if (listasComUmElementoAmenos.Any(l => ListaSegura(l)))
                {
                    listasSeguras++;
                }

            }
        }

        return listasSeguras.ToString();
    }
}
