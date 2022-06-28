using Sharp7;
internal abstract class Stan
{
    public string Name;
    public string IpAddress;
    internal int Counter;
    internal bool Status;
    internal bool WireBreak;
    internal bool DrawingChange;
    internal bool CointerErase;

    public abstract bool getData();
}
