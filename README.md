# Text-to-Image Converter

This project demonstrates the usage of Pango, a library for laying out and rendering text, especially in the context of internationalization. It includes both C# and Rust implementations for converting text to images.

## Project Structure

- `src/PangoTest/`: Contains the C# implementation
  - `PangoTest.csproj`: The C# project file
  - `Program.cs`: The main C# program file
- `app/text-to-image/`: Contains the Rust implementation
  - `Cargo.toml`: The Rust project file
  - `src/main.rs`: The main Rust program file

## Prerequisites

For C#:
- .NET Core SDK
- Pango library and its dependencies

For Rust:
- Rust toolchain
- Cairo and Pango libraries

## Building and Running

### C# Version

1. Navigate to the `src/PangoTest/` directory
2. Run `dotnet build` to build the project
3. Run `dotnet run` to execute the program

### Rust Version

1. Navigate to the `app/text-to-image/` directory
2. Run `cargo build` to build the project
3. Run `cargo run` to execute the program

## Features

- Converts text to image using Pango and Cairo
- Supports internationalized text, including Thai script
- Customizable font and font size
- Outputs PNG image file

## Sample Text

The project uses the following sample text to demonstrate its capabilities:

ผู้ไม่รู้ กตัญญูพื้น มาตรฐานครุภัณฑ์

This text showcases the ability to render Thai text, including complex characters and word combinations.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

[Add your chosen license here]
