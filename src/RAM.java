import java.io.File;
import java.io.FileInputStream;
import java.nio.ByteBuffer;

public class RAM {
    private final ByteBuffer memory;

    public RAM(int size) {
        memory = ByteBuffer.allocate(size);
    }

    public void write(int location, byte data) {
        if(location >= memory.limit()) {
            throw new ArrayIndexOutOfBoundsException("Ram is only " + memory.limit() + "bytes long");
        }
        memory.put(location, data);
    }

    public void writeBytes(int location, ByteBuffer data, int offset, int size) {
        if(location + size > memory.limit()) {
            throw new ArrayIndexOutOfBoundsException("Ram is only " + Settings.RAM_SIZE + "bytes long");
        }
        memory.put(location, data, offset, size);
    }

    public byte read8(int location) {
        if(location >= memory.limit()) {
            throw new ArrayIndexOutOfBoundsException("Ram is only " + memory.limit() + "bytes long");
        }
        return memory.get(location);
    }
    public short read16(int location) {
        if (location+1 >= memory.limit()){
            throw new ArrayIndexOutOfBoundsException("Ram is only " + memory.limit() + "bytes long");
        }
        return memory.getShort(location);
    }

    public ByteBuffer readBytes(int location, int size) {
        if(location + size >= memory.limit()) {
            throw new ArrayIndexOutOfBoundsException("Ram is only " + memory.limit() + "bytes long");
        }
        return memory.slice(location, size);
    }

    public void loadROMFile(int location, File file) {
        try (FileInputStream inputStream = new FileInputStream(file)) {
            var rom = inputStream.readAllBytes();
            writeBytes(location, ByteBuffer.wrap(rom), 0, rom.length);
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
