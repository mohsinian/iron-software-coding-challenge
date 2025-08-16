using System.Text;

namespace OldPhonePad.Core
{
    /// <summary>
    /// Converts old phone keypad input sequences to text messages.
    /// Based on T9 input method used in old mobile phones.
    /// </summary>
    public class OldPhonePadConverter
    {
        /// <summary>
        /// Mapping of phone keypad buttons to their corresponding characters.
        /// Each key press cycles through the available characters for that button.
        /// </summary>
        private static readonly Dictionary<char, string> KeypadMapping = new()
        {
            { '0', " " },
            { '1', "&'(" },
            { '2', "ABC" },
            { '3', "DEF" },
            { '4', "GHI" },
            { '5', "JKL" },
            { '6', "MNO" },
            { '7', "PQRS" },
            { '8', "TUV" },
            { '9', "WXYZ" }
        };

        private const char BackspaceKey = '*';
        private const char SendKey = '#';
        private const char SpaceIndicator = ' ';
        private const char ZeroKey = '0';

        /// <summary>
        /// Gets the character mapping for a specific key.
        /// </summary>
        public static string GetKeyMapping(char key)
        {
            KeypadMapping.TryGetValue(key, out string? mapping);
            return mapping ?? string.Empty;
        }

        /// <summary>
        /// Checks if the provided character has a key mapping.
        /// </summary>
        public static bool HasKeyMapping(char key)
        {
            return KeypadMapping.ContainsKey(key);
        }

        /// <summary>
        /// Converts an old phone keypad input string to the intended text message.
        /// </summary>
        /// <param name="input">The input string containing button presses, spaces for pauses,
        /// asterisks for backspace, and ending with '#' for send.</param>
        /// <returns>The decoded text message.</returns>
        /// <exception cref="ArgumentNullException">Thrown when input is null.</exception>
        /// <exception cref="ArgumentException">Thrown when input format is invalid.</exception>
        public static string OldPhonePad(string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input), "Input cannot be null.");

            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("Input cannot be empty.", nameof(input));

            if (!input.EndsWith(SendKey))
                throw new ArgumentException($"Input must end with '{SendKey}' (send button).", nameof(input));

            // Remove the send key for processing
            string processInput = input.Substring(0, input.Length - 1);

            return ProcessInput(processInput);
        }

        /// <summary>
        /// Processes the input string and converts it to text using a stack-based approach.
        /// </summary>
        private static string ProcessInput(string input)
        {
            var result = new StringBuilder();
            var keySequence = new Stack<char>();
            char? previousKey = null;

            for (int i = 0; i < input.Length; i++)
            {
                char currentChar = input[i];

                if (currentChar == SpaceIndicator)
                {
                    // Space indicates a pause - process any pending sequence
                    if (keySequence.Count > 0)
                    {
                        ProcessKeySequence(result, keySequence);
                        previousKey = null;
                    }
                }
                else if (currentChar == BackspaceKey)
                {
                    // Process any pending sequence first
                    if (keySequence.Count > 0)
                    {
                        ProcessKeySequence(result, keySequence);
                        previousKey = null;
                    }

                    // Remove last character if exists
                    if (result.Length > 0)
                    {
                        result.Length--;
                    }
                }
                else if (currentChar == ZeroKey)
                {
                    // Process any pending sequence first
                    if (keySequence.Count > 0)
                    {
                        ProcessKeySequence(result, keySequence);
                    }

                    // Add a space directly
                    result.Append(' ');
                    previousKey = null;
                }
                else if (KeypadMapping.ContainsKey(currentChar))
                {
                    // Check if we're continuing with the same key or starting a new one
                    if (previousKey.HasValue && previousKey.Value != currentChar)
                    {
                        // Different key pressed - process the previous sequence
                        if (keySequence.Count > 0)
                        {
                            ProcessKeySequence(result, keySequence);
                        }
                    }

                    keySequence.Push(currentChar);
                    previousKey = currentChar;
                }
                else
                {
                    throw new ArgumentException($"Invalid character '{currentChar}' in input. Only 0-9, *, #, and space are allowed.");
                }
            }

            // Process any remaining sequence
            if (keySequence.Count > 0)
            {
                ProcessKeySequence(result, keySequence);
            }

            return result.ToString();
        }

        /// <summary>
        /// Processes the key sequence stack and appends the corresponding character to the result.
        /// </summary>
        private static void ProcessKeySequence(StringBuilder result, Stack<char> keySequence)
        {
            if (keySequence.Count == 0) return;

            // Since all keys in the stack are the same, we can just peek at the top one
            char key = keySequence.Peek();
            int pressCount = keySequence.Count;

            // Clear the stack
            keySequence.Clear();

            if (!KeypadMapping.TryGetValue(key, out string? characters) || string.IsNullOrEmpty(characters))
                return;

            if (characters.Length == 0) return;

            // Calculate which character to use based on the number of presses
            // Use modulo to cycle through available characters
            int charIndex = (pressCount - 1) % characters.Length;
            result.Append(characters[charIndex]);
        }
    }
}