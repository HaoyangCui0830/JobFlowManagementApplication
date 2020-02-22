using System;
using NUnit.Framework;
using SWEN90013.ViewModels;

namespace Tests
{
    //This class is just some example of tests
    //To be deleted after we have actual tests
    [TestFixture]
    //Naming convention: ViewModelName/ObjectName followed by 'Test'
    public class SampleTest
    {
        //Global variables

        Tuple<int, int> testTuple;
    
        [SetUp]
        public void Setup()
        {
            //If there is something that all tests in this group will use,
            //it can be declared here
            testTuple = new Tuple<int, int>(5,5);
        }

        [Test]
        //Naming convention: MethodName_ExpectedInput_ExpectedOutput
        public void Add_PositiveNumbers_NumbersAdded()
        {
            //Arrange
            int expectedResult = 10;

            //Act
            int actualResult = testTuple.Item1 + testTuple.Item2;

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void Multiply_PositiveNumbers_NumbersMultiplied()
        {
            //Arrange
            int expectedResult = 25;

            //Act
            int actualResult = testTuple.Item1 * testTuple.Item2;

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}