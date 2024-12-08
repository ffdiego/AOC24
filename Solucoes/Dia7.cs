using AOC24.Utils;

namespace AOC24.Solucoes
{
    internal interface IEquacao
    {
        bool EhEquacaoValida();
        void SetaLinha(string linha);
        ulong Esperado { get; }
    }

    internal class Equacao3Operacoes : Equacao
    {
        public Equacao3Operacoes()
        {
            this.operacoesMatematicas =
            [
                (ulong a, ulong b) => a + b,
                (ulong a, ulong b) => a * b,
                (ulong a, ulong b) => (ulong.Parse(a.ToString() + b.ToString()))
            ];
        }
    }
    internal class Equacao : IEquacao
    {
        public ulong Esperado { get; private set; }
        private List<ulong> partes = new();

        protected List<Func<ulong, ulong, ulong>> operacoesMatematicas;

        private bool ExecutaTeste(List<Func<ulong, ulong, ulong>> funcoes)
        {
            var fila = new Queue<ulong>(partes);
            var stack = new Stack<Func<ulong, ulong, ulong>>(funcoes);

            ulong resultante = fila.Dequeue();

            while (stack.Count > 0)
            {
                var funcao = stack.Pop();
                resultante = funcao(resultante, fila.Dequeue());
            }

            return resultante == this.Esperado;
        }

        public bool EhEquacaoValida()
        {
            return EhEquacaoValidaDFS(new List<Func<ulong, ulong, ulong>>(), 0);
        }

        private bool EhEquacaoValidaDFS(List<Func<ulong, ulong, ulong>> listaFnc, int profundidade)
        {
            if (profundidade == partes.Count - 1)
            {
                return ExecutaTeste(listaFnc);
            }

            int i = 0;
            bool encontrou = false;
            do
            {
                var novaLista = listaFnc.ToList();
                novaLista.Add(operacoesMatematicas[i++]);

                encontrou = EhEquacaoValidaDFS(novaLista, profundidade + 1);
            } while (!encontrou && i < this.operacoesMatematicas.Count);

            return encontrou;
        }

        public void SetaLinha(string linha)
        {
            var linhapartes = linha.Split(": ");
            this.Esperado = ulong.Parse(linhapartes[0]);
            this.partes = linhapartes[1].Split(" ").Select(s => ulong.Parse(s)).ToList();
        }

        public Equacao()
        {
            this.operacoesMatematicas =
            [
                (ulong a, ulong b) => a + b,
                (ulong a, ulong b) => a * b,
            ];
        }
    }

    internal class Dia7 : ISolucionador
    {
        private string Soluciona<T>(string input) where T : IEquacao, new()
        {
            List<string> linhas = Parser.LinhasDeTexto(input);

            long count = 0;

            Parallel.ForEach(linhas, linha =>
            {
                T equacao = new();
                equacao.SetaLinha(linha);

                if (equacao.EhEquacaoValida())
                {
                    long valor = (long)equacao.Esperado;
                    Interlocked.Add(ref count, valor);
                }
            });

            return count.ToString();
        }

        public string SolucaoParte1(string input)
        {
            return Soluciona<Equacao>(input);
        }

        public string SolucaoParte2(string input)
        {
            return Soluciona<Equacao3Operacoes>(input);
        }
    }
}
