using Sharp7;
internal class Stan
{

    private string Name;
    private string IpAddress;
    private int Counter;
    private bool Status;
    private bool WireBreak = false;
    private bool DrawingChange = false;

    public Stan(string name, string ipAddress, int counter = 0, bool status = false)
    {
        Name = name;
        IpAddress = ipAddress;
        Counter = counter;
        Status = status;
    }

    public void GetName()
    {
        Console.WriteLine("Имя Стана: {0}", Name);
    }

    //public int getCounter()
    //{
    //    var buffer = new byte[4];
    //    int readResult = client.DBRead(66, 210, 4, buffer);
    //    int tmp = buffer[0] * 16777216 + buffer[1] * 65536 + buffer[2] * 256 + buffer[3];
    //    Console.WriteLine("Стан : {1}" + "Величина счетчика : {0}", tmp, Name);

    //    return tmp;
    //}
}
