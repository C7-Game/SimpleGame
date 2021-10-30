using System;

namespace simplegame
{
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
