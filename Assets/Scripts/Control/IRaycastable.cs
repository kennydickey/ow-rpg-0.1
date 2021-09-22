namespace RPG.Control
{
    public interface IRaycastable
    {
        CursorType GetCursorType(); // When called from PlayerController, method is activated in WeaponPickup and CombatTarget
        // no implementation needed for now, this is just the contract saying this is required
        bool HandleRaycast(PlayerController callingController); // tells us the Playercontroller that is calling this

    }
}