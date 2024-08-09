using Cairo;
using Pango;
using SkiaSharp;

class Program
{
    static void Main(string[] args)
    {
        string text = "จินตนาการอยู่เหนือความรู้ทั้งปวง กตัญญู";
        string fontDescription = "Sans 20";
        int width = 800;
        int height = 300;
        string outputPath = "image.png";

        ConvertTextToImage(text, fontDescription, width, height, outputPath);
    }

    static void ConvertTextToImage(string text, string fontDescription, int width, int height, string outputPath)
    {
        using (var surface = new ImageSurface(Format.Argb32, width, height))
        using (var context = new Cairo.Context(surface))
        {
            // Clear the surface with white color
            context.SetSourceRGB(1, 1, 1);
            context.Paint();

            // Create a Pango layout
            var layout = Pango.CairoHelper.CreateLayout(context);
            layout.SetText(text);

            // Set font description
            var fontDesc = FontDescription.FromString(fontDescription);
            layout.FontDescription = fontDesc;

            // Get the size of the layout
            int textWidth, textHeight;
            layout.GetPixelSize(out textWidth, out textHeight);

            // Calculate position to center the text
            double x = (width - textWidth) / 2;
            double y = (height - textHeight) / 2;

            // Draw the Pango layout
            context.SetSourceRGB(0, 0, 0);
            context.MoveTo(x, y);
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
