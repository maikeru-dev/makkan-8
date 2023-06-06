import java.awt.*;
import java.io.File;
import java.util.Timer;
import java.util.TimerTask;
import java.util.concurrent.atomic.AtomicInteger;

public class Main {
    public static void main(String[] args) {
        Display display = new Display();
        RAM ram = new RAM(Settings.RAM_SIZE);
        ram.loadROMFile(0x200, new File("resources/IBM Logo.ch8"));

        Timer timer = new Timer();
        AtomicInteger secondTimer = new AtomicInteger(360); // count this down at 60Hz

        var task = new TimerTask() {
            @Override
            public void run() {
                secondTimer.decrementAndGet();
                System.out.println("Second timer: " + secondTimer);
            }
        };

        timer.scheduleAtFixedRate(task, 2000, 2000);
    }
}