using OldPhonePad.Core;

namespace OldPhonePad.Demo
{
    /// <summary>
    /// Demo application showcasing the OldPhonePadConverter functionality.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===========================================");
            Console.WriteLine("    Old Phone Keypad Converter Demo");
            Console.WriteLine("===========================================\n");

            // Display keypad reference
            DisplayKeypadReference();

            // Run test cases from the challenge
            Console.WriteLine("\n📝 Running Test Cases from Challenge:\n");
            RunTestCases();

            // Interactive mode
            Console.WriteLine("\n🎮 Interactive Mode:\n");
            RunInteractiveMode();
        }

        static void DisplayKeypadReference()
        {
            Console.WriteLine("📱 Phone Keypad Reference:");
            Console.WriteLine("┌─────┬─────┬─────┐");
            Console.WriteLine("│  1  │  2  │  3  │");
            Console.WriteLine("│ &'( │ ABC │ DEF │");
            Console.WriteLine("├─────┼─────┼─────┤");
            Console.WriteLine("│  4  │  5  │  6  │");
            Console.WriteLine("│ GHI │ JKL │ MNO │");
            Console.WriteLine("├─────┼─────┼─────┤");
            Console.WriteLine("│  7  │  8  │  9  │");
            Console.WriteLine("│PQRS │ TUV │WXYZ │");
            Console.WriteLine("├─────┼─────┼─────┤");
            Console.WriteLine("│  *  │  0  │  #  │");
            Console.WriteLine("│ ← │space│ SEND│");
            Console.WriteLine("└─────┴─────┴─────┘");
            Console.WriteLine("\n💡 Tips:");
            Console.WriteLine("• Press a key multiple times to cycle through letters");
            Console.WriteLine("• Use space to pause between same-key letters");
            Console.WriteLine("• Use * for backspace");
            Console.WriteLine("• Always end with # to send");
        }

        static void RunTestCases()
        {
            var testCases = new[]
            {
                new { Input = "33#", Expected = "E", Description = "Simple: Press 3 twice" },
                new { Input = "227*#", Expected = "B", Description = "With backspace" },
                new { Input = "4433555 555666#", Expected = "HELLO", Description = "Word with pause" },
                new { Input = "8 88777444666*664#", Expected = "TURING", Description = "Complex with backspace" },
                new { Input = "222 2 22#", Expected = "CAB", Description = "Same key with pauses" },
                new { Input = "4433555 555666 0 9666777555 3#", Expected = "HELLO WORLD", Description = "Full sentence" }
            };

            foreach (var test in testCases)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"Test: {test.Description}");
                    Console.ResetColor();

                    Console.WriteLine($"Input:    {test.Input}");
                    Console.WriteLine($"Expected: {test.Expected}");

                    string result = OldPhonePadConverter.OldPhonePad(test.Input);
                    Console.Write($"Result:   {result} ");

                    if (result == test.Expected)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("✓ PASS");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("✗ FAIL");
                    }
                    Console.ResetColor();
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR: {ex.Message}");
                    Console.ResetColor();
                    Console.WriteLine();
                }
            }
        }

        static void RunInteractiveMode()
        {
            Console.WriteLine("Enter your own inputs to test (type 'exit' to quit):");
            Console.WriteLine("Remember to end each input with '#'\n");

            while (true)
            {
                Console.Write("Input: ");
                string? input = Console.ReadLine();

                if (input?.ToLower() == "exit")
                {
                    Console.WriteLine("\nThank you for using the Old Phone Keypad Converter!");
                    break;
                }

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Please enter a valid input.\n");
                    continue;
                }

                // Validate input
                if (!OldPhonePadExtensions.IsValidOldPhonePadInput(input))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("⚠️  Invalid input format. Make sure to:");
                    Console.WriteLine("   - Use only digits 0-9, space, * and #");
                    Console.WriteLine("   - End with # (send button)");
                    Console.ResetColor();
                    Console.WriteLine();
                    continue;
                }

                try
                {
                    string result = OldPhonePadConverter.OldPhonePad(input);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Output: {result}");
                    Console.ResetColor();

                    // Show detailed explanation for learning
                    Console.WriteLine("\nShow detailed explanation? (y/n): ");
                    if (Console.ReadKey().KeyChar.ToString().ToLower() == "y")
                    {
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine(OldPhonePadExtensions.ExplainConversion(input));
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine("\n");
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.ResetColor();
                    Console.WriteLine();
                }
            }
        }
    }
}