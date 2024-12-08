using AOC24.Utils;

namespace AOC24.Solucoes;

public class Dia1 : ISolucionador
{
    public string SolucaoParte1(string input)
    {
        var listas = Parser.ListaDuplaInts(input);

        return SomaDasDistancias(listas.Item1, listas.Item2).ToString();
    }

    public string SolucaoParte2(string input)
    {
        var listas = Parser.ListaDuplaInts(input);

        return Similaridade(listas.Item1, listas.Item2).ToString();
    }

    private (List<int>, List<int>) OrdenaListas(IEnumerable<int> a, IEnumerable<int> b) => 
        (a.OrderBy(a => a).ToList(), b.OrderBy(b => b).ToList());

    private int SomaDasDistancias(List<int> a, List<int> b)
    {
        var listasOrdenadas = OrdenaListas(a, b);

        return listasOrdenadas.Item1
            .Zip(listasOrdenadas.Item2, 
                (x, y) => Math.Abs(x - y))
            .Sum();
    }

    private int Similaridade(List<int> a, List<int> b) 
    {
        return a
            .Select(a => a * b.Count(b => b == a))
            .Sum();
    } 
}
