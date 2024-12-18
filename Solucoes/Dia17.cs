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
            private readonly int[] programa;
            private readonly int a_inicial;
            private readonly int b_inicial;
            private readonly int c_inicial;


            private int a;
            private int b;
            private int c;
            private int ip;

            private bool jumped;

            private int op => programa[ip + 1];
            private string saida;

            private Action GetFuncao()
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

            private int GetCombo() 
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
                a = a >> GetCombo();
            }

            private void bxl_1()
            {
                b = b ^ op;
            }

            private void bst_2()
            {
                b = GetCombo() & 0b111;
            }

            private void jnz_3()
            {
                if (a == 0) return;
                ip = op;
                jumped = true;
            }

            private void bxc_4()
            {
                b = b ^ c;
            }

            private void out_5()
            {
                string saidaFuncao = (GetCombo() % 8).ToString();
                this.saida += saidaFuncao;
            }

            private void bdv_6()
            {
                b = a >> GetCombo();
            }

            private void cdv_7()
            {
                c = a >> GetCombo();
            }

            private void Reset()
            {
                this.saida = string.Empty;
                this.a = a_inicial;
                this.b = b_inicial;
                this.c = c_inicial;
                this.ip = 0;
                this.jumped = false;
            }

            public string Run()
            {
                Reset();
                while (true)
                {
                    if (ip >= (programa.Count() - 1)) break;

                    Action funcao = this.GetFuncao();
                    funcao.Invoke();

                    if (jumped) 
                    { 
                        jumped = false; 
                    } 
                    else
                    {
                        ip += 2;
                    }
                }

                return string.Join(",", saida.ToCharArray());
            }

            public CPU(string txt)
            {
                string[] linhas = txt.ReplaceLineEndings("\n").Split("\n");
                this.saida = string.Empty;

                a_inicial = int.Parse(linhas[0].Substring(12));
                b_inicial = int.Parse(linhas[1].Substring(12));
                c_inicial = int.Parse(linhas[2].Substring(12));
                programa = linhas[4].Substring(9).Split(",").Select(int.Parse).ToArray();
            }
        }

        public string SolucaoParte1(string input)
        {
            CPU cpu = new(input);


            return cpu.Run();
        }

        public string SolucaoParte2(string input)
        {
            throw new NotImplementedException();
        }
    }
}
