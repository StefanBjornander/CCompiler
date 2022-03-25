using System.Text;
using System.Collections.Generic;

namespace CCompiler {
  class Slash {
    private static Dictionary<char,char> m_slashMap = new() {
                    // Key, ASCII value
      { '0', '\0'}, // Terminator, ASCII number 0
      {'a', '\a'},  // Alert (Beep, Bell), ASCII number 7 
      {'b', '\b'},  // Backspace, ASCII number 8 
      {'f', '\f'},  // Form Feed (Page Break), ASCII number 12
      {'n', '\n'},  // New Line (Line Feed), ASCII number 10
      {'r', '\r'},  // Carrige Return, ASCII number 13
      {'t', '\t'},  // Horizontal Tabulator, ASCII number 9
      {'v', '\v'},  // Vertical Tabulator, ASCII number 11
      {'\'', '\''}, // Single Quotation Mark, ASCII number 39
      {'\"', '\"'}, // Double Quotation Mark, ASCII number 34
      {'?', '?'},   // Question Mark, ASCII number 63
      {'\\', '\\'}  // Backslash, ASCII number 92
    };

    public static string SlashToChar(string text) {
      StringBuilder buffer = new(text);
      buffer.Append("\0\0\0");

      for (int index = 0; buffer[index] != '\0'; ++index) {
        if (buffer[index] == '\\') {
          char char1 = buffer[index + 1],
               char2 = buffer[index + 2],
               char3 = buffer[index + 3];

          if (m_slashMap.ContainsKey(char1)) {
            buffer.Remove(index, 2);
            buffer.Insert(index, m_slashMap[char1]);
          }
          else if (IsOctal(char1) && IsOctal(char2) && IsOctal(char3)) {
            int octValue = 64 * CharToOctal(char1) +
                            8 * CharToOctal(char2) +
                                CharToOctal(char3);
            Error.Check(octValue <= 255, Message.Invalid_octal_sequence);
            buffer.Remove(index, 4);
            buffer.Insert(index, (char) octValue);
          }
          else if (IsOctal(char1) && IsOctal(char2)) {
            int octValue = 8 * CharToOctal(char1) +
                               CharToOctal(char2);
            buffer.Remove(index, 3);
            buffer.Insert(index, (char)octValue);
          }
          else if (IsOctal(char1)) {
            int octValue = CharToOctal(char1);
            buffer.Remove(index, 2);
            buffer.Insert(index, (char) octValue);
          }
          else if (char.ToLower(char1) == 'x') {
            if (IsHex(char1) && IsHex(char2)) {
              int hexValue = 16 * CharToHex(char1) + CharToHex(char2);
              buffer.Remove(index, 3);
              buffer.Insert(index, (char) hexValue);
            }
            else if (IsHex(char1)) {
              int hexValue = CharToHex(char1);
              buffer.Remove(index, 2);
              buffer.Insert(index, (char) hexValue);
            }
            else {
              Error.Report(char1.ToString(),
                           Message.Invalid_hexadecimal_code);
            }
          }
          else {
            Error.Report(buffer[index + 1].ToString(),
                         Message.Invalid_slash_sequence);
          }
        }
      }

      buffer.Remove(buffer.Length - 3, 3);
      return buffer.ToString();
    }

    public static string CharToHex(string text) {
      StringBuilder buffer = new();

      for (int index = 0; index < text.Length; ++index) {
        char theChar = text[index];

        if (char.IsLetterOrDigit(theChar) || (theChar == '_')) {
          buffer.Append(theChar);
        }
        else {
          int asciiValue = (int) theChar;
          const string hexDigits = "0123456789ABCDEF";
          char hex1 = hexDigits[asciiValue / 16],
               hex2 = hexDigits[asciiValue % 16];
          buffer.Append(hex1.ToString() + hex2.ToString());
        }
      }

      return buffer.ToString();
    }

    private static bool IsOctal(char c) {
      return "01234567".Contains(c.ToString());
    }
  
    private static bool IsHex(char c) {
      return "0123456789abcdef".Contains(c.ToString().ToLower());
    }
  
    private static int CharToOctal(char c) {
      return "01234567".IndexOf(c);
    }
  
    private static int CharToHex(char c) {
      return "0123456789abcdef".IndexOf(c.ToString().ToLower());
    }
  }
}
