using System;

namespace simplegame
{
    class Program
    {
        static void Main(string[] args)
        {
            int Width = 25;
            int Height = 25;
            int MaxTurns = 250;
            Map GameMap = new Map(Width, Height);

            // TODO: Make AI selectable at launch, and allow custom Lua script
            LuaAI Player1LuaAI = new LuaAI(EmbeddedLua.AttackOrMoveRight());
            LuaAI Player2LuaAI = new LuaAI(EmbeddedLua.AttackOrMoveRandom());

            Player[] Players = new Player[2]; 
            Players[0] = new Player("Player 1", 5, 5, ai : Player1LuaAI);
            Players[1] = new Player("Player 2", 20, 20, ai : Player2LuaAI);
            Player Winner = null;
            int t;
            for(t=0; Winner==null && t < MaxTurns; t++)
            {
                Console.WriteLine("Turn " + t.ToString());
                for(int p=0; p<Players.Length; p++)
                {
                    // hack assuming only two players
                    int e = 1 - p;
                    Player Enemy = null;
                    if(Math.Abs(Players[p].X - Players[e].X) < 2 && Math.Abs(Players[p].Y - Players[e].Y) < 2)
                    {
                        Enemy = Players[e];
                    }
                    Turn PlayerTurn = new Turn(GameMap, Players[p], Enemy, t);
                    // take turn
                        Players[p].AI.PlayTurn(PlayerTurn);

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
            if(Winner!=null)
            {
                Console.WriteLine("\n" + Winner.Name + " wins in " + t.ToString() + (t > 1? " turns\n": " turn\n"));
            }
            else
            {
                Console.WriteLine(t.ToString() + " turns with no winner");
            }
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
            if(!IsTurnDone && IsEnemyInRange)
            {
                // Temp coin flip for winner
                bool Win = (new Random()).Next(0,2) == 1;
                if(Win)
                {
                    Action = TurnPlayer.Name + " defeats " + Enemy.Name;
                    IsEnemyDead = true;
                    IsTurnDone = true;
                    return true;
                }
                else
                {
                    Action = TurnPlayer.Name + " dies attacking " + Enemy.Name;
                    IsPlayerDead = true;
                    IsTurnDone = true;
                    return false;
                }
            }
            return false;
        }
        public int EnemyX
        { get{
            if(IsEnemyInRange)
            {
                return Enemy.X;
            }
            return -1;
        }}
        public int EnemyY
        { get{
            if(IsEnemyInRange)
            {
                return Enemy.Y;
            }
            return -1;
        }}
        public int EnemyDefense{ get => GameMap.Tiles[Enemy.X, Enemy.Y].DefenseBonus; }
        public void Move(int x, int y)
        {
            if(!IsTurnDone)
            {
                // Allow only one move point in each axis
                x = x == 0 ? 0 : x / Math.Abs(x);
                y = y == 0 ? 0 : y / Math.Abs(y);
                TurnPlayer.X = Mod((TurnPlayer.X + x), GameMap.Width);
                TurnPlayer.Y = Mod((TurnPlayer.Y + y), GameMap.Height);
                IsTurnDone = true;
                Action = TurnPlayer.Name + " moves to " + TurnPlayer.X.ToString() +", " + TurnPlayer.Y.ToString();
            }
        }
        private int Mod(int n, int m) => ((n % m) + m) % m;
    }
}
