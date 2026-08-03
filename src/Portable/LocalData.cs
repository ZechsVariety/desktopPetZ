using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Configuration;
using System.Xml;

namespace DesktopPet
{
    public class LocalData
    {
        Configuration AppConfiguration = null;
        KeyValueConfigurationCollection AppSettings = null;
		readonly bool isInstalled = false;

        public LocalData()
        {
            try
            {
                if (Program.IsApplicationInstalled())
                {
                    isInstalled = true;
                    //AppConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
                    AppConfiguration = ConfigurationManager.OpenMappedExeConfiguration(
                        new ExeConfigurationFileMap { ExeConfigFilename = "DesktopPet.config" }, ConfigurationUserLevel.None);
                }
                else
                {
                    AppConfiguration = ConfigurationManager.OpenMappedExeConfiguration(
                        new ExeConfigurationFileMap { ExeConfigFilename = "DesktopPet.config" }, ConfigurationUserLevel.None);
                }
                LoadSettings();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error opening settings: " + ex.Message, "Settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadSettings()
        {
            //var settings = AppConfiguration.AppSettings.Settings;
            foreach (SettingsProperty currentProperty in Properties.Settings.Default.Properties)
            {
                if (AppConfiguration.AppSettings.Settings[currentProperty.Name] == null)
                {
                    AppConfiguration.AppSettings.Settings.Add(currentProperty.Name, currentProperty.DefaultValue.ToString());
                }
            }
            AppSettings = AppConfiguration.AppSettings.Settings;
        }

        public void SetVolume(double volume)
        {
            int iVolume = (int)(volume * 100);
            if (iVolume.ToString() != AppSettings["Volume"].Value)
            {
                Properties.Settings.Default.Volume = iVolume;
                AppSettings["Volume"].Value = iVolume.ToString();
                Save();
            }
        }
        public float GetVolume()
        {
			int.TryParse(AppSettings["Volume"].Value, out int iVolume);
			return (float)(iVolume / 100.0);
        }

        public void SetScale(int pow2)
        {
            if (pow2.ToString() != AppSettings["Scale"].Value)
            {
                Properties.Settings.Default.Scale = pow2;
                AppSettings["Scale"].Value = pow2.ToString();
                Save();
            }
        }
        public int GetScale()
        {
            if (int.TryParse(AppSettings["Scale"].Value, out int iScale))
            {
                return iScale;
            }
            return 1;
        }

        public bool GetMultiscreen()
        {
            bool.TryParse(AppSettings["Multiscreen"].Value, out bool ret);
            return ret;
        }

        public void SetMultiscreen(bool multi)
        {
            if (multi.ToString() != AppSettings["Multiscreen"].Value)
            {
                Properties.Settings.Default.Multiscreen = multi;
                AppSettings["Multiscreen"].Value = multi.ToString();
                Save();
            }
        }

        public bool GetWindowForeground()
        {
            bool.TryParse(AppSettings["WinForeground"].Value, out bool ret);
            return ret;
        }

        public void SetWindowForeground(bool foreground)
        {
            if (foreground.ToString() != AppSettings["WinForeground"].Value)
            {
                Properties.Settings.Default.WinForeground = foreground;
                AppSettings["WinForeground"].Value = foreground.ToString();
                Save();
            }
        }

        public void SetStealTaskbarFocus(bool steal)
        {
            if (steal.ToString() != AppSettings["StealTaskbarFocus"].Value)
            {
                Properties.Settings.Default.WinForeground = steal;
                AppSettings["StealTaskbarFocus"].Value = steal.ToString();
                Save();
            }
        }

        public bool GetStealTaskbarFocus()
        {
            bool.TryParse(AppSettings["StealTaskbarFocus"].Value, out bool ret);
            return ret;
        }

        public int GetAutoStartPets()
        {
            int.TryParse(AppSettings["AutostartPets"].Value, out int ret);
            return Math.Max(1, ret);
        }

        public void SetAutoStartPets(int autostart)
        {
            if (autostart.ToString() != AppSettings["AutostartPets"].Value)
            {
                Properties.Settings.Default.AutostartPets = autostart;
                AppSettings["AutostartPets"].Value = autostart.ToString();
                Save();
            }
        }

        public void SetXml(string xml, string folder)
        {
            Properties.Settings.Default.xml = xml;
            AppSettings["xml"].Value = xml;
            Save();
        }

        public string GetXml()
        {
            return AppSettings["xml"].Value;
        }

        public string LoadXML()
        {
            //XmlSerializer mySerializer = new XmlSerializer(typeof(XmlData.RootNode));
            // To read the file, create a FileStream.
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);

            if (File.Exists(Application.StartupPath + "\\installpet.xml"))
            {
                string sXML = System.Text.Encoding.Default.GetString(File.ReadAllBytes(Application.StartupPath + "\\installpet.xml"));
                File.Delete(Application.StartupPath + "\\installpet.xml");
                writer.Write(sXML);
                SetXml(sXML, "");
                return sXML;
            }
            else if (Program.ArgumentLocalXML != "")
            {
                string sXML = System.Text.Encoding.Default.GetString(File.ReadAllBytes(Program.ArgumentLocalXML));
                writer.Write(sXML);
                return sXML;
            }
            else if (Program.ArgumentWebXML != "")
            {
                System.Net.WebClient client = new System.Net.WebClient();
                string sXML = client.DownloadString(Program.ArgumentWebXML);
                writer.Write(sXML);
                return sXML;
            }
            else
            {
                writer.Write(AppSettings["xml"].Value);
                return AppSettings["xml"].Value;
            }
        }

        /// <summary>
        /// Set the "multiXml" property, which is a concatenation of multiple xmls with a pipe (|) delimiter (ex: "[xml1]|[xml2]")
        /// It should save between sessions.
        /// </summary>
        /// <param name="xmls">List of xmls to be added</param>
        public void SetMultiXml(List<XmlDocument> xmls)
        {
            //concat string
            string multiXml = "";
            for (int i = 0; i < xmls.Count; i++)
            {
                //delimiter
                if(i != 0)
                    multiXml += "|";

                //add xml to string
                multiXml += xmls[i].OuterXml;
            }

            //set multiXml property
            Properties.Settings.Default.multiXml = multiXml;
            AppSettings["multiXml"].Value = multiXml;
            Save();
        }

        /// <summary>
        /// Get a random xml from the "multiXml" property
        /// </summary>
        /// <returns></returns>
        public string GetRandomXml()
        {
            //turn multiXml string into list of strings
            List<string> xmls = AppSettings["multiXml"].Value.Split('|').ToList();

            //get random index and return the chosen xml
            Random random = new Random();
            return xmls[random.Next(0, xmls.Count)];
        }

        /// <summary>
        /// Clear the "multiXml" property, so that the program knows a pet set is no longer selected (ie: if you select a singular pet)
        /// </summary>
        public void ClearMultiXml()
        {
            Properties.Settings.Default.multiXml = null;
            AppSettings["multiXml"].Value = null;
            Save();
        }

        public string GetImages()
        {
            return AppSettings["Images"].Value;
        }

        public void SetImages(string images)
        {
            Properties.Settings.Default.Images = images;
            AppSettings["Images"].Value = images;
            //Save();
        }

        public string GetIcon()
        {
            return AppSettings["Icon"].Value;
        }

        public void SetIcon(string icon)
        {
            Properties.Settings.Default.Icon = icon;
            AppSettings["Icon"].Value = icon;
            //Save();
        }

        public bool IsFirstBoot()
        {
            return false;
        }

        public delegate void MyFunction(object source, FileSystemEventArgs e);

        public void ListenOnXMLChanged(MyFunction f)
        {
            // not implemented in the portable version
        }

        public void ListenOnOptionsChanged(MyFunction f)
        {
            // not implemented in the portable version
        }

        private void Save()
        {
            if (isInstalled)
            {
                Properties.Settings.Default.Save();
                AppConfiguration.Save();
            }
            else
            {
                AppConfiguration.Save();
            }
        }
    }
}
