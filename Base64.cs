/* Source: https://de.wikipedia.org/wiki/Base64
 *
 * MIT License
 *
 * Copyright(c) 2024 Fabian2000 (GitHub) | Fabian Schlüter
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 * 
*/

using System.Text;

namespace FSC
{
    public class Base64
    {
        // Use of Dictionary instead of string or char array to display everything in a cleaner way like a table (For better reading)
        private static readonly Dictionary<byte, char> _charset = new Dictionary<byte, char>()
        {
            { 0, 'A' }, { 1, 'B' }, { 2, 'C' }, { 3, 'D' }, { 4, 'E' }, { 5, 'F' },
            { 6, 'G' }, { 7, 'H' }, { 8, 'I' }, { 9, 'J' }, { 10, 'K' }, { 11, 'L' },
            { 12, 'M' }, { 13, 'N' }, { 14, 'O' }, { 15, 'P' }, { 16, 'Q' }, { 17, 'R' },
            { 18, 'S' }, { 19, 'T' }, { 20, 'U' }, { 21, 'V' }, { 22, 'W' }, { 23, 'X' },
            { 24, 'Y' }, { 25, 'Z' },
            { 26, 'a' }, { 27, 'b' }, { 28, 'c' }, { 29, 'd' }, { 30, 'e' }, { 31, 'f' },
            { 32, 'g' }, { 33, 'h' }, { 34, 'i' }, { 35, 'j' }, { 36, 'k' }, { 37, 'l' },
            { 38, 'm' }, { 39, 'n' }, { 40, 'o' }, { 41, 'p' }, { 42, 'q' }, { 43, 'r' },
            { 44, 's' }, { 45, 't' }, { 46, 'u' }, { 47, 'v' }, { 48, 'w' }, { 49, 'x' },
            { 50, 'y' }, { 51, 'z' },
            { 52, '0' }, { 53, '1' }, { 54, '2' }, { 55, '3' }, { 56, '4' }, { 57, '5' },
            { 58, '6' }, { 59, '7' }, { 60, '8' }, { 61, '9' }, { 62, '+' }, { 63, '/' }
        };

        private const char PADDING = '=';

        public static string Encode(byte[] bytes)
        {
            StringBuilder result = new StringBuilder();
            int paddings = 0;

            // 6 + 6 + 6 + 6 = 24
            // 8 + 8 + 8 = 24
            // So we can work on 3 array items at the same time. Which means i < length, but instead of i++, we do i += 3

            for (int i = 0; i < bytes.Length; i += 3)
            {
                byte getSix = (byte)(bytes[i] >> 2);
                byte rest = (byte)(bytes[i] << 6);
                rest = (byte)(rest >> 2); // << 6 made it back to 8, but we need 6, so 2 leading zeros
                result.Append(_charset[getSix]);

                if (i + 1 >= bytes.Length)
                {
                    result.Append(rest); // There is no more to work with, so we turn rest into its own char
                    paddings = 2; // We left too early
                    continue;
                }

                byte getFour = (byte)(bytes[i + 1] >> 4);
                getSix = (byte)(getFour | rest);
                rest = (byte)(bytes[i + 1] << 4);
                rest = (byte)(rest >> 2); // << 6 made it back to 8, but we need 6, so 2 leading zeros
                result.Append(_charset[getSix]);

                if (i + 2 >= bytes.Length)
                {
                    result.Append(rest); // There is no more to work with, so we turn rest into its own char
                    paddings = 1; // We left too early
                    continue;
                }

                byte getTwo = (byte)(bytes[i + 2] >> 6);
                getSix = (byte)(getTwo | rest);
                rest = (byte)(bytes[i + 2] << 2);
                rest = (byte)(rest >> 2); // << 6 made it back to 8, but we need 6, so 2 leading zeros

                result.Append(_charset[getSix]);

                // Rest already contains the last 6, so it can just be added ...
                result.Append(_charset[rest]);
            }

            for (int i = 0; i < paddings; i++)
            {
                result.Append(PADDING); // We left too early, but should keep the needed length, otherwise some part is missing. That's where the padding comes in
            }

            return result.ToString();
        }

