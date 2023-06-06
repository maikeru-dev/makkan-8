import java.awt.*;
import java.io.File;

public class Main {
    public static void main(String[] args) {
        Display display = new Display();
        RAM ram = new RAM(Settings.RAM_SIZE);
        ram.loadROMFile(0x200, new File("resources/IBM Logo.ch8"));
    }
}