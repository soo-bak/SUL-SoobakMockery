using NUnit.Framework;
using SUL;

// XXX Consider namespace for classification structure in terms of hierarchy.
namespace SUL_Tests.Utilities {

[TestFixture]
  [Category("StringManipulation")]
  public class StringExtensionTests {

    [Test]
    public void Reverse_WithNonEmptyString_ReturnsReversedString() {
      // Arrange
      var original = "soobak";
      var expected = "kaboos";

      // Act
      string result = original.Reverse();
      
      // Assert
      Assert.AreEqual(expected, result);
    }

    [Test]
    public void Reverse_WithEmptyString_ReturnsEmptyString() {
      // Arrange
      var original = string.Empty;
      var expected = string.Empty;

      // Act
      string result = original.Reverse();

      // Assert
      Assert.AreEqual(expected, result);
    }

  }
}
