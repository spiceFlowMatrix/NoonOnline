using Moq;
using System;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;
using Xunit;

namespace Training24Admin.UnitTest
{
    public class BasicTestCase
    {
        /// <summary>
        /// Unit test for check chapter require value
        /// Parms Chapter chapter = new Chapter()
        /// This will define chapter as null object and it's check null value of model data
        /// </summary>
        [Fact]
        public void Chapter_Require_Field()
        {
            //Chapter chapter = new Chapter{
            //    Name = "Test",
            //    Code = "A1592020"
            //};
            Chapter chapter = new Chapter();
            Assert.Throws<ArgumentException>(() => CheckIfNullOrEmpty(chapter));
        }

        [Fact]
        public void PassingTest()
        {
            //var calculator = new ICalculator();  
            //Assert.Equal(4, calculator.Add(2, 2));  

            Feedback feedback = new Feedback();
            var feedbackRepo = new Mock<EFFeedback>();
            feedbackRepo.Setup(x => x.Insert(feedback)).Returns(1);
            Assert.Equal(4, feedbackRepo.Object.Insert(feedback));
        }

        /// <summary>
        /// Method for checking null value of chapter model 
        /// Quiz Id, ItemOrder values are optional here
        /// </summary>
        /// <param name="chapter"></param>
        public void CheckIfNullOrEmpty(Chapter chapter)
        {
            if (string.IsNullOrEmpty(chapter.Name))
            {
                throw new ArgumentException();
            }
            if (string.IsNullOrEmpty(chapter.Code))
            {
                throw new ArgumentException();
            }
            if (chapter.CourseId == 0)
            {
                throw new ArgumentException();
            }
        }
    }
}
