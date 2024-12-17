using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC24.Solucoes
{
    internal class Dia17 : ISolucionador
    {
        internal class CPU
        {
            private int a;
            private int b;
            private int c;
            private int ip;
            private int[] programa;

            private int op => programa[ip + 1];
            private int combo => Combo();
            private Action funcao => Funcao();
            private string saida;

            private Action Funcao()
            {
                switch (programa[ip])
                {
                    case 0:
                        return this.adv_0;
                    case 1:
                        return this.bxl_1;
                    case 2:
                        return this.bst_2;
                    case 3:
                        return this.jnz_3;
                    case 4:
                        return this.bxc_4;
                    case 5:
                        return this.out_5;
                    case 6:
                        return this.bdv_6;
                    case 7:
                        return this.cdv_7;
                    default:
                        throw new ArgumentOutOfRangeException(); 
                }
            }

            private int Combo() 
            {
                switch (op)
                {
                    case 1:
                    case 2:
                    case 3:
                        return op;
                    case 4:
                        return a;
                    case 5:
                        return b;
                    case 6:
                        return c;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            private void adv_0()
            {
                a = a >> combo;
            }

            private void bxl_1()
            {
                b = b ^ op;
            }

            private void bst_2()
            {
                b = combo & 0b111;
            }

            private void jnz_3()
            {
                if (a == 0) return;
                ip = op;
            }

            private void bxc_4()
            {
                b = b ^ c;
            }

            private void out_5()
            {
                this.saida += (combo % 8).ToString();
            }

            private void bdv_6()
            {
                b = a >> combo;
            }

            private void cdv_7()
            {
                c = a >> combo;
            }

            public string Run()
            {
                this.saida = string.Empty;
                while (true)
                {
                    if (ip >= (programa.Count() - 1)) break;

                    Action funcaoARodar = this.funcao;
                    this.funcao.Invoke();

                    if (funcaoARodar != this.jnz_3) ip += 2;
                }

                return string.Join(",", saida.ToCharArray());
            }

            public CPU(string txt)
            {
                string[] linhas = txt.ReplaceLineEndings("\n").Split("\n");
                ip = 0;

                a = int.Parse(linhas[0].Substring(12));
                b = int.Parse(linhas[1].Substring(12));
                c = int.Parse(linhas[2].Substring(12));
                ip = 0;
                saida = string.Empty;
                programa = linhas[4].Substring(9).Split(",").Select(n => int.Parse(n)).ToArray();
            }
        }

        public string SolucaoParte1(string input)
        {
            input = @"Register A: 729
Register B: 0
Register C: 0

Program: 0,1,5,4,3,0"
.ReplaceLineEndings("\n");

            CPU cpu = new(input);


            return cpu.Run();
        }

        public string SolucaoParte2(string input)
        {
            throw new NotImplementedException();
        }
    }
}
