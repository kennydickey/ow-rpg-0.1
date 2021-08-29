namespace RPG.Saving
{
    public interface ISaveable
    {
        object CaptureState(); // just wants to get state of whatever saveble component
        void RestoreState(object state); // update saveable component with this state
    }
}
