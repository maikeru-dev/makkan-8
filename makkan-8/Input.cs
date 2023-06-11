namespace makkan_8;

public class Input
{ // This class can be used to interface with the Chip8 Emulator and provide input
    protected bool[] keyArr = new bool[16];
    protected Queue<byte> keyStack = new ();
    public Input()
    {
        keyArr.Initialize();
        
    }

    public void Enable(byte keyCode)
    {
        keyArr[keyCode] = true;
        if (keyStack.Count < 30)
        {
            keyStack.Enqueue(keyCode);
        }
        else // tbh haven't thought that hard on this else statement, might not work as I imagine
        {
            keyStack.Dequeue();
            keyStack.Enqueue(keyCode);
        }
        
    }

    public void Disable(byte keyCode)
    {
        keyArr[keyCode] = false;
    }

    public byte Dequeue()
    {
        return keyStack.Dequeue();
    }

    public bool IsQueuedEnabled()
    {
        if (keyStack.Count > 0)
        {
            return keyArr[keyStack.Peek()];
        }

        return false;
    }

}  