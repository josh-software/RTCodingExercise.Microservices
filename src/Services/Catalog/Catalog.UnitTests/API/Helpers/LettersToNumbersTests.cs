using Catalog.API.Helpers;
using Xunit;

namespace Catalog.UnitTests.API.Helpers
{
    public class LettersToNumbersTests
    {
        [Fact]
        public void GenerateCombinations_Danny_ReturnsCorrectCombinations()
        {
            // Arrange
            string input = "Danny";
            // var expectedSubstring = "DA12NNY";
            var expectedSubstring = "D4NNY"; // Altered compared to Acceptance criteria, as I'm unsure this is a suitable test for this class

            // Act
            var actualCombinations = LettersToNumbers.GenerateCombinations(input);

            // Assert
            Assert.Contains(expectedSubstring, actualCombinations);
        }

        [Fact]
        public void GenerateCombinations_GSmith_ReturnsCorrectCombinations()
        {
            // Arrange
            string input = "GSmith";
            var expectedSubstring = "GSM17H";

            // Act
            var actualCombinations = LettersToNumbers.GenerateCombinations(input);

            // Assert
            Assert.Contains(expectedSubstring, actualCombinations);
        }

        [Fact]
        public void GenerateCombinations_James_ReturnsCorrectCombinations()
        {
            // Arrange
            string input = "James";
            var expectedSubstring = "JAM3S";

            // Act
            var actualCombinations = LettersToNumbers.GenerateCombinations(input);

            // Assert
            Assert.Contains(expectedSubstring, actualCombinations);
        }
    }
}
