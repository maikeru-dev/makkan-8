using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace makkan_8;

public class RAM
{
    private byte[] memory;
    private MemoryStream stream;

    public RAM(int size)
    {
        memory = new byte[size];
        stream = new MemoryStream(memory);
        memory[0] = 0b1111;
        memory[1] = 0b1111;
    }
    public void Write(int location, byte data)
    {
        if (location >= memory.Length)
        {
            throw new IndexOutOfRangeException($"Ram is only {memory.Length} long!");
        }
        memory[location] = data;
    }

    public void WriteBytes(int location, MemoryStream stream, int offset, int size)
    {
        if (location >= memory.Length)
        {
            throw new IndexOutOfRangeException($"Ram is only {memory.Length} long!");
        }

        this.stream.Position = location;
        this.stream.Write(stream.ToArray(), offset, size);
    }

    public byte Read8(int location)
    {
        if (location >= memory.Length)
        {
            throw new IndexOutOfRangeException($"Ram is only {memory.Length} long!");
        }

        return memory[location];
    }

    public ushort Read16(int location)
    {
        if (location+1 >= memory.Length)
        {
            throw new IndexOutOfRangeException($"Ram is only {memory.Length} long!");
        }
        
        return (ushort) ((memory[location] << 8) | memory[location + 1]);
    }
    
    public void LoadRomFile(int location, String filepath)
    {
        if (!File.Exists(filepath)) throw new FileNotFoundException($"File at {Path.GetFullPath($"./{filepath}")} doesn't exist.");
        try
        {
            var rom = File.ReadAllBytes(filepath);
            Console.Out.Write($"ROM read-in size: {rom.Length}");
            WriteBytes(location, new MemoryStream(rom), 0, rom.Length);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
        }
    }

}