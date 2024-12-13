using AOC24.Solucoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC24.Utils
{
    internal static class ParsersEspecificos
    {
        public static List<Maquina> CriaMaquinas(string input)
        {
            const string pattern = @"Button A: X\+(?<X1>\d+), Y\+(?<Y1>\d+)\s+Button B: X\+(?<X2>\d+), Y\+(?<Y2>\d+)\s+Prize: X=(?<X3>\d+), Y=(?<Y3>\d+)";
            Regex regex = new Regex(pattern, RegexOptions.Compiled);

            List<Maquina> list = [];

            foreach (string pedaco in input.Split("\n\n"))
            {
                Match match = regex.Match(pedaco);

                if (match.Success)
                {
                    list.Add(new Maquina()
                    {
                        BotaoA = (int.Parse(match.Groups["X1"].Value), int.Parse(match.Groups["Y1"].Value)),
                        BotaoB = (int.Parse(match.Groups["X2"].Value), int.Parse(match.Groups["Y2"].Value)),
                        Premio = (int.Parse(match.Groups["X3"].Value), int.Parse(match.Groups["Y3"].Value)),
                    });
                }
            }

            return list;
        }
    }
}
