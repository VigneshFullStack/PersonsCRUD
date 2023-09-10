namespace CRUDTests
{
    public class UnitTest1
    {
        [Fact]
        public void TestAdd()
        {
            // Arrange - declaration of variables and collecting inputs
            Math math = new Math();
            int input1 = 10, input2 = 20;
            int expected = 30;

            // Act - calling the method, which method you would like to test
            int actual = math.Add(input1, input2);

            // Assert - comparing the expected value with the actual value
            Assert.Equal(expected, actual);
        }
    }
}