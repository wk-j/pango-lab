use cairo::{Context, Format, ImageSurface};
use pango::FontDescription;
use pangocairo::functions::{create_layout, show_layout};

fn main() {
    let text = "ผู้ไม่รู้ กตัญญูพื้น มาตรฐานครุภัณฑ์";
    let font_description = "Sao Chingcha 50";
    let output_path = "image.png";

    convert_text_to_image(text, font_description, output_path);
    println!("Image saved to {}", output_path);
}

fn convert_text_to_image(text: &str, font_description: &str, output_path: &str) {
    // Create a temporary surface to measure the text
    let temp_surface = ImageSurface::create(Format::ARgb32, 1, 1).unwrap();
    let temp_context = Context::new(&temp_surface).unwrap();

    // Create a Pango layout
    let layout = create_layout(&temp_context);
    layout.set_text(text);

    // Set font description
    let font_desc = FontDescription::from_string(font_description);
    layout.set_font_description(Some(&font_desc));

    // Get the size of the layout
    let (text_width, text_height) = layout.pixel_size();

    // Add some padding
    let padding = 20;
    let width = text_width + (padding * 2);
    let height = text_height + (padding * 2);

    // Create the actual surface with the calculated dimensions
    let surface = ImageSurface::create(Format::ARgb32, width, height).unwrap();
    let context = Context::new(&surface).unwrap();

    // Clear the surface with white color
    context.set_source_rgb(1.0, 1.0, 1.0);
    context.paint().unwrap();

    // Draw the Pango layout
    context.set_source_rgb(0.0, 0.0, 0.0);
    context.move_to(padding as f64, padding as f64);
    show_layout(&context, &layout);

    // Save the surface as a PNG file
    let mut file = std::fs::File::create(output_path).unwrap();
    surface.write_to_png(&mut file).unwrap();
}
