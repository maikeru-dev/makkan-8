import javax.swing.*;
import java.awt.*;
import java.awt.geom.Line2D;

public class Display extends JFrame {
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
    private Graphics2D graphics;
    private final Colour[][] pixels;
    private final Dimension dimension;
    public Display() {

        if (Settings.SUPER_CHIP_PRESENT) {
            dimension = new Dimension(128, 64);
        }else dimension = new Dimension(64, 32);

        pixels = new Colour[dimension.width][dimension.height];

        this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        this.setPreferredSize(new Dimension(360, 360));
        this.setTitle(Settings.PROGRAM_TITLE);
        this.setResizable(false);
        this.pack();
        this.setVisible(true);

        graphics = (Graphics2D) this.getGraphics();

    }
    private void draw () {

    }
    public void changeColour(Color c) {
        graphics.setColor(c);
    }
    @Override
    public void paint(Graphics g) {
        super.paint(g);
        drawPixel(g,0, 0);
    }
    public void drawPixel(Graphics g, int pointX, int pointY) {
        g.fillRect(pointX,pointY, 120,120);

    }
}
