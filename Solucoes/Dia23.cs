using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC24.Solucoes
{
    internal class Computador
    {
        public static HashSet<Computador> TodosComputadores = [];
        public override int GetHashCode()
        {
            return this.Nome.GetHashCode();
        }

        public string Nome { get; private set; }
        public List<Computador> Conexoes { get; private set; }

        private Computador(string nome)
        {
            this.Nome = nome;
            this.Conexoes = [];
            Computador.TodosComputadores.Add(this);
        }

        private bool AdicionaConexao(Computador computador)
        {
            //if (this.Conexoes.Count > 1)
            //{
            //    return false;
            //}

            this.Conexoes.Add(computador);

            return true;
        }

        public static void AdicionaComputadores(string input)
        {
            string[] nomes = input.Split("-");
            List<Computador> computadores = [];

            foreach (string nome in nomes)
            {
                Computador computador = TodosComputadores.FirstOrDefault(c => c.Nome == nome) ?? new Computador(nome);
                computadores.Add(computador);
            }
            
            foreach (Computador computador in computadores)
            {
                foreach (Computador conexao in computadores)
                {
                    if (conexao != computador)
                    {
                        computador.AdicionaConexao(conexao);
                    }
                }
            }
        }
    }

    internal class Dia23 : ISolucionador
    {
        public string SolucaoParte1(string input)
        {
            foreach (string linha in input.Split(Environment.NewLine))
            {
                Computador.AdicionaComputadores(linha);
            }

            return Computador.TodosComputadores.Count(c => c.Conexoes.Count >= 2).ToString();
        }

        public string SolucaoParte2(string input)
        {
            throw new NotImplementedException();
        }
    }
}
