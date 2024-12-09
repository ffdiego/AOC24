using AOC24.Utils;

namespace AOC24.Solucoes
{
    internal class Dia8 : ISolucionador
    {
        internal class Mapa
        {
            private List<List<char>> mapa;
            private Dictionary<char, List<Antena>> antenas;

            private void AdicionaAntena(Antena antena)
            {
                if (!antenas.TryGetValue(antena.Frequencia, out var lista))
                {
                    antenas.Add(antena.Frequencia, [antena]);
                }
                else
                {
                    lista.Add(antena);
                }
            }

            protected bool EstaDentroDoMapa((int x, int y) pos) => (pos.y >= 0 && pos.x >= 0 && pos.y < this.mapa.Count && pos.x < this.mapa[pos.y].Count);

            protected virtual void ObtemAntinosEntreDuasAntenas(HashSet<(int x, int y)> antinos, Antena emissora, Antena oposta)
            {
                int dx = emissora.X - oposta.X;
                int dy = emissora.Y - oposta.Y;
                (int x, int y) antino = (emissora.X + dx, emissora.Y + dy);

                if (this.EstaDentroDoMapa(antino))
                {
                    antinos.Add(antino);
                }
            }

            public Mapa(string input)
            {
                this.mapa = Parser.MatrizDeChars(input);
                this.antenas = [];

                for (int y = 0; y < this.mapa.Count; y++)
                {
                    for (int x = 0; x < this.mapa[y].Count; x++)
                    {
                        char c = this.mapa[y][x];
                        if (c != '.')
                        {
                            Antena antena = new Antena(x, y, c);
                            this.AdicionaAntena(antena);
                        }
                    }
                }
            }

            public int CalculaQuantidadeAntiNos()
            {
                HashSet<(int x, int y)> antinos = [];
                foreach (var frequencia in this.antenas.Keys)
                {
                    var listaAntenasFrequencia = this.antenas[frequencia];

                    for (int i = 0; i < listaAntenasFrequencia.Count; i++)
                    {
                        Antena emissora = listaAntenasFrequencia[i];

                        var outrasAntenas = listaAntenasFrequencia.Where((_, j) => j != i);

                        foreach (Antena oposta in outrasAntenas)
                        {
                            ObtemAntinosEntreDuasAntenas(antinos, emissora, oposta);
                        }
                    }
                }

                foreach (var antino in antinos)
                {
                    this.mapa[antino.y][antino.x] = '#';
                }

                string teishto = Parser.MatrizDeChars(this.mapa);

                return antinos.Count();
            }
        }

        internal class MapaHarmonicosRessonantes : Mapa
        {
            public MapaHarmonicosRessonantes(string input) : base(input) { }

            protected override void ObtemAntinosEntreDuasAntenas(HashSet<(int x, int y)> antinos, Antena emissora, Antena oposta)
            {
                antinos.Add((emissora.X, emissora.Y));

                int dx = emissora.X - oposta.X;
                int dy = emissora.Y - oposta.Y;

                (int x, int y) antino = (emissora.X + dx, emissora.Y + dy);

                while (EstaDentroDoMapa(antino))
                {
                    antinos.Add(antino);
                    antino.x += dx;
                    antino.y += dy;
                }
            }
        }

        internal class Antena
        {
            public char Frequencia { get; private set; }
            public int X { get; private set; }
            public int Y { get; private set; }

            public Antena(int x, int y, char frequencia)
            {
                this.X = x;
                this.Y = y;
                this.Frequencia = frequencia;
            }
        }

        public string SolucaoParte1(string input)
        {
            Mapa mapa = new(input);

            return mapa.CalculaQuantidadeAntiNos().ToString();
        }

        public string SolucaoParte2(string input)
        {
            MapaHarmonicosRessonantes mapa = new(input);

            return mapa.CalculaQuantidadeAntiNos().ToString();
        }
    }
}
