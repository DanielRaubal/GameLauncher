using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HtmlAgilityPack;
using LiveCharts;
using Microsoft.Win32;

namespace GamesLauncher
{



    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateData();

            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 5, 0);
            dispatcherTimer.Start();



        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            // code goes here
            UpdateData();
        }

        void UpdateData()
        {
            HtmlWeb web = new HtmlWeb();

            string TotalPagesElement2 = "//div[@class='app-stat']";

            string TotalPagesElement = "//tr";

            string Url = string.Concat("https://steamcharts.com/app/730");

            HtmlAgilityPack.HtmlDocument NowPage
                   = web.Load(Url);

            HtmlNodeCollection TableElements = NowPage.DocumentNode.SelectNodes(TotalPagesElement2);

            List<string> listOfPlayers = new List<string>();

            foreach (var item in TableElements)
            {
                string InnerText = item.InnerText.Replace(" ", "").Replace("\n", "").Replace("\r", "");
                string formattedString = string.Format("{0:#,###}", int.Parse(InnerText.Split()[2]));

                listOfPlayers.Add(formattedString);
            }

            PlayingThisHourText.Text = listOfPlayers[0];
            DailyPeakText.Text = listOfPlayers[1];
            AllTimePeakText.Text = listOfPlayers[2];
        }


        public static List<string> FindSteamLibraries()
        {
            List<string> libraries = new List<string>();

            RegistryKey steamKey = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam");
            if (steamKey != null)
            {
                string[] libraryKeys = steamKey.GetValueNames();
           

                foreach (string key in libraryKeys)
                {
                    Console.WriteLine(key);
                    if (key.StartsWith("BaseInstallFolder"))
                    {
                        string path = steamKey.GetValue(key) as string;
                        libraries.Add(path);
                        Console.WriteLine(path);
                    }
                }
            }

            return libraries;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(@"E:\SteamLibrary\steamapps\common\Counter-Strike Global Offensive\csgo.exe");
        }
    }
}
