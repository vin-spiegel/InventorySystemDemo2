namespace Saving
{
    public interface ISavable
    {
        object CaptureState();
        void RestoreState();
    }
}