using System;
using Newtonsoft.Json;
using System.IO;
using DiscordRPC;
using DiscordRPC.Logging;
using System.Windows;

namespace PresenceSharp
{
    
    internal class Program
    {
        static void Main(string[] args)
        {
            string docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (!Directory.Exists(docs + "/PresenceSharp"))
            {
                Directory.CreateDirectory(docs + "/PresenceSharp");
            }
            if (!File.Exists(docs + "/PresenceSharp/run.json"))
            {
                File.Create(docs + "/PresenceSharp/run.json");
                Console.WriteLine("Welcome to PresenceSharp!");
                Console.WriteLine("To get started, make sure that you replace the contents in RPC.json with what you want it to say / have in the application.");
                Console.WriteLine("Tip: if you dont want something to be there, leave the JSON field blank");
            }
            try
            {
                Console.WriteLine("Made by 1 24 9");
                Initialize();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("There was a fatal error starting the RPC: " + ex.ToString());
            }
        }
        public static DiscordRpcClient client;

        private static void Initialize()
        {
            try
            {
                string DATA = File.ReadAllText("./RPC.json");
                LauncherCloud json = JsonConvert.DeserializeObject<LauncherCloud>(DATA);
                client = new DiscordRpcClient(json.clientid);
                client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };
                client.OnReady += (sender, e) =>
                {
                    Console.WriteLine("Received Ready from user {0}", e.User.Username);
                };
                client.OnPresenceUpdate += (sender, e) =>
                {
                    Console.WriteLine("Received Update! {0}", e.Presence);
                };
                client.Initialize();
                client.SetPresence(new RichPresence()
                {
                    Details = json.title,
                    State = json.subtitle,
                    Assets = new Assets()
                    {
                        LargeImageKey = json.largeimagename,
                        LargeImageText = json.largeimagetext,
                        SmallImageKey = json.smallimagename,
                        SmallImageText = json.smallimagetext
                    }
                });
                Console.WriteLine("RPC initialized! You can minimize this window now :)");
            }
            catch (Exception ex)
            {
                Console.WriteLine("There was a fatal error starting the RPC: " + ex.ToString());
            }
            
        }
        public class LauncherCloud
        {
            public string title { get; set; }
            public string subtitle { get; set; }
            public string clientid { get; set; }
            public string largeimagename { get; set; }
            public string largeimagetext { get; set; }
            public string smallimagename { get; set; }
            public string smallimagetext { get; set; }
        }
    }
}
