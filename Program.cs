using System.Text;
using FSC;
// Encode
/*
while (true)
{
    Console.Write("Input: \t");
    string text = Console.ReadLine() ?? string.Empty;

    byte[] bytes = Encoding.UTF8.GetBytes(text);
    Console.Write("Output: \t");
    Console.WriteLine(Base64.Encode(bytes));
    Console.WriteLine();
}
*/

// Validate
/*
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
*/

// Decode

while (true)
{
    Console.Write("Input: \t");
    string text = Console.ReadLine() ?? string.Empty;
    byte[] bytes = Base64.Decode(text);
    Console.Write("Output: \t");
    Console.WriteLine(Encoding.UTF8.GetString(bytes));
    Console.WriteLine();
}
