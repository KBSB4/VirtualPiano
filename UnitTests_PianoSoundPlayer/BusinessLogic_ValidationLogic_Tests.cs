using Controller;
using Model.DatabaseModels;

namespace UnitTests
{
    public class BusinessLogic_ValidationLogic_Tests
    {
        private readonly User? existingUser = await DatabaseController.GetUserByName();
        private readonly User? loggingInUser = await DatabaseController.GetLoggingInUser();
        [SetUp]
        public void SetUp()
        {

        }
        
        
        [TestCase("", )]
        private void AccountPage_Login_UsernameGetsCheckedOnEmpty(string username, User user)
        {

        }
    }
}