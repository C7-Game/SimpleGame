I started this intending to make a very simple demo game for which I
would make a Lua interface to access the game as an example. But it
got less simple quickly, and although it "plays", adding more features is just more complexity and
effort best spent elsewhere.

I have added a Lua AI functionality. A script defining `function player_turn (turn)` is run. Right now I'm embedding the Lua for dev convenience, but this is proof of concept for user-provided AI (or a handler function for any passed object).

## API

The API works the same in C# and Lua, but in C# method is Pascal case and lua version is camel case. (e.g. `IsTurnDone()` vs `isTurnDone`)

Some of this is useless and was working towards a more complex game. For example, you can get enemy's map coordinates if in range but not your own! The map tiles were to have defensive bonus values, but that feature hasn't been fully implemented.

- Turn is an object with methods to expose to the player or AI to read the situation and take their turn. This object is passed to the `PlayTurn` method (in C#) or `play_turn` global function (in Lua). You can perform only one action per turn.
  - Getters/Setters
    - IsTurnDone(get) - Returns true if no further action can be done on this turn
    - IsPlayerDead(get) - Returns true if current player has died (should only bee seen if player dies attacking)
    - IsEnemyDead(get) - Returns true if the Enemy is dead
    - Action(get) - Returns a string summarizing the action taken by the player during this turn
    - PlayerNote(get or set) - Player can set this string to annotate their turn
    - IsEnemyInRange(get) - Returns true if the enemy is in attack range
    - EnemyX(get) - Returns map X of enemy
    - EnemyY(get) - Returns map Y of enemy
    - EnemyDefense(get) - Returns enemy tile's defense bonus (but this is not yet implemented for combat so it's meaningless; combat is currently a coin toss)
  - Actions
    - Attack() - Attack an enemy (must be in range)
    - Move(x, y) - Move relative coordinates (e.g. -1, 1). Is coded to limit movement to 1 in each axis. Can move to 0,0 and stay put / pass.
