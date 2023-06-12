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
    public ushort PC = Settings.ROM_ADRESS;
    public ushort I = 0x000;
    private Stack<ushort> functionStack = new Stack<ushort>();
    private double timer = 0.0f;
    private byte delayTimer = 0x000;
    private byte soundTimer = 0x000;
    public byte[] V = new byte[16];

    public Chip8()
    {
        memory.WriteBytes(Settings.FONT_ADRESS, Font.FONT_DATA, 0, (int) Font.FONT_DATA.Length);
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

        var instructionAmount = 1;
        if (Settings.INSTRUCTIONS_PER_SECOND != 0)
        {
            instructionAmount = (int)(Settings.INSTRUCTIONS_PER_SECOND * deltaTime);
        }

        for (int i = 0; i < instructionAmount; i++)
        {
            DEInstruction(FetchInstruction());
        }
        Debug.WriteLine($"Instructions per sercond: {instructionAmount / deltaTime}");

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
            default:
                switch (instruction & 0b1111_0000_0000_0000)
                {
                    case 0x0000: // : SUBROUTINES RETURN : return to top of function stack and pop
                        if(N2 == 0xEE) {
                            PC = functionStack.Pop();
                        }
                        break;
                    case 0x2000: // : SUBROUTINES CALL : call the function in NNN but push PC to function stack
                        functionStack.Push(PC);
                        PC = N3;
                        break;
                    case 0x1000: // : JUMP - 12 bit NNN
                        PC = N3;
                        break;
                    case 0x3000: // : SKIP : Skip if VX == NN
                        if (V[X] == N2) PC += 2;
                        break;
                    case 0x4000: // : SKIP : Skip if VX != NN
                        if (V[X] != N2) PC += 2;
                        break;
                    case 0x5000: // : SKIP : Skip if VX == VY
                        if (V[X] == V[Y]) PC += 2;
                        break;
                    case 0x9000: // : SKIP : Skip if VX != VY
                        if (V[X] != V[Y]) PC += 2;
                        break;
                    case 0x6000: // : SET REG X TO NN : 6XNN
                        V[X] = N2;
                        break;
                    case 0x7000: // : ADD NN TO REG X : 7XNN
                        V[X] += N2;
                        break;
                    case 0x8000:
                        switch(N) {
                            case 0:
                                V[X] = V[Y];
                                break;
                            case 1:
                                V[X] |= V[Y];
                                break;
                            case 2:
                                V[X] &= V[Y];
                                break;
                            case 3:
                                V[X] ^= V[Y];
                                break;
                            case 4:
                                var result = V[X] + V[Y];
                                V[X] = (byte) result;
                                V[0xF] = 0;
                                if (result >= 0xFF) V[0xF] = 1;
                                break;
                            case 5:
                                var result1 = V[X] - V[Y];
                                V[X] = (byte) result1;
                                V[0xF] = 0;
                                if (result1 > 0) V[0xF] = 1;
                                break;
                            case 7:
                                var result2 = V[Y] - V[X];
                                V[X] = (byte) result2;                 
                                V[0xF] = 0;
                                if (result2 > 0) V[0xF] = 1;
                                break;
                            case 6:
                                if (Settings.QUIRKY)
                                {
                                    V[X] = V[Y];
                                }
                                byte shiftedBit = (byte) (V[X] & 0x0000_0001);
                                V[X] >>= 1;
                                V[0xF] = shiftedBit;
                                break;
                            case 0xE:
                                if (Settings.QUIRKY)
                                {
                                    V[X] = V[Y];
                                }
                                byte shiftedBit1 = (byte) (V[X] >> 7);
                                V[X] <<= 1;
                                V[0xF] = shiftedBit1;
                                break;
                        }
                        break;
                    case 0xA000: // : SET INDEX REG TO NNN : ANNN
                        I = N3;
                        break;
                    case 0xB000: // : JUMP WITH OFFSET :
                        if (Settings.QUIRKY)
                        {
                            PC = (ushort) (N3 + V[0]);
                            break;
                        }
                        PC = (ushort) (N2 + X);
                        break;
                    case 0xC000: // uNFINSHED damn why is NFINISHED unsigned?
                        byte randomByte = (byte) Random.Shared.Next();
                        randomByte &= N2;
                        V[X] = randomByte;
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
                            case 0x001E: // : ADD TO INDEX :
                                I += V[X];
                                if (I > 0xFFF)
                                {
                                    V[0xF] = 1;
                                }
                                break;
                            case 0x0029: // : FONT CHARACTER : Point to the char in the second nibble of VX
                                byte character = (byte) (V[X] & 0x00FF);
                                I = (ushort) (Settings.FONT_ADRESS + character * 5);
                                break;
                            case 0x0033:
                                // 255 % 10 = 5
                                // 255 % 100 = 55 / 10 = 5
                                // 255 / 100
                                int ones = V[X] % 10;
                                int tens = V[X] % 100 / 10;
                                int hundrets = V[X] / 100;
                                memory.Write(I, (byte) hundrets);
                                memory.Write(I + 1, (byte) tens);
                                memory.Write(I + 2, (byte) ones);
                                break;
                            case 0x0055:
                                for (byte i = 0; i <= X; i++)
                                {
                                    memory.Write(I + i, V[i]);
                                }

                                if (Settings.QUIRKY) I += X;
                                break;
                            case 0x0065:
                                for (byte i = 0; i <= X; i++)
                                {
                                    V[i] = memory.Read8(I + i);
                                }
                                if (Settings.QUIRKY) I += X;
                                break;
                        }
                        break;
                }
                break;
        }
    }
}