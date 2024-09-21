# Base64 Encoder and Decoder in C#

This project provides a custom implementation of a Base64 encoder and decoder in C#. The code includes methods for encoding byte arrays to Base64 strings, decoding Base64 strings back to byte arrays, and validating Base64 strings.

## Features

- **Custom Charset Dictionary**: Uses a `_charset` dictionary to map byte values to Base64 characters, presented in a table-like format for better readability.

- **Encoding**: The `Encode` method converts a byte array into a Base64 encoded string.

- **Decoding**: The `Decode` and `TryDecode` methods convert a Base64 encoded string back into a byte array. `TryDecode` returns a boolean indicating success or failure, while `Decode` throws an exception if the input is invalid.

- **Validation**: The `IsValid` method checks if a given string is a valid Base64 encoded string.

## Usage

### Encoding Example

```csharp
using System.Text;
using FSC;

while (true)
{
    Console.Write("Input: \t");
    string text = Console.ReadLine() ?? string.Empty;

    byte[] bytes = Encoding.UTF8.GetBytes(text);
    Console.Write("Output: \t");
    Console.WriteLine(Base64.Encode(bytes));
    Console.WriteLine();
}
```

### Decoding Example

```csharp
using System.Text;
using FSC;

while (true)
{
    Console.Write("Input: \t");
    string text = Console.ReadLine() ?? string.Empty;
    byte[] bytes = Base64.Decode(text);
    Console.Write("Output: \t");
    Console.WriteLine(Encoding.UTF8.GetString(bytes));
    Console.WriteLine();
}
```

### Validation Example

```csharp
Console.WriteLine($"IsValid Base64: {Base64.IsValid("ABC")} == TRUE");
Console.WriteLine($"IsValid Base64: {Base64.IsValid("XYZ")} == TRUE");
Console.WriteLine($"IsValid Base64: {Base64.IsValid("...")}  == FALSE");
Console.WriteLine($"IsValid Base64: {Base64.IsValid("/\\")}  == FALSE");
Console.WriteLine($"IsValid Base64: {Base64.IsValid("123")}  == TRUE");
Console.WriteLine($"IsValid Base64: {Base64.IsValid("mno")}  == TRUE");
Console.WriteLine($"IsValid Base64: {Base64.IsValid("==")}  == FALSE");
Console.WriteLine($"IsValid Base64: {Base64.IsValid("Hallo==")}  == TRUE");
Console.WriteLine($"IsValid Base64: {Base64.IsValid("")}  == FALSE");
Console.WriteLine($"IsValid Base64: {Base64.IsValid(null)}  == FALSE");
Console.WriteLine($"IsValid Base64: {Base64.IsValid(string.Empty)}  == FALSE");
```

## Class Overview

### `Base64` Class

- **Fields**:
  - `_charset`: A `Dictionary<byte, char>` representing the Base64 character set in a table-like structure for better readability.
  - `PADDING`: A constant `char` representing the padding character `'='`.

- **Methods**:
  - `Encode(byte[] bytes)`: Encodes a byte array into a Base64 string.
  - `Decode(string str)`: Decodes a Base64 string into a byte array.
  - `TryDecode(string str, out byte[] bytes)`: Tries to decode a Base64 string, returning a boolean indicating success.
  - `IsValid(string str)`: Validates whether a string is a valid Base64 encoded string.
  - `GetCharsetBytes(char c)`: Retrieves the byte value corresponding to a Base64 character from the `_charset` dictionary.

## Implementation Details

- **Encoding Process**:
  - The input byte array is processed in chunks of 3 bytes (24 bits).
  - These 24 bits are divided into four 6-bit groups.
  - Each 6-bit group is mapped to a Base64 character using the `_charset` dictionary.
  - If the input length is not a multiple of 3, padding characters are added to the result.

- **Decoding Process**:
  - The input Base64 string is processed in chunks of 4 characters.
  - Padding characters are removed before processing.
  - Each character is converted back to its 6-bit value using `GetCharsetBytes`.
  - The 6-bit values are combined to reconstruct the original bytes.

- **Validation**:
  - The `IsValid` method checks for null or empty strings, verifies that padding is correctly placed, and ensures all characters are within the Base64 character set.

## Notes

- This implementation avoids using built-in Base64 methods to provide a deeper understanding of how Base64 encoding and decoding work.

- The use of a dictionary for the character set enhances readability and mimics a table structure, making it easier to understand the mapping between bytes and characters.

- Exception handling is implemented in the `Decode` method to handle invalid Base64 strings gracefully.

## License

This code is provided as-is without any warranty. You are free to use and modify it for your own projects.

