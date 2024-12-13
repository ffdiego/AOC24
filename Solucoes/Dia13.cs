using AOC24.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC24.Solucoes
{
    internal class Maquina()
    {
        public const int LIMITE_POR_BOTAO = 100;
        public const int PRECO_BOTAO_A = 3;
        public const int PRECO_BOTAO_B = 1;

        public (int x, int y) BotaoA;
        public (int x, int y) BotaoB;
        public (int x, int y) Premio;

        private bool PassouDoPremio((int x, int y)pos)
        {
            return pos.x > Premio.x || pos.y > Premio.y;
        } 

        public int Soluciona()
        {
            Dictionary<(int a, int b), (int, int)> cache = [];
            bool premioMultiploEmX = true;
            bool premioMultiploEmY = true;
            if (!premioMultiploEmX || !premioMultiploEmY)
            {
                return 0;
            }

            (int x, int y) resultante = (0, 0);
            for (int a = 0; a <= LIMITE_POR_BOTAO; a++)
            {
                for (int b = 0; b <= LIMITE_POR_BOTAO; b++)
                {
                    resultante = (BotaoA.x * a + BotaoB.x * b, BotaoA.y * a + BotaoB.y * b);

                    if (PassouDoPremio(resultante))
                    {
                        break;
                    }

                    cache.Add((a, b), resultante);
                }
            }



            return cache.Count(c => c.Value == this.Premio);
        }
    }

    internal class Dia13 : ISolucionador
    {


        public string SolucaoParte1(string input)
        {
            List<Maquina> maquinas = ParsersEspecificos.CriaMaquinas(input);
            
            return maquinas[0].Soluciona().ToString();
        }

        public string SolucaoParte2(string input)
        {
            return "0";
        }
    }
}
