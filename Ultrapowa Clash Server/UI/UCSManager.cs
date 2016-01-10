using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using UCS.Core;
using UCS.Logic;
using UCS.Network;
using UCS.PacketProcessing;

//using MetroFramework.Forms;

namespace UCS
{
    public partial class UCSManager : Form
    {
        public int MessageNumber;

        public DateTime RunTimeDate = DateTime.Now;

        public UCSManager()
        {
            InitializeComponent();
        }

        //store fist start program date and time

        private void cmdSubmutBtn_Click(object sender, EventArgs e)
        {
            kServerConsole.SelectionStart = kServerConsole.Text.Length;
            kServerConsole.ScrollToCaret();
            if (EnterCmdTextBox.Text == "/clear")
            {
                kServerConsole.Clear();
            }
            else if (EnterCmdTextBox.Text == "/restart")
            {
                var mail = new AllianceMailStreamEntry();
                mail.SetId((int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                mail.SetSenderId(0);
                mail.SetSenderAvatarId(0);
                mail.SetSenderName("System Manager");
                mail.SetIsNew(0);
                mail.SetAllianceId(0);
                mail.SetAllianceBadgeData(1728059989);
                mail.SetAllianceName("Legendary Administrator");
                mail.SetMessage("System is about to restart in a few moments.");
                mail.SetSenderLevel(500);
                mail.SetSenderLeagueId(22);

                foreach (var onlinePlayer in ResourcesManager.GetOnlinePlayers())
                {
                    var pm = new GlobalChatLineMessage(onlinePlayer.GetClient());
                    var ps = new ShutdownStartedMessage(onlinePlayer.GetClient());
                    var p = new AvatarStreamEntryMessage(onlinePlayer.GetClient());
                    ps.SetCode(5);
                    p.SetAvatarStreamEntry(mail);
                    pm.SetChatMessage("System is about to restart in a few moments.");
                    pm.SetPlayerId(0);
                    pm.SetLeagueId(22);
                    pm.SetPlayerName("System Manager");
                    PacketManager.ProcessOutgoingPacket(p);
                    PacketManager.ProcessOutgoingPacket(ps);
                    PacketManager.ProcessOutgoingPacket(pm);
                }
                kServerConsole.Text += Environment.NewLine + "Message Send To Users And Server Is Restarting ...";
                Console.WriteLine("Message Send To Users And Server Is Restarting ...");
                Program.RestartProgram();
                StatusLabel.Text = "Status : Restart";
            }
            else if (EnterCmdTextBox.Text == "/info")
            {
                kServerConsole.Text += Environment.NewLine + @"Created By UCS Team
                    Aidid
                    Berkan
                    Iswuefel
                    Moien007
                    Tobiti";
            }
            else if (EnterCmdTextBox.Text == "/quit")
            {
                Environment.Exit(1);
            }
            else
            {
                kServerConsole.Text += Environment.NewLine + "unknown command = " + EnterCmdTextBox.Text +
                                       Environment.NewLine +
                                       "Only [clear] and [credits] and [quit] and [sv_start] command is available.";
            }
            EnterCmdTextBox.Clear();
        }

        private void ConfigEditor_Click(object sender, EventArgs e)
        {
        }

        private void EShutdown_Click(object sender, EventArgs e)
        {
            foreach (var onlinePlayer in ResourcesManager.GetOnlinePlayers())
            {
                var p = new ShutdownStartedMessage(onlinePlayer.GetClient());
                p.SetCode(5);
                PacketManager.ProcessOutgoingPacket(p);
            }
            StatusLabel.Text = "Status : Emengency Shutdown";
            Console.WriteLine("Emengency Shutdown Message has been send to the user");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadConfigFile();
            var hostName = Dns.GetHostName();
            var IP = Dns.GetHostByName(hostName).AddressList[0].ToString();
            ServerIPtextBox.Text = IP;
            MaximizeBox = false;
            MinimizeBox = false;
            tabPage3.Show();
            timer1.Enabled = true;
            timer2.Enabled = true;
            StatusLabel.Text = "Status : Normal";
            iInfoLabel.Text = "Auto Updating Information and More After 30Sec";
            SkipRemInfoUpdateBtn.Visible = true;
            StartRMSystemBtn.Enabled = true;
            PlayersMailBoxBtn.Enabled = true;
            GlobalChatBtn.Enabled = true;
            LoadPlayerProfileBtn.Enabled = true;
        }

        private void GlobalChatBtn_Click(object sender, EventArgs e)
        {
            var mail = new AllianceMailStreamEntry();
            foreach (var onlinePlayer in ResourcesManager.GetOnlinePlayers())
            {
                var pm = new GlobalChatLineMessage(onlinePlayer.GetClient());
                pm.SetChatMessage(CMessageTextBox.Text);
                pm.SetPlayerId(0);
                pm.SetLeagueId(22);
                pm.SetPlayerName(MessageSenderNameTextBox.Text);
                PacketManager.ProcessOutgoingPacket(pm);
            }
        }

        private void GuidePriBtn_Click(object sender, EventArgs e)
        {
            Process.Start(
                "http://ultrapowa.com/forum/showthread.php?43-Ultrapowa-Clash-Server-Official-Documentation&p=8090&viewfull=1#post8090");
        }

        private void loadConfigFile()
        {
            StartingDarkElixirTextBox.Text = ConfigurationManager.AppSettings["startingDarkElixir"];
            StartingElixirTextBox.Text = ConfigurationManager.AppSettings["startingElixir"];
            StartingGoldTextBox.Text = ConfigurationManager.AppSettings["startingGold"];
            StartingGemsTextBox.Text = ConfigurationManager.AppSettings["startingGems"];
            StartingLevelTextBox.Text = ConfigurationManager.AppSettings["startingLevel"];
            StartingTrophiesTextBox.Text = ConfigurationManager.AppSettings["startingTrophies"];
            StartingExpTextBox.Text = ConfigurationManager.AppSettings["startingExperience"];
            StartingShieldTimeTextBox.Text = ConfigurationManager.AppSettings["startingShieldTime"];
            ClientVersionTextBox.Text = ConfigurationManager.AppSettings["clientVersion"];
            UsePatchComboBox.Text =
                Convert.ToString(Convert.ToBoolean(ConfigurationManager.AppSettings["useCustomPatch"]));
            PatchingServerTextBox.Text = ConfigurationManager.AppSettings["patchingServer"];
            EnableMaintenanceModeComboBox.Text =
                Convert.ToString(Convert.ToBoolean(ConfigurationManager.AppSettings["maintenanceMode"]));
            MaintenanceTimeTextBox.Text = ConfigurationManager.AppSettings["maintenanceTimeleft"];
            DBConnectionTextBox.Text = ConfigurationManager.AppSettings["databaseConnectionName"];
            LoggingLevelTextBox.Text = ConfigurationManager.AppSettings["loggingLevel"];
            ActiveAPIManagerComboBox.Text =
                Convert.ToString(Convert.ToBoolean(ConfigurationManager.AppSettings["apiManager"]));
            oldClientUrlTextBox.Text = ConfigurationManager.AppSettings["oldClientVersion"];
            DebugModeComboBox.Text = Convert.ToString(Convert.ToBoolean(ConfigurationManager.AppSettings["debugMode"]));
            ServerDebugPortTextBox.Text = ConfigurationManager.AppSettings["debugPort"];
            ServerPortTextBox.Text = ConfigurationManager.AppSettings["serverPort"];
            PvEDiffComboBox.Text = ConfigurationManager.AppSettings["expertPve"];
            SaveThreadCountTextBox.Text = ConfigurationManager.AppSettings["saveThreadCount"];
        }

        private void LoadPlayerProfileBtn_Click(object sender, EventArgs e)
        {
            try
            {
                PlayerScoreTextBox.Enabled = true;
                PlayerTownhallLevelTextBox.Enabled = true;
                PlayerDiamondsTextBox.Enabled = true;
                PlayerAvatarNameTextBox.Enabled = true;
                PlayerExpTextBox.Enabled = true;
                PlayerTownhallLevelTextBox.Enabled = true;
                PlayerScoreTextBox.Enabled = true;

                PlayerScoreTextBox.Clear();
                PlayerTownhallLevelTextBox.Clear();
                PlayerDiamondsTextBox.Clear();
                PlayerAvatarNameTextBox.Clear();
                PlayerExpTextBox.Clear();
                PlayerTownhallLevelTextBox.Clear();

                PlayerScoreTextBox.Text +=
                    ResourcesManager.GetPlayer(long.Parse(PlayerIDBox.Text)).GetPlayerAvatar().GetScore();
                ;
                PlayerTownhallLevelTextBox.Text +=
                    ResourcesManager.GetPlayer(long.Parse(PlayerIDBox.Text)).GetPlayerAvatar().GetTownHallLevel() + 1;
                PlayerDiamondsTextBox.Text +=
                    ResourcesManager.GetPlayer(long.Parse(PlayerIDBox.Text)).GetPlayerAvatar().GetDiamonds();
                PlayerExpTextBox.Text +=
                    ResourcesManager.GetPlayer(long.Parse(PlayerIDBox.Text)).GetPlayerAvatar().GetAvatarLevel();
                PlayerAvatarNameTextBox.Text +=
                    ResourcesManager.GetPlayer(long.Parse(PlayerIDBox.Text)).GetPlayerAvatar().GetAvatarName();
                if (ResourcesManager.GetPlayer(long.Parse(PlayerIDBox.Text)).GetAccountStatus() == 0)
                {
                    SetUserStatusComboBox.Text = "Normal";
                }
                else if (ResourcesManager.GetPlayer(long.Parse(PlayerIDBox.Text)).GetAccountStatus() == 0)
                {
                    SetUserStatusComboBox.Text = "Banned";
                }
                if (ResourcesManager.GetPlayer(long.Parse(PlayerIDBox.Text)).GetAccountPrivileges() == 0)
                {
                    SetUserPriComboBox.Text = "Standard";
                }
                else if (ResourcesManager.GetPlayer(long.Parse(PlayerIDBox.Text)).GetAccountPrivileges() == 1)
                {
                    SetUserPriComboBox.Text = "Moderator";
                }
                else if (ResourcesManager.GetPlayer(long.Parse(PlayerIDBox.Text)).GetAccountPrivileges() == 2)
                {
                    SetUserPriComboBox.Text = "High Moderator";
                }
                else if (ResourcesManager.GetPlayer(long.Parse(PlayerIDBox.Text)).GetAccountPrivileges() == 4)
                {
                    SetUserPriComboBox.Text = "Administrator";
                }
                else if (ResourcesManager.GetPlayer(long.Parse(PlayerIDBox.Text)).GetAccountPrivileges() == 5)
                {
                    SetUserPriComboBox.Text = "Server Owner";
                }

                SetUserPriComboBox.Enabled = true;
                SetUserStatusComboBox.Enabled = true;

                UpdatePlayerProfileBtn.Enabled = true;
            }
            catch (Exception)
            {
                PlayerScoreTextBox.Enabled = false;
                PlayerTownhallLevelTextBox.Enabled = false;
                PlayerDiamondsTextBox.Enabled = false;
                PlayerAvatarNameTextBox.Enabled = false;
                PlayerExpTextBox.Enabled = false;
                UpdatePlayerProfileBtn.Enabled = false;
                MessageBox.Show("Can't Load Player Profile, This Player ID Not Found : " + PlayerIDBox.Text +
                                "\nCheck Player ID Your Copied. (Player ID Is Not Player Name)\nCopy Player ID from Player List.\nContact to Developers on Ultrapowa.com Forum.\nTry Again.");
            }
        }

        private void PlayerIDBox_Text(object sender, EventArgs e)
        {
            PlayerScoreTextBox.Enabled = false;
            PlayerTownhallLevelTextBox.Enabled = false;
            PlayerDiamondsTextBox.Enabled = false;
            PlayerAvatarNameTextBox.Enabled = false;
            PlayerExpTextBox.Enabled = false;
            UpdatePlayerProfileBtn.Enabled = false;
        }

        private void PlayersMailBoxBtn_Click(object sender, EventArgs e)
        {
            var mail = new AllianceMailStreamEntry();
            mail.SetId((int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
            mail.SetSenderId(0);
            mail.SetSenderAvatarId(0);
            mail.SetSenderName(MessageSenderNameTextBox.Text);
            mail.SetIsNew(0);
            mail.SetAllianceId(0);
            mail.SetAllianceBadgeData(1728059989);
            mail.SetAllianceName("System Admin");
            mail.SetMessage(CMessageTextBox.Text);
            mail.SetSenderLevel(500);
            mail.SetSenderLeagueId(22);

            foreach (var onlinePlayer in ResourcesManager.GetOnlinePlayers())
            {
                var p = new AvatarStreamEntryMessage(onlinePlayer.GetClient());
                p.SetAvatarStreamEntry(mail);
                PacketManager.ProcessOutgoingPacket(p);
            }
        }

        private void processConfigFile()
        {
            // ucsconf-editor 
            var doc = new XmlDocument();
            var path = "ucsconf.config";
            doc.Load(path);
            var ie = doc.SelectNodes("appSettings/add").GetEnumerator();

            while (ie.MoveNext())
            {
                if ((ie.Current as XmlNode).Attributes["key"].Value == "startingGems")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = StartingGemsTextBox.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "startingGold")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = StartingGoldTextBox.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "startingElixir")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = StartingElixirTextBox.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "startingDarkElixir")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = StartingDarkElixirTextBox.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "startingTrophies")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = StartingTrophiesTextBox.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "maintenanceMode")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = EnableMaintenanceModeComboBox.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "maintenanceTimeleft")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = MaintenanceTimeTextBox.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "startingShieldTime")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = StartingShieldTimeTextBox.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "clientVersion")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = ClientVersionTextBox.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "useCustomPatch")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = UsePatchComboBox.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "patchingServer")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = PatchingServerTextBox.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "databaseConnectionName")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = DBConnectionTextBox.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "loggingLevel")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = LoggingLevelTextBox.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "apiManager")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = ActiveAPIManagerComboBox.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "oldClientVersion")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = oldClientUrlTextBox.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "debugMode")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = DebugModeComboBox.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "debugPort")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = ServerDebugPortTextBox.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "serverPort")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = ServerPortTextBox.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "startingExperience")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = StartingExpTextBox.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "expertPve")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = PvEDiffComboBox.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "saveThreadCount")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = SaveThreadCountTextBox.Text;
                }
            }
            doc.Save(path);
            var title2 = "Done.";
            var message = "Config-file updated.";
            MessageBox.Show(message, title2, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ReloadConfig_Click(object sender, EventArgs e)
        {
            loadConfigFile();
        }

        private void RestartServer_Click(object sender, EventArgs e)
        {
            var mail = new AllianceMailStreamEntry();
            mail.SetId((int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
            mail.SetSenderId(0);
            mail.SetSenderAvatarId(0);
            mail.SetSenderName("System Manager");
            mail.SetIsNew(0);
            mail.SetAllianceId(0);
            mail.SetAllianceBadgeData(1728059989);
            mail.SetAllianceName("Legendary Administrator");
            mail.SetMessage("System is about to restart in a few moments.");
            mail.SetSenderLevel(500);
            mail.SetSenderLeagueId(22);
            foreach (var onlinePlayer in ResourcesManager.GetOnlinePlayers())
            {
                var pm = new GlobalChatLineMessage(onlinePlayer.GetClient());
                var ps = new ShutdownStartedMessage(onlinePlayer.GetClient());
                var p = new AvatarStreamEntryMessage(onlinePlayer.GetClient());
                ps.SetCode(5);
                p.SetAvatarStreamEntry(mail);
                pm.SetChatMessage("System is about to restart in a few moments.");
                pm.SetPlayerId(0);
                pm.SetLeagueId(22);
                pm.SetPlayerName("System Manager");
                PacketManager.ProcessOutgoingPacket(p);
                PacketManager.ProcessOutgoingPacket(ps);
                PacketManager.ProcessOutgoingPacket(pm);
            }
            StatusLabel.Text = "Status : Restart";
            kServerConsole.Text += Environment.NewLine + "Message Send To Users And Server Is Restarting ...";
            Console.WriteLine("Message Send To Users And Server Is Restarting ...");
            Program.RestartProgram();
        }

        private void RMTimer_Tick(object sender, EventArgs e)
        {
            if (MessageNumber == 0)
            {
                foreach (var onlinePlayer in ResourcesManager.GetOnlinePlayers())
                {
                    var pm = new GlobalChatLineMessage(onlinePlayer.GetClient());
                    pm.SetChatMessage(RMessageText1.Text);
                    pm.SetPlayerId(0);
                    pm.SetLeagueId(22);
                    pm.SetPlayerName(RSenderNameTextBox.Text);
                    PacketManager.ProcessOutgoingPacket(pm);
                }
                MessageNumber++;
            }
            else if (MessageNumber == 1)
            {
                foreach (var onlinePlayer in ResourcesManager.GetOnlinePlayers())
                {
                    var pm = new GlobalChatLineMessage(onlinePlayer.GetClient());
                    pm.SetChatMessage(RMessageText2.Text);
                    pm.SetPlayerId(0);
                    pm.SetLeagueId(22);
                    pm.SetPlayerName(RSenderNameTextBox.Text);
                    PacketManager.ProcessOutgoingPacket(pm);
                }
                MessageNumber++;
            }
            else if (MessageNumber == 2)
            {
                foreach (var onlinePlayer in ResourcesManager.GetOnlinePlayers())
                {
                    var pm = new GlobalChatLineMessage(onlinePlayer.GetClient());
                    pm.SetChatMessage(RMessageText3.Text);
                    pm.SetPlayerId(0);
                    pm.SetLeagueId(22);
                    pm.SetPlayerName(RSenderNameTextBox.Text);
                    PacketManager.ProcessOutgoingPacket(pm);
                }
                MessageNumber++;
            }
            else if (MessageNumber == 3)
            {
                foreach (var onlinePlayer in ResourcesManager.GetOnlinePlayers())
                {
                    var pm = new GlobalChatLineMessage(onlinePlayer.GetClient());
                    pm.SetChatMessage(RMessageText4.Text);
                    pm.SetPlayerId(0);
                    pm.SetLeagueId(22);
                    pm.SetPlayerName(RSenderNameTextBox.Text);
                    PacketManager.ProcessOutgoingPacket(pm);
                }
                MessageNumber = 0;
            }
        }

        private void ScrollToEndBtn_Click_1(object sender, EventArgs e)
        {
            kServerConsole.SelectionStart = kServerConsole.Text.Length;
            kServerConsole.ScrollToCaret();
        }

        private void serverstatusbtn_Click(object sender, EventArgs e)
        {
            var mail = new AllianceMailStreamEntry();
            mail.SetId((int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
            mail.SetSenderId(0);
            mail.SetSenderAvatarId(0);
            mail.SetSenderName("System Manager");
            mail.SetIsNew(0);
            mail.SetAllianceId(0);
            mail.SetAllianceBadgeData(1728059989);
            mail.SetAllianceName("Legendary Administrator");
            mail.SetMessage("Latest Server Status:\nConnected Players:" + ResourcesManager.GetConnectedClients().Count +
                            "\nIn Memory Alliances:" + ObjectManager.GetInMemoryAlliances().Count +
                            "\nIn Memory Levels:" + ResourcesManager.GetInMemoryLevels().Count);
            mail.SetSenderLeagueId(22);
            mail.SetSenderLevel(500);

            foreach (var onlinePlayer in ResourcesManager.GetOnlinePlayers())
            {
                var p = new AvatarStreamEntryMessage(onlinePlayer.GetClient());
                var pm = new GlobalChatLineMessage(onlinePlayer.GetClient());
                pm.SetChatMessage("Our current Server Status is now sent at your mailbox!");
                pm.SetPlayerId(0);
                pm.SetLeagueId(22);
                pm.SetPlayerName("System Manager");
                p.SetAvatarStreamEntry(mail);
                PacketManager.ProcessOutgoingPacket(p);
                PacketManager.ProcessOutgoingPacket(pm);
            }
        }

        private void SkipRemInfoUpdateBtn_Click(object sender, EventArgs e)
        {
            Updateinfo();
        }

        private void SpeedT_Click(object sender, EventArgs e)
        {
            var speeds = new double[5];
            for (var i = 0; i < 5; i++)
            {
                var jQueryFileSize = 261; //Size of File in KB.
                var client = new WebClient();
                var startTime = DateTime.Now;
                client.DownloadFile("http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.js", "speedtest.txt");
                var endTime = DateTime.Now;
                speeds[i] = Math.Round(jQueryFileSize/(endTime - startTime).TotalSeconds);
            }
            MessageBox.Show(string.Format("Download Speed: {0}KB/s", speeds.Average(), MessageBoxButtons.YesNo,
                MessageBoxIcon.Question));
            Console.WriteLine("Download Speed: {0}KB/s", speeds.Average());
            File.Delete("speedtest.txt");
        }

        private void StartRMSystemBtn_Click(object sender, EventArgs e)
        {
            RMTimer.Interval = int.Parse(SendMessageSecBox.Text)*1000;
            RMTimer.Enabled = true;
            StopRMSystemBtn.Enabled = true;
            StartRMSystemBtn.Enabled = false;
        }

        private void StopRMSystemBtn_Click(object sender, EventArgs e)
        {
            RMTimer.Enabled = false;
            StopRMSystemBtn.Enabled = false;
            StartRMSystemBtn.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Updateinfo();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            var RunTimeSpan = DateTime.Now - RunTimeDate;
            UpTimeTextBox.Text = RunTimeSpan.Days + " : " + RunTimeSpan.Hours + " : " + RunTimeSpan.Minutes + " : " +
                                 RunTimeSpan.Seconds;
        }

        private void UCSClick_Click(object sender, EventArgs e)
        {
            var dialogResult =
                MessageBox.Show("Need Help ? Connect With Us At http://ultrapowa.com/ or https://www.flamewall.net/",
                    "Need Help?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                var ucs = new ProcessStartInfo("http://ultrapowa.com/");
                Process.Start(ucs);
                var flamewall = new ProcessStartInfo("https:/www.flamewall.net/");
                Process.Start(flamewall);
            }
            else if (dialogResult == DialogResult.No)
            {
            }
        }

        private void UCSClick2_Click(object sender, EventArgs e)
        {
            var dialogResult =
                MessageBox.Show("Need Help ? Connect With Us At http://ultrapowa.com/ or https://www.flamewall.net/",
                    "Need Help?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                var ucs = new ProcessStartInfo("http://ultrapowa.com/");
                Process.Start(ucs);
                var flamewall = new ProcessStartInfo("https://www.flamewall.net/");
                Process.Start(flamewall);
            }
            else if (dialogResult == DialogResult.No)
            {
            }
        }

        private void updatecheck_Click(object sender, EventArgs e)
        {
            var downloadUrl = "";
            Version newVersion = null;
            var aboutUpdate = "";
            var xmlUrl = "https://www.flamewall.net/ucs/system.xml";
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(xmlUrl);
                reader.MoveToContent();
                var elementName = "";
                if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "appinfo"))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            elementName = reader.Name;
                        }
                        else
                        {
                            if ((reader.NodeType == XmlNodeType.Text) && reader.HasValue)
                                switch (elementName)
                                {
                                    case "version":
                                        newVersion = new Version(reader.Value);
                                        break;

                                    case "url":
                                        downloadUrl = reader.Value;
                                        break;

                                    case "about":
                                        aboutUpdate = reader.Value;
                                        break;
                                }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            var applicationVersion = Assembly.GetExecutingAssembly().GetName().Version;
            if (applicationVersion.CompareTo(newVersion) < 0)
            {
                StatusLabel.Text = "Status : New version available!";
                var str =
                    string.Format(
                        "New version found!\nYour version: {0}.\nNewest version: {1}. \nAdded in this version: {2}. ",
                        applicationVersion, newVersion, aboutUpdate);
                if (DialogResult.No !=
                    MessageBox.Show(str + "\nWould you like to download this update?", "Check for updates",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    try
                    {
                        Process.Start(downloadUrl);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
            else
            {
                MessageBox.Show("Your version: " + applicationVersion + "  is up to date.", "Check for Updates",
                    MessageBoxButtons.OK, MessageBoxIcon.None);
            }
        }

        private void UpdateConfig_Click(object sender, EventArgs e)
        {
            processConfigFile();
        }

        private void Updateinfo()
        {
            StatusTextBox.Clear();
            StatusTextBox.Text += "Online Players : " + ResourcesManager.GetOnlinePlayers().Count;
            StatusTextBox.Text += Environment.NewLine;
            StatusTextBox.Text += "In Memory Alliances : " + ObjectManager.GetInMemoryAlliances().Count;
            StatusTextBox.Text += Environment.NewLine;
            StatusTextBox.Text += "In Memory Levels : " + ResourcesManager.GetInMemoryLevels().Count;
            StatusTextBox.Text += Environment.NewLine;
            StatusTextBox.Text += "Established Connections : " + ResourcesManager.GetConnectedClients().Count;
            PlayerListDataGrid.Rows.Clear();
            foreach (var account in ResourcesManager.GetOnlinePlayers())
            {
                //PlayersListTextBox.Text += "Player Name : " + account.GetPlayerAvatar().GetAvatarName() + ", Player ID : " + account.GetPlayerAvatar().GetId() + System.Environment.NewLine;
                int PlayerNumAccess = account.GetAccountPrivileges();
                string PlayerAccess;
                if (PlayerNumAccess == 0)
                {
                    PlayerAccess = "Standard";
                }
                else if (PlayerNumAccess == 1)
                {
                    PlayerAccess = "Moderator";
                }
                else if (PlayerNumAccess == 2)
                {
                    PlayerAccess = "High Moderator";
                }
                else if (PlayerNumAccess == 3)
                {
                    PlayerAccess = "unused";
                }
                else if (PlayerNumAccess == 4)
                {
                    PlayerAccess = "Administrator";
                }
                else if (PlayerNumAccess == 5)
                {
                    PlayerAccess = "Server Owner";
                }
                else
                {
                    PlayerAccess = "ERROR";
                }
                ;
                var PlayerHaveClanNum = account.GetPlayerAvatar().GetAllianceId();
                string PlayerHaveClan;
                if (PlayerHaveClanNum == 0)
                {
                    PlayerHaveClan = "Clans #0";
                }
                else
                {
                    PlayerHaveClan = "Clans #" + PlayerHaveClanNum;
                }
                ;
                string kPlayerAccStatus;
                int PlayerAccStatusNum = account.GetAccountStatus();
                if (PlayerAccStatusNum == 0)
                {
                    kPlayerAccStatus = "Normal";
                }
                else if (PlayerAccStatusNum == 99)
                {
                    kPlayerAccStatus = "Banned";
                }
                else
                {
                    kPlayerAccStatus = "ERR0R";
                }

                PlayerListDataGrid.Rows.Add(account.GetPlayerAvatar().GetAvatarName(), account.GetPlayerAvatar().GetId(),
                    kPlayerAccStatus, PlayerAccess, PlayerHaveClan, account.GetPlayerAvatar().GetTownHallLevel() + 1);
            }
            ;
        }

        private void UpdatePlayerProfileBtn_Click(object sender, EventArgs e)
        {
            Console.Beep();
            if (SetUserPriComboBox.Text == "Standard")
            {
                ResourcesManager.GetPlayer(long.Parse(PlayerIDBox.Text)).SetAccountPrivileges(0);
            }
            else if (SetUserPriComboBox.Text == "Moderator")
            {
                ResourcesManager.GetPlayer(long.Parse(PlayerIDBox.Text)).SetAccountPrivileges(1);
            }
            else if (SetUserPriComboBox.Text == "High Moderator")
            {
                ResourcesManager.GetPlayer(long.Parse(PlayerIDBox.Text)).SetAccountPrivileges(2);
            }
            else if (SetUserPriComboBox.Text == "Administrator")
            {
                ResourcesManager.GetPlayer(long.Parse(PlayerIDBox.Text)).SetAccountPrivileges(4);
            }
            else if (SetUserPriComboBox.Text == "Server Owner")
            {
                ResourcesManager.GetPlayer(long.Parse(PlayerIDBox.Text)).SetAccountPrivileges(5);
            }
            if (SetUserStatusComboBox.Text == "Normal")
            {
                ResourcesManager.GetPlayer(long.Parse(PlayerIDBox.Text)).SetAccountStatus(0);
            }
            else if (SetUserStatusComboBox.Text == "Banned")
            {
                ResourcesManager.GetPlayer(long.Parse(PlayerIDBox.Text)).SetAccountStatus(99);
            }

            ResourcesManager.GetPlayer(long.Parse(PlayerIDBox.Text))
                .GetPlayerAvatar()
                .SetScore(int.Parse(PlayerScoreTextBox.Text));
            ResourcesManager.GetPlayer(long.Parse(PlayerIDBox.Text))
                .GetPlayerAvatar()
                .SetName(PlayerAvatarNameTextBox.Text);
            ResourcesManager.GetPlayer(long.Parse(PlayerIDBox.Text))
                .GetPlayerAvatar()
                .SetDiamonds(int.Parse(PlayerDiamondsTextBox.Text));
            ResourcesManager.GetPlayer(long.Parse(PlayerIDBox.Text))
                .GetPlayerAvatar()
                .SetTownHallLevel(int.Parse(PlayerTownhallLevelTextBox.Text) - 1);
            MessageBox.Show("Player Profile Updated.");
        }
    }
}