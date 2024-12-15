﻿using AOC24.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AOC24.Comuns
{
    internal class Mapa
    {
        protected List<List<char>> mapa;
        protected char vazio;
        protected int Altura => this.mapa.Count;
        protected int Largura => this.mapa[0].Count;

        public Mapa(List<List<char>> mapa, char vazio = '.')
        {
            this.mapa = mapa;
            this.vazio = vazio;
        }

        public Mapa(string entrada)
        {
            this.mapa = Parser.MatrizDeChars(entrada);
            this.vazio = '.';
        }

        protected Mapa()
        {
            this.vazio = '.';
        }

        public bool EstaDentroDoMapa(int x, int y) => (y >= 0 && x >= 0 && y < this.mapa.Count && x < this.mapa[y].Count);

        public char GetItem((int x, int y)pos) => GetItem(pos.x, pos.y);
        public char GetItem(int x, int y)
        {
            if (!EstaDentroDoMapa(x, y))
            {
                return default;
            }

            return mapa[y][x];
        }

        public int GetItemInt(int x, int y)
        {
            char c = GetItem(x, y);
            return (c == this.vazio) ? -1 : c & 0b1111;
        }

        public HashSet<(int x, int y)> PegaTodasOcorrenciasDe(char item, bool remove = false)
        {
            HashSet<(int x, int y)> coordenadas = [];

            for (int y = 0; y < this.mapa.Count; y++)
            {
                for (int x = 0; x < this.mapa[0].Count; x++)
                {
                    if (this.mapa[y][x]!.Equals(item))
                    {
                        coordenadas.Add((x, y));

                        if (remove)
                        {
                            this.mapa[y][x] = vazio;
                        }
                    }
                }
            }

            return coordenadas;
        }
    }
}
