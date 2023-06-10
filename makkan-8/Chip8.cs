namespace makkan_8;

public class Chip8
{
    private const ushort LastMask = 0b0000_0000_0000_1111;
    private const ushort ThirdMask = 0b0000_0000_1111_0000;
    private const ushort SecondMask = 0b0000_1111_0000_0000;
    private const ushort FirstMask = 0b1111_0000_0000_0000;

    private RAM memory = new RAM(Settings.RAM_SIZE);
    public Stack<byte> inputs = new ();
    public Display display = new Display();
    private int PC = 0x200 / 8;
    private int I = 0x000;
    private Stack<ushort> functionStack = new Stack<ushort>();
    private byte delayTimer = 0x000;
    private byte soundTimer = 0x000;
    private byte[] V = new byte[16];
    
    public void LoadRom(String filepath) {
        memory.LoadRomFile(PC, filepath);
    }

    public ushort FetchInstruction()
    {
        PC += 2;
        return memory.Read16((PC - 2) * 8);
    }

    public void DEInstruction(ushort instruction)
    {
        switch (instruction)
        {
            case 0x00E0:
                display.FillScreen(0x000000);
                break;
            case 0x00EE: // : SUBROUTINE RETURN : JUMP BUT BEFORE JUMP FIRST PUSH PC
                break;
            default:
                int a = instruction & 0b1111_0000_0000_0000;
                Console.WriteLine($"{a:X}");
                switch (instruction & 0b1111_0000_0000_0000)
                {
                    case 0x1000: // : JUMP - 12 bit NNN
                        PC = 0b0000_1111_1111_1111 & instruction;
                        break;
                    case 0x6000: // : SET REG X TO NN : 6XNN
                        V[(instruction & SecondMask) >> 8] = (byte) (0b0000_0000_1111_1111 & instruction);
                        break;
                    case 0x7000: // : ADD NN TO REG X : 7XNN
                        V[(instruction & SecondMask) >> 8] += (byte) (0b0000_0000_1111_1111 & instruction);
                        break;
                    case 0xA000: // : SET INDEX REG TO NNN : ANNN
                        I = 0b0000_1111_1111_1111 & instruction;
                        break;
                    case 0xC000: // uNFINSHED
                        break;
                    case 0xD000:
                        break;
                    case 0xF000:
                        switch (0b0000_0000_1111_1111 & instruction)
                        {
                            case 0x000A:
                                byte location = (byte) ((SecondMask & instruction) >> 8);

                                if (inputs) // byte
                                {
                                    PC += 2;
                                    V[location] = 
                                }
                                else
                                {
                                    PC -= 2; 
                                }
                                break;
                        }
                        break;
                }
                break;
        }
    }
}