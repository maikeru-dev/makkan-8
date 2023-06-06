import javax.swing.*;
import java.awt.*;
import java.awt.geom.AffineTransform;
import java.awt.image.BufferedImage;

import static java.awt.image.BufferedImage.TYPE_BYTE_BINARY;
import static java.awt.image.BufferedImage.TYPE_INT_RGB;

public class Renderer extends JPanel {
    private BufferedImage image;
    private Display display;
    private int pixelSize;
    public Renderer(Display display) {
        this.display = display;
        this.pixelSize = Settings.DISPLAY_MULTIPLIER;
        Dimension commonDimension = new Dimension(display.width*Settings.DISPLAY_MULTIPLIER, display.height*Settings.DISPLAY_MULTIPLIER);

        this.setSize(commonDimension);
        this.image = new BufferedImage(getWidth(), getHeight(), TYPE_INT_RGB);
        image.createGraphics();

        JFrame JFrame = new JFrame(Settings.PROGRAM_TITLE);
        JFrame.setSize(commonDimension);
        JFrame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        JFrame.setResizable(false);
        JFrame.getContentPane().add(this);
        JFrame.setVisible(true);
    }
    @Override
    public void paintComponent(Graphics g) {
        super.paintComponent(g);
        Graphics2D g2d = (Graphics2D) g;
        for (int i = 0; i < getHeight(); i++) {
            for (int j = 0; j < getWidth(); j++) {
                int pixelValue = image.getRGB(j, i);
                Color pixelColor = new Color(pixelValue);
                g2d.setColor(pixelColor);
                g2d.fillRect(j * pixelSize, i * pixelSize, pixelSize, pixelSize);
            }
        }
    }
    public void update() {
        for (int i = 0; i < display.height; i++) {
            for (int j = 0; j < display.width; j++) {
                image.setRGB(j, i, display.pixel(j, i));
            }
        }
        this.repaint();
    }

}
