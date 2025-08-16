using OldPhonePad.Core;

namespace OldPhonePad.Tests
{
    /// <summary>
    /// Comprehensive test suite for OldPhonePadConverter functionality.
    /// </summary>
    public class OldPhonePadConverterTests
    {
        #region Basic Functionality Tests

        [Fact]
        public void OldPhonePad_SingleKeyPress_ReturnsFirstCharacter()
        {
            // Arrange
            string input = "2#";

            // Act
            string result = OldPhonePadConverter.OldPhonePad(input);

            // Assert
            Assert.Equal("A", result);
        }

        [Fact]
        public void OldPhonePad_MultipleKeyPresses_CyclesThroughCharacters()
        {
            // Test pressing '2' three times should give 'C'
            Assert.Equal("C", OldPhonePadConverter.OldPhonePad("222#"));

            // Test pressing '3' twice should give 'E'
            Assert.Equal("E", OldPhonePadConverter.OldPhonePad("33#"));

            // Test pressing '7' four times should give 'S'
            Assert.Equal("S", OldPhonePadConverter.OldPhonePad("7777#"));
        }

        [Fact]
        public void OldPhonePad_DifferentKeys_ProducesMultipleCharacters()
        {
            // Arrange
            string input = "23#"; // 2='A', 3='D'

            // Act
            string result = OldPhonePadConverter.OldPhonePad(input);

            // Assert
            Assert.Equal("AD", result);
        }

        #endregion

        #region Provided Test Cases

        [Theory]
        [InlineData("33#", "E")]
        [InlineData("227*#", "B")]
        [InlineData("4433555 555666#", "HELLO")]
        [InlineData("8 88777444666*664#", "TURING")]
        public void OldPhonePad_ProvidedExamples_ReturnsExpectedOutput(string input, string expected)
        {
            // Act
            string result = OldPhonePadConverter.OldPhonePad(input);

            // Assert
            Assert.Equal(expected, result);
        }

        #endregion

        #region Space (Pause) Functionality Tests

        [Fact]
        public void OldPhonePad_SpaceBetweenSameKeys_AllowsRepeatingButton()
        {
            // Arrange
            string input = "222 2 22#"; // Should produce "CAB"

            // Act
            string result = OldPhonePadConverter.OldPhonePad(input);

            // Assert
            Assert.Equal("CAB", result);
        }

        [Fact]
        public void OldPhonePad_MultipleSpaces_TreatedAsSinglePause()
        {
            // Multiple spaces should still just separate sequences
            string input = "22  22#"; // Should produce "BB"

            string result = OldPhonePadConverter.OldPhonePad(input);

            Assert.Equal("BB", result);
        }

        #endregion

        #region Backspace Functionality Tests

        [Fact]
        public void OldPhonePad_BackspaceAfterCharacter_RemovesLastCharacter()
        {
            // Arrange
            string input = "23*#"; // Type 'A', 'D', then backspace

            // Act
            string result = OldPhonePadConverter.OldPhonePad(input);

            // Assert
            Assert.Equal("A", result);
        }

        [Fact]
        public void OldPhonePad_BackspaceOnEmpty_DoesNothing()
        {
            // Arrange
            string input = "*#"; // Backspace on empty

            // Act
            string result = OldPhonePadConverter.OldPhonePad(input);

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void OldPhonePad_MultipleBackspaces_RemovesMultipleCharacters()
        {
            // Arrange
            string input = "234**#"; // Type 'A', 'D', 'G', then backspace twice

            // Act
            string result = OldPhonePadConverter.OldPhonePad(input);

            // Assert
            Assert.Equal("A", result);
        }

        #endregion

        #region Edge Cases and Boundary Tests

        [Fact]
        public void OldPhonePad_EmptyInput_ThrowsArgumentException()
        {
            // Arrange
            string input = "";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => OldPhonePadConverter.OldPhonePad(input));
        }

