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
}
