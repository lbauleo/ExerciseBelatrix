using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExerciseBelatrix;
using System.Data.SqlClient;
using System.IO;

namespace ExerciseBelatrixUnitTests
{
    [TestClass]
    public class BelatrixExerciseTests
    {
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void EmptyMessageTest()
        {
            JobLogger.LogMessage("   ");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void InvalidConfigurationTest()
        {
            JobLogger.LogMessage("Log");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void FlagConfigurationTest()
        {
            JobLogger.LogToConsole = true;
            JobLogger.LogMessage("Log");
        }

        [TestMethod]
        public void ValidConfigurationTest()
        {
            JobLogger.LogToConsole = true;
            JobLogger.LogMessages = true;

            JobLogger.LogMessage("Log");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void LogToDataBaseTest()
        {
            JobLogger.LogToDatabase = true;
            JobLogger.LogMessages = true;

            JobLogger.LogMessage("Este es mi mensaje");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void LogToFileTest()
        {
            JobLogger.LogToFile = true;
            JobLogger.LogMessages = true;

            JobLogger.LogMessage("Este es mi mensaje");
        }
    }
}
