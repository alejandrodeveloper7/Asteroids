public interface IPooleableItem
{
    //This interface allows any Gameobject that implement it in a Script be Pooled with the "SimplePool" Script

    bool ReadyToUse
    {
        get;
        set;
    }
}
