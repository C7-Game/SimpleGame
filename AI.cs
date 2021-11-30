using System;
using MoonSharp.Interpreter;

namespace simplegame
{
    interface IAI {
        void PlayTurn(Turn turn);
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
    // Inherits MoonSharp.Interpreter.Script and sandboxes itself
    //   constructor parameter script should be a Lua script that defines function player_turn accepting a Turn as parameter
    class LuaAI : Script, IAI {
        public LuaAI(string script) : base(CoreModules.Preset_HardSandbox) {
            UserData.RegisterType<Turn>();
            DoString(script);
        }
        public void PlayTurn(Turn turn) {
            DynValue _ = Call(Globals["player_turn"], turn);
        }
    }
}
