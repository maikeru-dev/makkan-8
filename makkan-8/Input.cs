namespace makkan_8;

public class Input
{ // This class can be used to interface with the Chip8 Emulator and provide input
    protected bool[] keyArr = new bool[16];
    protected Queue<byte> keyQueue = new ();

    protected enum KeyState
    {
        NOINPUT,
        RELEASED,
        HELD,
        PRESSED
    }
    public Input()
    {
        keyArr.Initialize();
        
    }

    public byte Peek()
    {
        byte x;
        keyQueue.TryPeek(out x);
        return x;
    }

    protected void Update(byte keyCode, KeyState state)
    {
        if (keyCode == 16) return;
    }

    public void Enable(byte keyCode)
    {
        if (keyCode == 16) return;
        keyArr[keyCode] = true;
        if (keyQueue.Count < 300) // debug do not forget
        {
            keyQueue.Enqueue(keyCode);
        }
        else // tbh haven't thought that hard on this else statement, might not work as I imagine
        {
            keyQueue.Clear(); // debug do not forget
            keyQueue.Enqueue(keyCode);
        }
        
    }

    public bool isPressed(byte keyCode)
    {
        return keyArr[keyCode];
    }

    public void Disable(byte keyCode)
    {
        if (keyCode == 16) return;
        keyArr[keyCode] = false;
    }

    public byte Dequeue()
    {
        byte x;
        keyQueue.TryDequeue(out x);
        return x;
    }

    public bool IsQueuedEnabled()
    {
        if (keyQueue.Count > 0)
        {
            return keyArr[keyQueue.Peek()];
        }

        return false;
    }

}  