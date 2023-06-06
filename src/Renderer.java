import javax.swing.*;
import java.awt.*;
import java.awt.image.BufferedImage;

import static java.awt.image.BufferedImage.TYPE_INT_RGB;

public class Renderer extends JPanel {

    private Display display;
    private BufferedImage image;
    private int pixelSize;
    private Dimension renderingDimension;


    public Renderer(Display display) {
        this.display = display;
        JFrame frame = new JFrame();
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.setSize(new Dimension(Settings.DISPLAY_MULTIPLIER * display.width, Settings.DISPLAY_MULTIPLIER*display.height));
        frame.setTitle(Settings.PROGRAM_TITLE);
        frame.setResizable(false);

        renderingDimension = new Dimension(display.width, display.height);
        this.setSize(new Dimension(display.width*Settings.DISPLAY_MULTIPLIER, display.height*Settings.DISPLAY_MULTIPLIER));
        this.image = new BufferedImage(getWidth(), getHeight(), TYPE_INT_RGB);
        pixelSize = Settings.DISPLAY_MULTIPLIER;
        image.createGraphics();

        frame.add(this);
        frame.setVisible(true);
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

    public void setDisplay(Display pixels) {

        this.display = pixels;
    }

    public void update() {
        for (int i = 0; i < renderingDimension.height; i++) {
            for (int j = 0; j < renderingDimension.width; j++) {
                image.setRGB(j, i, display.pixel(j, i));
            }
        }
        this.repaint();
    }

}
