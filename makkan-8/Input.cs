namespace makkan_8;

public class Input
{ // This class can be used to interface with the Chip8 Emulator and provide input
    private bool[] keyArr = new bool[16];
    private Stack<byte> keyStack = new ();
    public Input()
    {

    }

    public void addPressed(byte key)
    {
        if (key > 16) throw new Exception("key input is out of range");

        keyArr[key] = true;
    }
    
    
}  