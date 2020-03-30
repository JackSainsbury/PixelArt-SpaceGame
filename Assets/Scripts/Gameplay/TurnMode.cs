public enum TurnMode
{
    OnPlayerSpace,  // Wait for player to press space to pause/unpause time
    Timer,          // Timer ticks, units have a "max traverse distance" as a result and decisions must be made within timer
    NoTurn          // Game plays in realtime (no pause time) units follow commands immediately
}
