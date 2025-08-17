# Old Phone Pad Converter

A .NET implementation that converts old phone keypad input sequences into text, simulating the text input method used on classic mobile phones (T9 input).

## 📱 Overview

This project provides a solution for converting numeric keypad sequences (like those from old Nokia phones) into text. Each number key represents multiple letters, and pressing a key multiple times cycles through the available characters.

## 🎯 Features

- **T9 Text Conversion**: Converts numeric key sequences to text according to old phone keypad rules
- **Multiple Key Press Support**: Handles cycling through characters on repeated key presses
- **Special Characters**: Supports special characters via the '1' key (&, ', ()
- **Space Character**: Uses '0' key to add spaces
- **Backspace Support**: Uses '*' key to delete previous characters
- **Pause Functionality**: Uses spaces to separate sequences on the same key
- **Detailed Explanation**: Can provide step-by-step explanation of the conversion process
- **Input Validation**: Validates input format and provides helpful error messages

## 📋 Project Structure

```
OldPhonePad/
├── OldPhonePad.Core/           # Core library
│   ├── OldPhonePadConverter.cs # Main conversion logic
│   └── OldPhonePadExtensions.cs # Additional utility methods
├── OldPhonePad.Tests/          # Unit tests
│   └── OldPhonePadTests.cs     # Comprehensive test suite
├── OldPhonePad.Demo/           # Demo console application
│   └── Program.cs              # Interactive demo program
└── OldPhonePad.sln             # Solution file
```

## 📱 Phone Keypad Layout

```
┌─────┬─────┬─────┐
│  1  │  2  │  3  │
│ &'( │ ABC │ DEF │
├─────┼─────┼─────┤
│  4  │  5  │  6  │
│ GHI │ JKL │ MNO │
├─────┼─────┼─────┤
│  7  │  8  │  9  │
│PQRS │ TUV │WXYZ │
├─────┼─────┼─────┤
│  *  │  0  │  #  │
│ ←   │space│ SEND│
└─────┴─────┴─────┘
```

## 💡 How It Works

### Basic Rules
- **Multiple Presses**: Press a key multiple times to cycle through its letters
  - Example: `2` = 'A', `22` = 'B', `222` = 'C'
- **Different Keys**: Different number keys produce different letters immediately
  - Example: `23` = 'AD'
- **Same Key Pause**: Use a space to type multiple letters from the same key
  - Example: `222 2 22` = 'CAB'
- **Backspace**: Use `*` to delete the last character
  - Example: `227*` = 'B' (types 'BA' then deletes 'A')
- **Space**: Use `0` to add a space character
  - Example: `20` = 'A '
- **Send**: Always end input with `#` to indicate completion

### Examples

| Input | Output | Description |
|-------|--------|-------------|
| `33#` | E | Press 3 twice |
| `227*#` | B | Type 'BA', backspace, result is 'B' |
| `4433555 555666#` | HELLO | Using pause for repeated keys |
| `8 88777444666*664#` | TURING | Complex with backspace |
| `222 2 22#` | CAB | Same key with pauses |
| `4433555 555666 0 9666777555 3#` | HELLO WORLD | Full sentence with space |

## 🚀 Getting Started

### Prerequisites

- .NET 9.0 SDK
- VS Code or any ide

### Building the Solution

```bash
dotnet build
```

### Running the Tests

```bash
dotnet test
```

### Running the Demo

```bash
dotnet run --project OldPhonePad.Demo
```

## 🔧 Functionality

### Basic Usage

```csharp
using OldPhonePad.Core;

// Convert a keypad sequence to text
string result = OldPhonePadConverter.OldPhonePad("4433555 555666#");
Console.WriteLine(result); // Output: HELLO
```

### Input Validation

```csharp
// Check if input is valid before conversion
if (OldPhonePadExtensions.IsValidOldPhonePadInput(input))
{
    string result = OldPhonePadConverter.OldPhonePad(input);
    Console.WriteLine(result);
}
else
{
    Console.WriteLine("Invalid input format");
}
```

### Get Detailed Explanation

```csharp
// Get a step-by-step explanation of the conversion
string explanation = OldPhonePadExtensions.ExplainConversion("22 3#");
Console.WriteLine(explanation);
// Shows detailed steps of how the conversion works
```

## 🧪 Testing

The project includes a comprehensive test suite with over 25 test cases covering:

- ✅ Basic functionality (single key press, multiple key press)
- ✅ Space/pause functionality
- ✅ Backspace operations
- ✅ Special characters
- ✅ Edge cases and input validation
- ✅ Complex messages
- ✅ Zero key (space) functionality
- ✅ Explanation functionality

## 📖 Implementation Notes

The implementation uses a stack-based approach to handle sequences of the same key. When a different key is pressed or a space is encountered, the stack is processed to determine the appropriate character.

Key features of the implementation:

- **Efficient Processing**: Processes input character by character in a single pass
- **Stack-Based Approach**: Uses a stack to count repeated key presses
- **StringBuilder**: Uses StringBuilder for efficient string building
- **Clear Separation**: Separate classes for core functionality and extensions

## 👤 Author

Created by Tahsin Ahmed as a solution for the Iron Software C# Coding Challenge.
