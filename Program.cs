using System;

namespace simplegame
{
    class Program
    {
        // public readonly Player Player1;
        // public readonly Player Player2;
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            int Width = 25;
            int Height = 25;
            Map GameMap = new Map(Width, Height);
            Console.WriteLine(GameMap.Tiles[5,5].DefenseBonus);

        }
    }
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
        public Player(string name, int x, int y, int attack = 1, int defense = 1)
        {
            Name = name;
            X = x;
            Y = y;
            Attack = attack;
            Defense = defense;
        }
    }
    class Turn
    {
        public readonly int TurnNumber;
        private Map GameMap;
        public bool IsTurnDone{ get; private set; }
        public bool IsPlayerDead{ get; private set; }
        public bool IsEnemyDead{ get; private set; }
        public string Action{ get; private set; }
        public string PlayerNote;
        private Player TurnPlayer;
        private Player Enemy;
        public Turn(Map map, Player player, Player enemy, int turnNum)
        {
            TurnPlayer = player;
            GameMap = map;
            Enemy = enemy;
            TurnNumber = turnNum;
            IsPlayerDead = false;
            IsEnemyDead = false;
            IsTurnDone = false;
        }
        public bool IsEnemyInRange{ get => Enemy != null; }
        public bool Attack
        { get {
            if(IsEnemyInRange)
            {
                IsTurnDone = true;
            }
            return false;
        }}
    }
}
