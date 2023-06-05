import java.io.File;
import java.io.FileInputStream;

public class RAM {
    private final byte[] memory;

    public RAM(int size) {
        memory = new byte[size];
    }

    public void write(int location, byte data) {
        if(location >= memory.length) {
            throw new ArrayIndexOutOfBoundsException("Ram is only " + memory.length + "bytes long");
        }
        memory[location] = data;
    }

    public byte read(int location) {
        if(location >= memory.length) {
            throw new ArrayIndexOutOfBoundsException("Ram is only " + memory.length + "bytes long");
        }
        return memory[location];
    }

    public void loadROMFile(int location, File file) {
        try (FileInputStream inputStream = new FileInputStream(file)) {
            var rom = inputStream.readAllBytes();
            assert location + rom.length < memory.length : "Ram is only " + Settings.RAM_SIZE + "bytes long";
            System.arraycopy(rom, 0, memory, location, rom.length);
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
