using System.Diagnostics;

namespace makkan_8;

public class Chip8
{
    private const ushort LastMask = 0b0000_0000_0000_1111;
    private const ushort ThirdMask = 0b0000_0000_1111_0000;
    private const ushort SecondMask = 0b0000_1111_0000_0000;
    private const ushort FirstMask = 0b1111_0000_0000_0000;

    public RAM memory = new RAM(Settings.RAM_SIZE);
    public Stack<byte> inputs = new ();
    public Display display = new Display();
    public int PC = 0x200;
    public int I = 0x000;
    private Stack<ushort> functionStack = new Stack<ushort>();
    private byte delayTimer = 0x000;
    private byte soundTimer = 0x000;
    public byte[] V = new byte[16];

    public Chip8()
    {
        memory.WriteBytes(0x50, Font.FONT_DATA, 0, (int) Font.FONT_DATA.Length);
    }

    public void LoadRom(String filepath) {
        memory.LoadRomFile(PC, filepath);
    }

    public void Update()
    {
        DEInstruction(FetchInstruction());
    }

    public ushort FetchInstruction()
    {
        PC += 2;
        return memory.Read16(PC - 2);
    }

    public void DEInstruction(ushort instruction)
    {
        // print out all instructions in hex
        switch (instruction)
        {
            case 0x00E0:
                display.FillScreen(0x000000);
                break;
            case 0x00EE: // : SUBROUTINE RETURN : JUMP BUT BEFORE JUMP FIRST PUSH PC
                break;
            default:
                int a = instruction & 0b1111_0000_0000_0000;
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
                    case 0xC000: // uNFINSHED damn why is NFINISHED unsigned?
                        break;
                    case 0xD000:
                        byte x = (byte) (V[(instruction & SecondMask) >> 8] % display.width);
                        byte y = (byte) (V[(instruction & ThirdMask) >> 4] % display.height);
                        byte N = (byte) (instruction & LastMask);

                        V[0xF] = 0;

                        for (int n = 0; n < N; n++) {
                            byte spriteRow = memory.Read8(I + n);
                            for (int i = (x > display.width-7 ? display.width-x : 7); i >= 0; i--) {
                                int bit = (spriteRow >> i) & 1;
                                if (display.UpdatePixel(x,y, bit)) {
                                    V[0xF] = 1;
                                }
                                x++;
                            }
                            x = (byte) (x-8);
                            y++;
                            if (y > 32) break;
                        }
                        break;
                    case 0xF000:
                        switch (0b0000_0000_1111_1111 & instruction)
                        {
                            case 0x000A:
                                // THIS IS UR CODE MAKKA <3
                                // byte location = (byte) ((SecondMask & instruction) >> 8);
                                //
                                // if (inputs) // byte
                                // {
                                //     PC += 2;
                                //     V[location] = 
                                // }
                                // else
                                // {
                                //     PC -= 2; 
                                // }
                                break;
                        }
                        break;
                }
                break;
        }
    }
}