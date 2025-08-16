using System.Text;

namespace OldPhonePad.Core
{
    /// <summary>
    /// Extension methods for OldPhonePadConverter for additional functionality.
    /// </summary>
    public static class OldPhonePadExtensions
    {
        public static bool IsValidOldPhonePadInput(string input)
        {
            // Existing implementation is fine
            if (string.IsNullOrEmpty(input))
                return false;

            if (!input.EndsWith('#'))
                return false;

            foreach (char c in input)
            {
                if (c != '#' && c != '*' && c != ' ' && (c < '0' || c > '9'))
                    return false;
            }

            return true;
        }

        public static string ExplainConversion(string input)
        {
            if (!IsValidOldPhonePadInput(input))
                return "Invalid input format";

            var explanation = new StringBuilder();
            explanation.AppendLine($"Input: {input}");
            explanation.AppendLine("Processing steps:");

            string processInput = input[..^1];
            var currentSequence = new List<char>();
            char? previousKey = null;
            int stepNumber = 1;

            for (int i = 0; i < processInput.Length; i++)
            {
                char currentChar = processInput[i];

                if (currentChar == ' ')
                {
                    if (currentSequence.Count > 0)
                    {
                        explanation.AppendLine($"  Step {stepNumber++}: Pause detected - processing '{string.Join("", currentSequence)}'");
                        currentSequence.Clear();
                        previousKey = null;
                    }
                }
                else if (currentChar == '*')
                {
                    explanation.AppendLine($"  Step {stepNumber++}: Backspace key pressed");
                }
                else if (currentChar >= '0' && currentChar <= '9')
                {
                    if (previousKey.HasValue && previousKey.Value != currentChar)
                    {
                        if (currentSequence.Count > 0)
                        {
                            explanation.AppendLine($"  Step {stepNumber++}: New key pressed - processing '{string.Join("", currentSequence)}'");
                            currentSequence.Clear();
                        }
                    }
                    currentSequence.Add(currentChar);
                    previousKey = currentChar;
                }
            }

            if (currentSequence.Count > 0)
            {
                explanation.AppendLine($"  Step {stepNumber++}: End of input - processing '{string.Join("", currentSequence)}'");
            }

            explanation.AppendLine($"Result: {OldPhonePadConverter.OldPhonePad(input)}");
            return explanation.ToString();
        }

    }
}