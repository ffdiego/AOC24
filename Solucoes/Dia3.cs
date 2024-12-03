using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC24.Solucoes
{
    public class Dia3 : ISolucionador
    {
        private int Multiplicador(string expressao)
        {
            string apenasNumerosEVirgula = expressao.Replace("mul", "").Replace("(","").Replace(")", "");

            string[] numeros = apenasNumerosEVirgula.Split(",");

            return int.Parse(numeros[0]) * int.Parse(numeros[1]);
        }

        public string SolucaoParte1(string input)
        {
            int acumulador = 0;

            foreach (var expressao in Regex.Matches(input, "mul\\(\\d+,\\d+\\)")) 
            {
                acumulador += Multiplicador(expressao.ToString()!);
            }

            return acumulador.ToString();
        }

        public string SolucaoParte2(string input)
        {
            int acumulador = 0;
            bool deveExecutarMul = true;

            foreach (var expressao in Regex.Matches(input, "(mul\\(\\d+,\\d+\\)|do\\(\\)|don't\\(\\))"))
            {
                if (expressao.ToString() == "don't()")
                {
                    deveExecutarMul = false;
                    continue;
                }

                if (expressao.ToString() == "do()")
                {
                    deveExecutarMul = true;
                    continue;
                }

                if (deveExecutarMul) 
                {
                    acumulador += Multiplicador(expressao.ToString()!);
                }
            }

            return acumulador.ToString();
        }
    }
}
