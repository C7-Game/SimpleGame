using System;

namespace simplegame
{
    interface IAI {
        public void PlayTurn(Turn turn) {}
    }
    class SimpleAI : IAI {
        public void PlayTurn(Turn turn)
        {
            if(turn.IsEnemyInRange)
            {
                bool Result = turn.Attack();
                if(Result)
                {
                    turn.PlayerNote = "Woo! PWNED!";
                }
                else
                {
                    turn.PlayerNote = "Lame!";
                }
            }
            else
            {
                Random Rng = new Random();
                turn.Move(1 - Rng.Next(0,3), 1 - Rng.Next(0,3));
                // turn.PlayerNote = "Where are they?";
            }
        }
    }
}
