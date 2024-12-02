using System;

namespace AOC24.Utils;

public static class Listas
{
    public static (List<int>, List<int>) ParseListDuplaInts(string txt)
    {
        (List<int>, List<int>) listas;
        listas = (new List<int>(), new List<int>());

        foreach (string line in txt.Split('\n'))
        {
            try
            {
                string[] inteiros = line.Split("   ");
                listas.Item1.Add(int.Parse(inteiros[0]));
                listas.Item2.Add(int.Parse(inteiros[1]));
            }
            catch (Exception) {}
        }

        return listas;
    }

    public static List<List<int>> ParseListaDeListas(string txt) 
    {
        List<List<int>> listaDeListas = new();

        foreach (string line in txt.Split('\n'))
        {
            try
            {
                List<int> lista = line.Split(" ").Select(a => int.Parse(a)).ToList();
                listaDeListas.Add(lista);
            }
            catch (Exception) {}
        }

        return listaDeListas;
    }

}
