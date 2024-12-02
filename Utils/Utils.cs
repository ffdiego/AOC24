using System;

namespace AOC24.Utils;

public static class Listas
{
    public static (List<int>, List<int>) ParseLists(string file)
    {
        (List<int>, List<int>) listas;
        listas = (new List<int>(), new List<int>());

        foreach (string line in file.Split('\n'))
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

}
