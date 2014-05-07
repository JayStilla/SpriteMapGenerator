using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using System.IO;

namespace SpriteMapGenerator
{
    public class MyCanvas : Canvas
    {
        BitmapImage background = null;

        public void LoadImage(string filename)
        {
            background = new BitmapImage(new Uri(filename));
            this.InvalidateVisual();
        }
        protected override void OnRender(DrawingContext dc)
        {
            if (background != null)
            {
                dc.DrawImage(background, new Rect(0, 0, background.PixelWidth, background.PixelHeight));
            }
        }


        public void GenSheet(string[] images)
        {
            BitmapFrame[] frames = new BitmapFrame[images.Length];
            //int[] imageWidth = new int[images.Length];
            //int[] imageHeight = new int[images.Length];

            // Atlas Dimensions
            int iW = 0;
            int iH = 0; 

            // Loads the images to tile (no need to specify PngBitmapDecoder, the correct decoder is automatically selected)
            for (int i = 0; i < images.Length; i++)
            {
               frames[i] = BitmapDecoder.Create(new Uri(images[i]), BitmapCreateOptions.None, BitmapCacheOption.OnLoad).Frames.First();
               //imageWidth[i] = frames[i].PixelWidth;
               //imageHeight[i] = frames[i].PixelHeight; 
            }
            //Make it so that the two biggest heights and widths depict size
            //iW = imageWidth[0] + imageWidth[1];
            //iH = imageHeight[0] + imageHeight[2];
            for(int i = 0; i<frames.Length; i++)
            {
                iW += frames[i].PixelWidth;
            }
            for(int i = 1; i<frames.Length; i++)
            {
                iH = frames[i - 1].PixelHeight;
            }
            

            // Draws the images into a DrawingVisual component
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                int prevFrames = 0;
                for (int i = 0; i < frames.Length; i++)
                {
                    drawingContext.DrawImage(frames[i], new Rect(prevFrames, 0, frames[i].PixelWidth, frames[i].PixelHeight));
                    prevFrames += frames[i].PixelWidth;

                }
            }

            

            // Converts the Visual (DrawingVisual) into a BitmapSource
            RenderTargetBitmap bmp = new RenderTargetBitmap(iW, iH, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);

            // Creates a PngBitmapEncoder and adds the BitmapSource to the frames of the encoder
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));

            string directory = Directory.GetCurrentDirectory();

            // Saves the image into a file using the encoder
            using (Stream stream = File.Create(directory + @"\tile.png"))
                encoder.Save(stream);
        }
    }
}
