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
            int MaxTurns = 25;
            Map GameMap = new Map(Width, Height);
            Console.WriteLine(GameMap.Tiles[5,5].DefenseBonus);
            Player[] Players = new Player[2]; 
            Players[0] = new Player("Player 1", 5, 5);
            Players[1] = new Player("Player 2", 20, 20);
            Player Winner = null;
            // Turn Player1Turn = new Turn(GameMap, Player1, Player2, 1);
            // Console.WriteLine(Player1Turn.Attack);
            // Console.WriteLine(Player1Turn.Action);
            int t;
            for(t=0; Winner==null && t < MaxTurns; t++)
            {
                Console.WriteLine("Turn " + t.ToString());
                for(int p=0; p<Players.Length; p++)
                {
                    // hack assuming only two players
                    int e = 1 - p;
                    Turn PlayerTurn = new Turn(GameMap, Players[p], Players[e], t);
                    // take turn
                    PlayerTurn.Attack();
                    Console.WriteLine("  " + PlayerTurn.Action);
                    if(PlayerTurn.PlayerNote != null) Console.WriteLine("  Player's note: " + PlayerTurn.PlayerNote);
                    if(PlayerTurn.IsEnemyDead)
                    {
                        Winner = Players[p];
                        break;
                    }
                    if(PlayerTurn.IsPlayerDead)
                    {
                        Winner = Players[e];
                        break;
                    }
                }
            }
            Console.WriteLine("\n" + Winner.Name + " wins in " + t.ToString() + (t > 1? " turns\n": " turn\n"));
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
        public bool Attack()
        {
            if(IsEnemyInRange)
            {
                // Temp coin flip for winner
                bool Win = (new Random()).Next(0,2) == 1;
                if(Win)
                {
                    Action = TurnPlayer.Name + " dies attacking " + Enemy.Name;
                    IsPlayerDead = true;
                    IsTurnDone = true;
                    return true;
                }
                else
                {
                    Action = TurnPlayer.Name + " defeats " + Enemy.Name;
                    IsEnemyDead = true;
                    IsTurnDone = true;
                    return false;
                }
            }
            return false;
        }
    }
}
