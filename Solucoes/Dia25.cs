using System;
using AOC24.Utils;

namespace AOC24.Solucoes;

public class Dia25 : ISolucionador
{
    internal class ConjuntoChavesFechaduras 
    {
        public HashSet<int[]> fechaduras = [];
        public HashSet<int[]> chaves = [];
        public ConjuntoChavesFechaduras(string input) 
        {
            foreach(var esquemaStr in input.ReplaceLineEndings("\n").Split("\n\n")) 
            {
                bool ehFechadura = esquemaStr.FirstOrDefault() == '#';

                var matrizChars = Parser.MatrizDeChars(esquemaStr)
                    .Skip(1)
                    .Take(5);

                var esquema = Enumerable.Range(0, 5) 
                    .Select(n => matrizChars.Count(l => l[n] == '#'))
                    .ToArray();

                (ehFechadura ? fechaduras : chaves).Add(esquema);
            }
        }

        public long QuantidadeChavesEncaixam()
        {
            long count = 0;

            foreach (var fechadura in fechaduras)
            {
                foreach (var chave in chaves)
                {
                    if (Enumerable.Range(0, 5).All(i => fechadura[i] + chave[i] <= 5))
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }

    public string SolucaoParte1(string input)
    {
        var conjunto = new ConjuntoChavesFechaduras(input);

        return conjunto.QuantidadeChavesEncaixam().ToString();
    }

    public string SolucaoParte2(string input)
    {
        throw new NotImplementedException();
    }
}
