import javax.swing.*;
import java.awt.*;
import java.awt.geom.AffineTransform;
import java.awt.image.BufferedImage;

import static java.awt.image.BufferedImage.TYPE_BYTE_BINARY;
import static java.awt.image.BufferedImage.TYPE_INT_RGB;

public class Renderer extends JPanel {
    private BufferedImage image;
    private int[] pixels;
    private int pixelSize;
    private Dimension renderingDimension;
    public Renderer(int width, int height, int[] pixels) {
        renderingDimension = new Dimension(width, height);

        this.setSize(new Dimension(width*Settings.DISPLAY_MULTIPLIER, height*Settings.DISPLAY_MULTIPLIER));

        this.image = new BufferedImage(getWidth(), getHeight(), TYPE_INT_RGB);
        this.pixels = pixels;
        pixelSize = Math.min(width / getWidth(), height / getHeight());
        image.createGraphics();
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
    private Graphics2D getImageGraphics() {
        return (Graphics2D) image.getGraphics();
    }
    public void setPixels(int[] pixels) {
        this.pixels = pixels;
    }
    public void update() {
        for (int i = 0; i < renderingDimension.height; i++) {
            for (int j = 0; j < renderingDimension.width; j++) {
                image.setRGB(j, i, pixels[j+i*Settings.DISPLAY_MULTIPLIER]);
            }
        }
        this.repaint();
    }

}
