using Sharp7;
internal abstract class Stan
{
    internal string Name;
    internal string IpAddress;
    internal int Counter;
    internal bool Status;
    internal bool WireBreak;
    internal bool DrawingChange;
    internal bool CointerErase;

    public abstract bool getData();
}
