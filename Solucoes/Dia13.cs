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
        private const long ADICIONAL_PARTE2 = 10000000000000;
        private const int LIMITE_POR_BOTAO = 100; // isso aqui não ocorre no puzzle :)
        private const int PRECO_BOTAO_A = 3;
        private const int PRECO_BOTAO_B = 1;

        public (int x, int y) A;
        public (int x, int y) B;
        public (int x, int y) Premio;

        public long Soluciona(bool parte2 = false)
        {
            bool EhInteiro(double n) => n % 1 == 0;

            (long x, long y) premio = parte2 ? (Premio.x + ADICIONAL_PARTE2, Premio.y + ADICIONAL_PARTE2) : Premio;
 
            double pressionadasBotaoA = (double)(premio.x * B.y - premio.y * B.x) / (A.x * B.y - A.y * B.x);
            double pressionadasBotaoB = (double)(premio.x - A.x * pressionadasBotaoA) / (B.x);           

            if (!EhInteiro(pressionadasBotaoA) || 
                !EhInteiro(pressionadasBotaoB))
            {
                return 0;
            }

            return (long)pressionadasBotaoA * PRECO_BOTAO_A + (long)pressionadasBotaoB * PRECO_BOTAO_B;
        }
    }

    internal class Dia13 : ISolucionador
    {

        public string SolucaoParte1(string input)
        {
            List<Maquina> maquinas = ParsersEspecificos.CriaMaquinas(input);

            long solucao = maquinas.Sum(m => m.Soluciona());
            
            return solucao.ToString();
        }

        public string SolucaoParte2(string input)
        {
            List<Maquina> maquinas = ParsersEspecificos.CriaMaquinas(input);

            long solucao = maquinas.Sum(m => m.Soluciona(true));

            return solucao.ToString();
        }
    }
}
