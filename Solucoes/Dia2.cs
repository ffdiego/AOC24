using System;
using System.Collections.Generic;

namespace AOC24.Solucoes;

public class Dia2 : ISolucionador
{
    private bool ListaSegura(List<int> lista) 
    {
        bool ehCrescente = lista[0] < lista[1];
        bool seguro = true;

        for (int i = 0; (i < lista.Count() - 1) && seguro; i++)
        {
            int diferenca = lista[i + 1] - lista[i];

            bool NaoRespeitaCrescente = (diferenca > 0) != ehCrescente;
            bool NaoRespeitaLimite = (diferenca == 0 || Math.Abs(diferenca) > 3);

            if (NaoRespeitaCrescente || NaoRespeitaLimite)
            {
                seguro = false;
            }
        }

        return seguro;
    }

    public string SolucaoParte1(string input)
    {
        var listaDeListas = Utils.Parser.ListaDeListaDeInts(input);

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
        var listaDeListas = Utils.Parser.ListaDeListaDeInts(input);

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
