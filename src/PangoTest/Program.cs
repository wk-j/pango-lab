using Cairo;
using Pango;
using SkiaSharp;

public class Program
{
    static void Main(string[] args)
    {
        string text = "กตัญญุตาผู้รู้ที่ครุภัณฑ์ตั้งถิ่นฐาน";
        string fontDescription = "Noto Serif Thai 50";
        string outputPath = "output2.png";

        ConvertTextToImage(text, fontDescription, outputPath);
    }

    static void ConvertTextToImage(string text, string fontDescription, string outputPath)
    {
        // Create a temporary surface to measure the text
        using (var tempSurface = new ImageSurface(Format.Argb32, 1, 1))
        using (var tempContext = new Cairo.Context(tempSurface))
        {
            // Create a Pango layout
            var layout = Pango.CairoHelper.CreateLayout(tempContext);
            layout.SetText(text);

            // Set font description
            var fontDesc = FontDescription.FromString(fontDescription);
            layout.FontDescription = fontDesc;

            // Get the size of the layout
            int textWidth, textHeight;
            layout.GetPixelSize(out textWidth, out textHeight);

            // Add some padding
            int padding = 20;
            int width = textWidth + (padding * 2);
            int height = textHeight + (padding * 2);

            // Create the actual surface with the calculated dimensions
            using (var surface = new ImageSurface(Format.Argb32, width, height))
            using (var context = new Cairo.Context(surface))
            {
                // Clear the surface with white color
                context.SetSourceRGB(1, 1, 1);
                context.Paint();

                // Draw the Pango layout
                context.SetSourceRGB(0, 0, 0);
                context.MoveTo(padding, padding);
                Pango.CairoHelper.ShowLayout(context, layout);

                // Convert Cairo surface to SkiaSharp bitmap
                var skImageInfo = new SKImageInfo(width, height, SKColorType.Bgra8888, SKAlphaType.Premul);
                using (var skSurface = SKSurface.Create(skImageInfo))
                {
                    var skCanvas = skSurface.Canvas;
                    var pixelData = new byte[width * height * 4];
                    System.Runtime.InteropServices.Marshal.Copy(surface.DataPtr, pixelData, 0, pixelData.Length);

                    using (var skBitmap = new SKBitmap())
                    {
                        var pinnedPixelData = System.Runtime.InteropServices.GCHandle.Alloc(pixelData, System.Runtime.InteropServices.GCHandleType.Pinned);
                        try
                        {
                            skBitmap.InstallPixels(skImageInfo, pinnedPixelData.AddrOfPinnedObject());
                            skCanvas.DrawBitmap(skBitmap, 0, 0);
                        }
                        finally
                        {
                            pinnedPixelData.Free();
                        }
                    }

                    using (var image = skSurface.Snapshot())
                    using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                    using (var stream = File.OpenWrite(outputPath))
                    {
                        data.SaveTo(stream);
                    }
                }
            }
        }
    }
}