        public static byte[] Decode(string str)
        {
            byte[] bytes;
            if (!TryDecode(str, out bytes))
            {
                throw new Exception("Invalid base64 string");
            }

            return bytes;
        }

        public static bool TryDecode(string str, out byte[] bytes)
        {
            List<byte> getBytes = new List<byte>();

            // Validate Base64. If it is wrong, no more action is needed
            if (!IsValid(str))
            {
                bytes = getBytes.ToArray();
                return false;
            }

            // Remove padding
            int len = str.Length;
            for (int i = 0; i < 2; i++)
            {
                if (str[len - i - 1] == PADDING)
                {
                    len--;
                }
                else
                {
                    break;
                }
            }

            // 6 + 6 + 6 + 6 = 24
            // 8 + 8 + 8 = 24
            // So we can work on 4 array items at the same time. Which means i < length, but instead of i++, we do i += 4

            for (int i = 0; i < len; i += 4)
            {
                // currByte (current byte) to avoid redundant call of GetCharsetBytes(...)
                byte currByte = GetCharsetBytes(str[i]);
                byte getEight = currByte;
                getEight = (byte)(getEight << 2); // Only contains first 6, we need 8. Last 2 bits are in the next element
                
                if (i + 1 >= len)
                {
                    getBytes.Add(getEight); // That's it. Just add to bytes ... There's nothing else
                    break;
                }

                currByte = GetCharsetBytes(str[i + 1]);
                byte getTwo = currByte;
                getTwo = (byte)(getTwo << 2); // Remove both leading zeros, they are false bits
                getTwo = (byte)(getTwo >> 6); // Get first 2, but move them to the end to merge them with getEight with Bitwise OR operation
                getEight = (byte)(getEight | getTwo);

                getBytes.Add(getEight); // Final byte, add to list ...

                byte getFour = currByte;
                getEight = (byte)(getFour << 4);
                 
                if (i + 2 >= len)
                {
                    getBytes.Add(getEight); // That's it. Just add to bytes ... There's nothing else
                    break;
                }

                currByte = GetCharsetBytes(str[i + 2]);
                getFour = currByte;
                getFour = (byte)(getFour << 2); // Remove both leading zeros, they are false bits
                getFour = (byte)(getFour >> 4); // Get first 4, but move them to the end to merge them with getEight with Bitwise OR operation
                getEight = (byte)(getEight | getFour);

                getBytes.Add(getEight);

                getTwo = currByte;
                getEight = (byte)(getTwo << 6);

                if (i + 3 >= len)
                {
                    getBytes.Add(getEight); // That's it. Just add to bytes ... There's nothing else
                    break;
                }

                currByte = GetCharsetBytes(str[i + 3]);
                byte getSix = currByte;
                getSix = (byte)(getSix << 2); // Remove both leading zeros, they are false bits
                getSix = (byte)(getSix >> 2); // Get first 6, but move them to the end to merge them with getEight with Bitwise OR operation
                getEight = (byte)(getEight | getSix);

                getBytes.Add(getEight);
            }

            bytes = getBytes.ToArray();
            return true;
        }

        private static byte GetCharsetBytes(char c)
        {
            for (byte i = 0; i < _charset.Count; i++)
            {
                if (_charset[i] == c)
                {
                    return i;
                }
            }

            return 0;
        }

        public static bool IsValid(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
            }

            int len = str.Length;

            for (int i = 0; i < 2; i++)
            {
                if (str[len - 1] == PADDING)
                {
                    len--;
                }
                else
                {
                    break;
                }

                if (len == 1)
                {
                    break;
                }
            }

            for (int i = 0; i < len; i++)
            {
                if (!_charset.ContainsValue(str[i]))
                {
                    return false;
                }
            }

            if (str[0] == PADDING)
            {
                return false;
            }

            return true;
        }
    }
}
