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
    public ushort PC = 0x200;
    public ushort I = 0x000;
    private Stack<ushort> functionStack = new Stack<ushort>();
    private double timer = 0.0f;
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

    public void Update(double deltaTime = 0.0f)
    {
        timer += deltaTime;
        if (timer >= 1d / 60d)
        {
            delayTimer -= 1;
            delayTimer = Math.Min(delayTimer, (byte) 0);
            soundTimer -= 1;
            soundTimer = Math.Min(soundTimer, (byte)0);
            timer = 0;
        }
        DEInstruction(FetchInstruction());
    }

    public ushort FetchInstruction()
    {
        PC += 2;
        return memory.Read16(PC - 2);
    }

    public void DEInstruction(ushort instruction)
    {
        byte X = (byte) ((instruction & 0x0F00) >> 8);
        byte Y = (byte) ((instruction & 0x00F0) >> 4);
        byte N = (byte) (instruction & 0x000F);
        byte N2 = (byte) (instruction & 0x00FF);
        ushort N3 = (ushort) (instruction & 0x0FFF);

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
                        PC = N3;
                        break;
                    case 0x6000: // : SET REG X TO NN : 6XNN
                        V[X] = N2;
                        break;
                    case 0x7000: // : ADD NN TO REG X : 7XNN
                        V[X] += N2;
                        break;
                    case 0xA000: // : SET INDEX REG TO NNN : ANNN
                        I = N3;
                        break;
                    case 0xC000: // uNFINSHED damn why is NFINISHED unsigned?
                        break;
                    case 0xD000:
                        byte x = (byte) (V[X] % display.width);
                        byte y = (byte) (V[Y] % display.height);

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
                            case 0x0007:
                                V[X] = delayTimer;
                                break;
                            case 0x0015:
                                delayTimer = V[X];
                                break;
                            case 0x0017:
                                soundTimer = V[X];
                                break;
                        }
                        break;
                }
                break;
        }
    }
}