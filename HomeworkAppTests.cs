using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class ResourceIntensiveAppTests
{
    [TestMethod]
    public void InitializeMemory_ShouldSetAllBytesToOne()
    {
        // Arrange
        byte[] memoryChunk = new byte[10];

        // Act
        ResourceIntensiveApp.InitializeMemory(memoryChunk);

        // Assert
        foreach (var b in memoryChunk)
        {
            Assert.AreEqual(1, b);
        }
    }
}