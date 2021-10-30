using System;

namespace simplegame
{
    // Dev-time hack to provide Lua scripts; ideally scripts would be read from a user submitted file or similar
    class EmbeddedLua {
        static public string DoNothing() => @"function player_turn(turn) turn.move(0,0) end";
        static public string AttackOrMoveRight() => @"
        function player_turn(turn)
            if turn.isEnemyInRange == true then
                turn.attack()
            else
                turn.move(1,0)
            end
        end";
        static public string AttackOrMoveRandom() => @"
        function player_turn(turn)
            if turn.isEnemyInRange == true then
                turn.attack()
            else
                local x = math.random(-1,1)
                local y = math.random(-1,1)
                turn.move(x,y)
            end
        end";
    }
}