        [Fact]
        public void OldPhonePad_NullInput_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => OldPhonePadConverter.OldPhonePad(null!));
        }

        [Fact]
        public void OldPhonePad_NoSendKey_ThrowsArgumentException()
        {
            // Arrange
            string input = "223"; // Missing '#'

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => OldPhonePadConverter.OldPhonePad(input));
            Assert.Contains("send button", exception.Message);
        }

        [Fact]
        public void OldPhonePad_OnlySendKey_ReturnsEmptyString()
        {
            // Arrange
            string input = "#";

            // Act
            string result = OldPhonePadConverter.OldPhonePad(input);

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void OldPhonePad_InvalidCharacter_ThrowsArgumentException()
        {
            // Arrange
            string input = "2a3#"; // 'a' is invalid

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => OldPhonePadConverter.OldPhonePad(input));
            Assert.Contains("Invalid character", exception.Message);
        }

        #endregion

        #region Cycling and Wrapping Tests

        [Fact]
        public void OldPhonePad_ExcessivePresses_CyclesThrough()
        {
            // Pressing '2' 4 times should cycle back to 'A' (A-B-C-A)
            Assert.Equal("A", OldPhonePadConverter.OldPhonePad("2222#"));

            // Pressing '2' 5 times should give 'B'
            Assert.Equal("B", OldPhonePadConverter.OldPhonePad("22222#"));

            // Pressing '7' 5 times should cycle to 'P' (P-Q-R-S-P)
            Assert.Equal("P", OldPhonePadConverter.OldPhonePad("77777#"));
        }

        #endregion

        #region Special Keys Tests

        [Fact]
        public void OldPhonePad_ZeroKey_ProducesSpace()
        {
            // Arrange
            string input = "203#"; // Should produce "A D"

            // Act
            string result = OldPhonePadConverter.OldPhonePad(input);

            // Assert
            Assert.Equal("A D", result);
        }

        [Fact]
        public void OldPhonePad_OneKey_ProducesSpecialCharacters()
        {
            // Test various presses of '1'
            Assert.Equal("&", OldPhonePadConverter.OldPhonePad("1#"));
            Assert.Equal("'", OldPhonePadConverter.OldPhonePad("11#"));
            Assert.Equal("(", OldPhonePadConverter.OldPhonePad("111#"));
            Assert.Equal("&", OldPhonePadConverter.OldPhonePad("1111#")); // Cycles back
        }

        #endregion

        #region Complex Scenarios

        [Fact]
        public void OldPhonePad_ComplexMessage_WorksCorrectly()
        {
            // "HELLO WORLD" = 4433555 555666 0 9666777555 3#
            string input = "4433555 555666 0 9666777555 3#";

            string result = OldPhonePadConverter.OldPhonePad(input);

            Assert.Equal("HELLO WORLD", result);
        }

        [Fact]
        public void OldPhonePad_MessageWithBackspaceCorrection_WorksCorrectly()
        {
            // Type "HELLO" but with mistakes and corrections
            // Type "HI" then backspace twice, then type "HELLO"
            string input = "44444**4433555 555666#";

            string result = OldPhonePadConverter.OldPhonePad(input);

            Assert.Equal("HELLO", result);
        }

        #endregion

        #region Extension Methods Tests

        [Theory]
        [InlineData("223#", true)]
        [InlineData("22 3*#", true)]
        [InlineData("#", true)]
        [InlineData("", false)]
        [InlineData(null, false)]
        [InlineData("223", false)] // Missing #
        [InlineData("22a3#", false)] // Invalid character
        public void IsValidOldPhonePadInput_VariousInputs_ReturnsExpectedResult(string? input, bool expected)
        {
            // Act
            bool result = OldPhonePadExtensions.IsValidOldPhonePadInput(input!);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ExplainConversion_ValidInput_ProvidesDetailedExplanation()
        {
            // Arrange
            string input = "22 3#";

            // Act
            string explanation = OldPhonePadExtensions.ExplainConversion(input);

            // Assert
            Assert.Contains("Input: 22 3#", explanation);
            Assert.Contains("Result: BD", explanation);
            Assert.Contains("Pause detected", explanation);
        }

        #endregion

        #region Zero Key Specific Tests

        [Fact]
        public void OldPhonePad_MultipleZeroKeys_ProducesMultipleSpaces()
        {
            // Arrange
            string input = "2000003#"; // Should produce "A     D"

            // Act
            string result = OldPhonePadConverter.OldPhonePad(input);

            // Assert
            Assert.Equal("A     D", result);
        }

        [Theory]
        [InlineData("20#", "A ")]
        [InlineData("200#", "A  ")]
        [InlineData("2000#", "A   ")]
        [InlineData("20000#", "A    ")]
        public void OldPhonePad_IncreasingZeros_ProducesProportionalSpaces(string input, string expected)
        {
            // Act
            string result = OldPhonePadConverter.OldPhonePad(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("222 0 222#", "C C")]  // C (pause) space (pause) C
        [InlineData("2220222#", "C C")]    // C space C
        public void OldPhonePad_ZerosVersusSpacePause_ProducesSimilarOutput(string input, string expected)
        {
            // Act
            string result = OldPhonePadConverter.OldPhonePad(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void OldPhonePad_LongSequenceOfZeros_ProducesEquivalentSpaces()
        {
            // Arrange
            string input = "20000000000000000003#"; // 2 followed by 18 zeros followed by 3

            // Act
            string result = OldPhonePadConverter.OldPhonePad(input);

            // Assert
            Assert.Equal(20, result.Length); // "A" + 18 spaces + "D" = 20 characters
            Assert.Equal("A", result[0].ToString());
            Assert.Equal("D", result[^1].ToString());

            // Check that all characters between first and last are spaces
            for (int i = 1; i < result.Length - 1; i++)
            {
                Assert.Equal(' ', result[i]);
            }
        }

        #endregion

        #region Zero Key Explanation Tests

        [Fact]
        public void ExplainConversion_MultipleZeros_ExplainsEachSeparately()
        {
            // Arrange
            string input = "2000#";

            // Act
            string explanation = OldPhonePadExtensions.ExplainConversion(input);

            // Assert
            Assert.Contains("Zero key pressed", explanation);
            Assert.Contains("Result: A   ", explanation);

            // Count occurrences of "Zero key pressed" - should be 3
            int count = 0;
            int index = 0;
            while ((index = explanation.IndexOf("Zero key pressed", index)) != -1)
            {
                count++;
                index += "Zero key pressed".Length;
            }

            Assert.Equal(3, count);
        }

        [Theory]
        [InlineData("20#", 1)]
        [InlineData("200#", 2)]
        [InlineData("2000#", 3)]
        public void ExplainConversion_CountsZeroKeyPresses_Correctly(string input, int expectedZeroPresses)
        {
            // Act
            string explanation = OldPhonePadExtensions.ExplainConversion(input);

            // Count occurrences of "Zero key pressed"
            int count = 0;
            int index = 0;
            while ((index = explanation.IndexOf("Zero key pressed", index)) != -1)
            {
                count++;
                index += "Zero key pressed".Length;
            }

            // Assert
            Assert.Equal(expectedZeroPresses, count);
        }

        #endregion
    }
}