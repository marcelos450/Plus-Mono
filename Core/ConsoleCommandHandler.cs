using System;
using log4net;
using Plus.HabboHotel;

using Plus.Communication.Packets.Outgoing.Moderation;

namespace Plus.Core
{
    public class ConsoleCommandHandler
    {
        private static readonly ILog log = LogManager.GetLogger("Plus.Core.ConsoleCommandHandler");

        public static void InvokeCommand(string inputData)
        {
            if (string.IsNullOrEmpty(inputData))
                return;

            try
            {
                #region Command parsing
                string[] parameters = inputData.Split(' ');

                switch (parameters[0].ToLower())
                {
                    #region stop
                    case "stop":
                    case "shutdown":
                        {
                            Logging.DisablePrimaryWriting(true);
                            Logging.WriteLine("The server is saving users furniture, rooms, etc. WAIT FOR THE SERVER TO CLOSE, DO NOT EXIT THE PROCESS IN TASK MANAGER!!", ConsoleColor.Yellow);
                            PlusEnvironment.PerformShutDown();
                            break;
                        }
                    #endregion

                    #region alert
                    case "alert":
                        {
                            string Notice = inputData.Substring(6);

                            PlusEnvironment.GetGame().GetClientManager().SendMessage(new BroadcastMessageAlertComposer(PlusEnvironment.GetGame().GetLanguageLocale().TryGetValue("console.noticefromadmin") + "\n\n" + Notice));

                            log.Info("Alert successfully sent.");
                            break;
                        }
                    #endregion

                    default:
                        {
                            log.Error(parameters[0].ToLower() + " is an unknown or unsupported command. Type help for more information");
                            break;
                        }
                }
                #endregion
            }
            catch (Exception e)
            {
                log.Error("Error in command [" + inputData + "]: " + e);
            }
        }
    }
}