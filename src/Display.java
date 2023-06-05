import javax.swing.*;
import java.awt.*;
public class Display {
    private enum Colour {
        BLACK,
        WHITE
    }
    // Notes about graphics2D:
    /* Renders 0,0 @ top left
       All coordinates are integers.
       Increasing Y is downward, Increasing X is rightward.
       Can draw images (sprites?)
    *  */
    private static final int DPM = Settings.DISPLAY_MULTIPLIER; // Also size of the sq. pixel
    private Graphics2D graphics;
    private final Colour[] pixels;
    private final int height;
    private final int width;
    private final Dimension dimension;
    public Display() {

        if (Settings.SUPER_CHIP_PRESENT) {
            width = 128; height = 64;
        }else {width = 64; height = 32;}

        pixels = new Colour[width*height];
        dimension = new Dimension(width*DPM, height*DPM);

        JFrame frame = new JFrame();
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.setPreferredSize(dimension);
        frame.setTitle(Settings.PROGRAM_TITLE);
        frame.setResizable(false);
        frame.setVisible(true);
        frame.getContentPane().add(new Renderer(dimension));

    }
}
