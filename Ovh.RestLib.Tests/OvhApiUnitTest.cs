using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;

namespace Ovh.RestLib.Tests
{
    /// <summary>
    /// Ovh Api unit test
    /// </summary>
    [TestClass]
    public class OvhApiUnitTest
    {
        /// <summary>
        /// Tests the constructor with invalid application key throw exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorWithInvalidApplicationKeyThrowException()
        {
            // ARRANGE
            const string applicationKey = null;
            const string applicationSecret = null;
            const string region = null;

            // ACT
            var ovhApi = new OvhApi(applicationKey, applicationSecret, region);
        }

        /// <summary>
        /// Tests the constructor with invalid application secret throw exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorWithInvalidApplicationSecretThrowException()
        {
            // ARRANGE
            const string applicationKey = "applicationKey";
            const string applicationSecret = null;
            const string region = null;

            // ACT
            var ovhApi = new OvhApi(applicationKey, applicationSecret, region);
        }

        /// <summary>
        /// Tests the constructor with invalid region throw exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorWithInvalidRegionThrowException()
        {
            // ARRANGE
            const string applicationKey = "applicationKey";
            const string applicationSecret = "applicationSecret";
            const string region = null;

            // ACT
            var ovhApi = new OvhApi(applicationKey, applicationSecret, region);
        }

        /// <summary>
        /// Tests the constructor with unknow region throw exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorWithUnknowRegionThrowException()
        {
            // ARRANGE
            const string applicationKey = "applicationKey";
            const string applicationSecret = "applicationSecret";
            const string region = "bad-region";

            // ACT
            var ovhApi = new OvhApi(applicationKey, applicationSecret, region);
        }

        /// <summary>
        /// Tests the constructor with valid arguments.
        /// </summary>
        [TestMethod]
        public void TestConstructorWithValidArguments()
        {
            // ARRANGE
            const string applicationKey = "applicationKey";
            const string applicationSecret = "applicationSecret";

            // ACT
            var ovhApi = new OvhApi(applicationKey, applicationSecret);

            // ASSERT
            Assert.IsNotNull(ovhApi, "instance is null");
            Assert.IsNotNull(ovhApi.ApplicationKey, "application key is null");
            Assert.IsNotNull(ovhApi.ApplicationSecret, "application secret is null");
            Assert.IsNotNull(ovhApi.EndPointName, "end point name is null");
            Assert.AreEqual(applicationKey, ovhApi.ApplicationKey, "application key is not the same has been set");
            Assert.AreEqual(applicationSecret, ovhApi.ApplicationSecret, "application secret is not the same has been set");
        }

        /// <summary>
        /// Tests the request consumer key with invalid key return string empty.
        /// </summary>
        [TestMethod]
        public void TestRequestConsumerKeyWithInvalidKeyReturnStringEmpty()
        {
            // ARRANGE
            const string applicationKey = "applicationKey";
            const string applicationSecret = "applicationSecret";
            var ovhApi = new OvhApi(applicationKey, applicationSecret);

            // ACT
            string requestConsumerKey = ovhApi.RequestConsumerKey();

            // ASSERT
            Assert.AreEqual(requestConsumerKey, string.Empty, "requestConsumerKey is not empty");
        }
    }
}
