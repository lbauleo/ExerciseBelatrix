using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace ExerciseBelatrix
{
    public static class JobLogger
    {
        public static bool LogToDatabase { get; set; }
        public static bool LogToFile { get; set; }
        public static bool LogToConsole { get; set; }
        public static bool LogMessages { get; set; }
        public static bool LogWarnings { get; set; }
        public static bool LogErrors { get; set; }

        public static void LogMessage(string message)
        {
            Log(message, true, false, false);
        }

        public static void LogWarning(string message)
        {
            Log(message, false, true, false);
        }

        public static void LogError(string message)
        {
            Log(message, false, false, true);
        }

        private static void Log(string msg, bool message, bool warning, bool error)
        {

            if (String.IsNullOrWhiteSpace(msg))
            {
                throw new Exception("Null or Empty Message");
            }

            if (!LogToConsole && !LogToFile && !LogToDatabase)
            {
                throw new Exception("Invalid configuration");
            }
            if ((!LogErrors && !LogMessages && !LogWarnings) || (!message && !warning && !error))
            {
                throw new Exception("Error or Warning or Message must be specified");
            }

            if (LogToDatabase)
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]))
                {
                    int t = 0;
                    if (message && LogMessages)
                    {
                        t = 1;
                    }
                    if (error && LogErrors)
                    {
                        t = 2;
                    }
                    if (warning && LogWarnings)
                    {
                        t = 3;
                    }

                    if (t > 0)
                    {
                        using (SqlCommand command = new SqlCommand("Insert into Log Values('" + message + "', " + t.ToString() + ")", connection))
                        {
                            try
                            {
                                connection.Open();
                                command.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                connection.Close();
                                throw new Exception("Error logging to database");
                            }
                        }
                    }
                }

            }

            if (LogToFile)
            {
                string l = null;


                if (error && LogErrors)
                {
                    l = "Err - " + DateTime.Now.ToShortDateString() + " - " + message;
                }
                if (warning && LogWarnings)
                {
                    l = "Wrn - " + DateTime.Now.ToShortDateString() + " - " + message;
                }
                if (message && LogMessages)
                {
                    l = "Msg - " + DateTime.Now.ToShortDateString() + " - " + message;
                }

                if (!String.IsNullOrEmpty(l))
                {
                    try
                    {
                        File.AppendAllText(ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt", l);

                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error logging to file");
                    }

                }
            }

            if (LogToConsole)
            {
                if (error && LogErrors)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                if (warning && LogWarnings)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                if (message && LogMessages)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine(DateTime.Now.ToShortDateString() + message);

                Console.ResetColor();
            }
        }
    }
}
