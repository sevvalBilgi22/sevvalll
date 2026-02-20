using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ConsoleApp53
{
    internal class Program
    {
       class VigenereCipher
    {
        // Constants for character sets
        const string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const int ASCII_START = 32;  // Space character
        const int ASCII_END = 126;   // Tilde character
        const int ASCII_RANGE = 95;  // Total characters (126-32+1)

        static void Main()
        {
            Console.WriteLine("Vigenère Cipher Encryption/Decryption System\n");

            // user input
            Console.Write("Enter the plaintext message: ");
            string plaintext = Console.ReadLine();

            Console.Write("Enter the encryption key: ");
            string key = Console.ReadLine();

            // Validate input, checking if the input is null
            if (string.IsNullOrEmpty(plaintext) || string.IsNullOrEmpty(key))
            {
                Console.WriteLine("Error: Plaintext and key cannot be empty!");
                return;
            }

            // Ask user for mode
            Console.WriteLine("\nSelect mode:");
            Console.WriteLine("1. Basic Mode (only letters A-Z, a-z)");
            Console.WriteLine("2. Advanced Mode (ASCII characters 32-126)");
            Console.Write("Enter your choice (1 or 2): ");
            string choice = Console.ReadLine();

            string encrypted = "";
            string decrypted = "";

            // Perform encryption/decryption based on mode
            if (choice == "1")
            {
                Console.WriteLine("\nBasic Mode (Alphabetic only)");
                encrypted = EncryptBasic(plaintext, key);
                decrypted = DecryptBasic(encrypted, key);
            }
            else if (choice == "2")
            {
                Console.WriteLine("\nAdvanced Mode (ASCII 32-126)");
                encrypted = EncryptAdvanced(plaintext, key);
                decrypted = DecryptAdvanced(encrypted, key);
            }
            else
            {
                Console.WriteLine("Invalid choice!");
                return;
            }

            // Display results
            Console.WriteLine($"\nOriginal text: {plaintext}");
            Console.WriteLine($"Key: {key}");
            Console.WriteLine($"Encrypted text: {encrypted}");
            Console.WriteLine($"Decrypted text: {decrypted}");

            // Verify
            if (plaintext == decrypted)
            {
                Console.WriteLine("\n!Decrypted text matches original!");
            }
            else
            {
                Console.WriteLine("\n!Decryption failed!");
            }
        }

        // BASIC MODE - Only alphabetic characters (A-Z, a-z)
        static string EncryptBasic(string text, string key)
        {
            StringBuilder result = new StringBuilder();
            key = key.ToUpper(); // Convert key to uppercase for consistency

            int keyIndex = 0;

            foreach (char c in text)
            {
                if (char.IsLetter(c))
                {
                    // Determine if character is uppercase or lowercase
                    char baseChar = char.IsUpper(c) ? 'A' : 'a';

                    // Get the corresponding key character (cyclically)
                    char keyChar = key[keyIndex % key.Length];

                    // Calculate shift: A=0, B=1, etc.
                    int shift = keyChar - 'A';

                    // Apply Vigenère encryption formula: E = (P + K) mod 26
                    char encryptedChar = (char)(((c - baseChar + shift) % 26) + baseChar);
                    result.Append(encryptedChar);

                    keyIndex++; // Move to next key character only for letters
                }
                else
                {
                    // Non-letters remain unchanged
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        static string DecryptBasic(string text, string key)
        {
            StringBuilder result = new StringBuilder();
            key = key.ToUpper();

            int keyIndex = 0;

            foreach (char c in text)
            {
                if (char.IsLetter(c))
                {
                    char baseChar = char.IsUpper(c) ? 'A' : 'a';
                    char keyChar = key[keyIndex % key.Length];

                    // Calculate shift: A=0, B=1, etc.
                    int shift = keyChar - 'A';

                    // Apply Vigenère decryption formula: D = (C - K + 26) mod 26, plaintext=(encrypted - key) mod 26
                    // Add 26 before mod to handle negative values
                    char decryptedChar = (char)(((c - baseChar - shift + 26) % 26) + baseChar);
                    result.Append(decryptedChar);

                    keyIndex++;
                }
                else
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        // ADVANCED MODE - All printable ASCII characters (32-126)
        static string EncryptAdvanced(string text, string key)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < text.Length; i++)
            {
                char textChar = text[i];
                char keyChar = key[i % key.Length];

                // Convert characters to positions in ASCII range 32-126
                int textPos = textChar - ASCII_START;
                int keyPos = keyChar - ASCII_START;

                // Apply Vigenère encryption: E = (T + K) mod range
                int encryptedPos = (textPos + keyPos) % ASCII_RANGE;

                // Convert back to character
                char encryptedChar = (char)(encryptedPos + ASCII_START);
                result.Append(encryptedChar);
            }

            return result.ToString();
        }

        static string DecryptAdvanced(string text, string key)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < text.Length; i++)
            {
                char textChar = text[i];
                char keyChar = key[i % key.Length];

                int textPos = textChar - ASCII_START;
                int keyPos = keyChar - ASCII_START;

                // Apply Vigenère decryption: D = (C - K + range) mod range
                int decryptedPos = (textPos - keyPos + ASCII_RANGE) % ASCII_RANGE;

                char decryptedChar = (char)(decryptedPos + ASCII_START);
                result.Append(decryptedChar);
            }

            return result.ToString();
        }
    }
}
}
