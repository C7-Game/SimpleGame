using System;

namespace simplegame
{
    class Map
    {
        public readonly int Width;
        public readonly int Height;
        public readonly Tile[,] Tiles;
        public Map(int width = 25, int height = 25)
        {
            Width = width;
            Height = height;
            Tiles = new Tile[Width,Height];
            for(int x=0; x < Width; x++)
            {
                for(int y=0; y < Width; y++)
                {
                    Tiles[x,y] = new Tile();
                }
            }
        }
    }
    class Tile
    {
        public int DefenseBonus{ get; private set; }
        private int[] BonusTable = new int[]{ 10, 25, 50, 100 };
        public Tile()
        {
            DefenseBonus = BonusTable[(new Random()).Next(0,BonusTable.Length)];
        }
    }
    class Player
    {
        public int X;
        public int Y;
        public int Attack;
        public int Defense;
        public readonly string Name;
        public IAI AI;
        public Player(string name, int x, int y, int attack = 1, int defense = 1, IAI ai = null)
        {
            Name = name;
            X = x;
            Y = y;
            Attack = attack;
            Defense = defense;
            if (ai == null) { AI = new SimpleAI(); }
            else { AI = ai; }
        }
    }
}
