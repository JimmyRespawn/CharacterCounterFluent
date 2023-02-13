using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ViewManagement;
using System.Text.RegularExpressions;
using Windows.Foundation.Metadata;
using Windows.ApplicationModel.DataTransfer;
using CharacterCounter.Services;
using CharacterCounter.Helpers;

namespace CharacterCounter
{
    public sealed partial class MainPage : Page
    {
        string backedString = "";
        int easterEgg = 0;
        public bool isFirstLaunch = true;

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (isFirstLaunch)
            {
                //AutoDetectTheme
                string theme = "Light";
                UISettings uiSettings = new UISettings();
                var backgroundColor = uiSettings.GetColorValue(UIColorType.Background);
                if (backgroundColor == Windows.UI.Colors.Black)
                    theme = "Dark";

                if (theme=="Light")
                {
                    this.RequestedTheme = ElementTheme.Light;
                    //ThemeButton.Content = "\uE706";

                    var ttb = ApplicationView.GetForCurrentView().TitleBar;
                    ttb.BackgroundColor = GeneralHelper.ConvertStringToColor("#F8F9FC");
                    ttb.ForegroundColor = Windows.UI.Colors.Black;
                    ttb.ButtonBackgroundColor = GeneralHelper.ConvertStringToColor("#F8F9FC");
                    ttb.ButtonForegroundColor = Windows.UI.Colors.Black;
                    ttb.InactiveBackgroundColor = GeneralHelper.ConvertStringToColor("#F8F9FC");
                    ttb.InactiveForegroundColor = Windows.UI.Colors.Black;
                    ttb.ButtonInactiveBackgroundColor = GeneralHelper.ConvertStringToColor("#F8F9FC");
                    ttb.ButtonInactiveForegroundColor = Windows.UI.Colors.Black;

                    ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size { Width = 400, Height = 450 });

                    if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
                    {
                        Windows.UI.ViewManagement.StatusBar.GetForCurrentView().BackgroundColor = GeneralHelper.ConvertStringToColor("#F8F9FC");
                        Windows.UI.ViewManagement.StatusBar.GetForCurrentView().BackgroundOpacity = 1;
                        Windows.UI.ViewManagement.StatusBar.GetForCurrentView().ForegroundColor = Windows.UI.Colors.Black;
                    }
                }
                else
                {
                    var ttb = ApplicationView.GetForCurrentView().TitleBar;
                    ttb.BackgroundColor = GeneralHelper.ConvertStringToColor("#232735");
                    ttb.ForegroundColor = Windows.UI.Colors.White;
                    ttb.ButtonBackgroundColor = GeneralHelper.ConvertStringToColor("#232735");
                    ttb.ButtonForegroundColor = Windows.UI.Colors.White;
                    ttb.InactiveBackgroundColor = GeneralHelper.ConvertStringToColor("#232735");
                    ttb.InactiveForegroundColor = Windows.UI.Colors.White;
                    ttb.ButtonInactiveBackgroundColor = GeneralHelper.ConvertStringToColor("#232735");
                    ttb.ButtonInactiveForegroundColor = Windows.UI.Colors.White;

                    ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size { Width = 400, Height = 450 });

                    //Detect Phone status bar
                    if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
                    {
                        Windows.UI.ViewManagement.StatusBar.GetForCurrentView().BackgroundColor = GeneralHelper.ConvertStringToColor("#232735");
                        Windows.UI.ViewManagement.StatusBar.GetForCurrentView().BackgroundOpacity = 1;
                        Windows.UI.ViewManagement.StatusBar.GetForCurrentView().ForegroundColor = Windows.UI.Colors.White;
                    }
                }

                //Clipboard.ContentChanged += async (s, ex) =>
                //{
                //    DataPackageView dataPackageView = Clipboard.GetContent();
                //    if (dataPackageView.Contains(StandardDataFormats.Text))
                //    {
                //        string text = await dataPackageView.GetTextAsync();
                //        // To output the text from this example, you need a TextBlock control
                //        PopupContent.Text = "黏贴内容为：" + text;
                //        //AlertNotification.Begin();
                //    }
                //};
            }
            isFirstLaunch = false;
        }
        
        private void CopyFullTextButton_Click(object sender, RoutedEventArgs e)
        {
            string copy = myTextBox.Text;

            DataPackage dataPackage = new DataPackage();
            dataPackage.RequestedOperation = DataPackageOperation.Copy;
            dataPackage.SetText(copy);
            Clipboard.SetContent(dataPackage);
            
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            PopupContent.Text = resourceLoader.GetString("Copied");
            AlertNotification.Begin();
        }

        private async void RateButton_Click(object sender, RoutedEventArgs e)
        {
            await GeneralHelper.Rate("9WZDNCRDXTF1");
        }

        private async void CountButton_Click(object sender, RoutedEventArgs e)
        {
            string str = myTextBox.Text;
            char[] ch = str.ToCharArray();

            //int letternumber = await ICountLetter.AllLetterCountAsync(ch);//字符总数
            //int chinesenumber = await ICountLetter.ChineseLetterCountAsync(str);//中文字符总数
            //int englishwordnumber = await ICountLetter.EnglishWordCountAsync(str);//英语单词总数
            //int digitnumber = await ICountLetter.DigitCountAsync(ch);//数字总数
            //int punctuationnumber = await ICountLetter.PunctuationCount(ch);//标点总数

            int letternumber = allLetterCount(ch);//字符总数
            int chinesenumber = chineseLetterCount(str);//中文字符总数
            int englishwordnumber = englishWordCount(str);//英语单词总数
            int digitnumber = digitCount(ch);//数字总数
            int punctuationnumber = punctuationCount(ch);//标点总数

            int englishnumber = letternumber - chinesenumber;//英语字符总数
            letternumber = letternumber + digitnumber + punctuationnumber;

            if (chinesenumber > 0 && englishwordnumber > 0)
                englishwordnumber = 0;

            if (letternumber != 0)
                letterNumber.Text = Convert.ToString(letternumber);
            if (punctuationnumber != 0)
                punctuationNumber.Text = Convert.ToString(punctuationnumber);
            if (chinesenumber != 0)
                chineseNumber.Text = Convert.ToString(chinesenumber);
            if (englishnumber != 0)
                englishNumber.Text = Convert.ToString(englishnumber);
            if (digitnumber != 0)
                digitNumber.Text = Convert.ToString(digitnumber);
            if (englishwordnumber != 0)
                englishWordNumber.Text = Convert.ToString(englishwordnumber);

            //Easter Egge Trigger
            if (letternumber == 0)
            {
                easterEgg++;
                if (easterEgg == 3)
                {
                    var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
                    myTextBox.Text = resourceLoader.GetString("EasterEgg");
                    easterEgg = 0;
                }
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearHisoryCount();
        }

        private void ClearButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ClearHisoryCount();
        }

        private void ClearHisoryCount()
        {
            if (myTextBox.Text != "")
            {
                backedString = myTextBox.Text;
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
                PopupContent.Text = resourceLoader.GetString("Cleared");
                AlertNotification.Begin();
            }

            letterNumber.Text = "";
            punctuationNumber.Text = "";
            chineseNumber.Text = "";
            englishNumber.Text = "";
            digitNumber.Text = "";
            englishWordNumber.Text = "";
            myTextBox.Text = "";
        }

        private void HoldingToRewind_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            if (myTextBox.Text == "" && backedString != "")
            {
                myTextBox.Text = backedString;
                backedString = "";
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
                PopupContent.Text = resourceLoader.GetString("Restored");
                AlertNotification.Begin();
            }
        }

        private async void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("https://icons8.com/"));
        }

        //Tobe Cleared
        //algorithm for counting letters
        public static int allLetterCount(char[] ch)
        {
            //char[] ch = str.ToCharArray();
            int letterNumber = 0;
            for (int i = 0; i < ch.Length; i++)
            {
                if (Char.IsLetter(ch[i]))
                {
                    letterNumber++;
                }
            }
            return letterNumber;
        }

        public static int chineseLetterCount(string str)
        {
            int chinesenumber = 0;

            //for (int i = 0; i < ch.Length; i++)
            //{
            //    for (int temp = 0X4e00; temp <= 0X9fa5; temp++)
            //    {
            //        if (ch[i] == Convert.ToChar(temp))
            //        {
            //            chinesenumber++;
            //        }
            //    }
            //}
            Regex regex = new Regex(@"^[\u4E00-\u9fbb]+$");
            for (int i = 0; i < str.Length; i++)
            {
                if (regex.IsMatch(str[i].ToString()))
                {
                    chinesenumber++;
                }
            }

            return chinesenumber;
        }

        public static int englishWordCount(string str)
        {
            int englishWordNumber;
            string[] words = str.Split(new char[] { ',', ' ', '.', '?', '!', ':', ';', '—', '(', ')', '[', ']', '{', '}', '"', '\'' });
            if (str == "")
                englishWordNumber = 0;
            else
                englishWordNumber = words.Length;
            return englishWordNumber;
        }

        public static int digitCount(char[] ch)
        {
            int digitNumber = 0;
            for (int i = 0; i < ch.Length; i++)
            {
                if (Char.IsDigit(ch[i]))
                {
                    digitNumber++;
                }
            }
            return digitNumber;
        }

        public static int punctuationCount(char[] ch)
        {
            int punctuationNumber = 0;
            for (int i = 0; i < ch.Length; i++)
            {
                if (Char.IsPunctuation(ch[i]))
                {
                    punctuationNumber++;
                }
            }
            return punctuationNumber;
        }

        //private void ThemeButton_Click(object sender, RoutedEventArgs e)
        //{
        //    Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        //    if (this.RequestedTheme == ElementTheme.Dark)
        //    {
        //        this.RequestedTheme = ElementTheme.Light;
        //        localSettings.Values["theme"] = "1";
        //        ThemeButton.Content = "\uE706";
        //        var ttb = ApplicationView.GetForCurrentView().TitleBar;
        //        ttb.BackgroundColor = ConvertStringToColor("#F8F9FC");
        //        ttb.ForegroundColor = Windows.UI.Colors.Black;
        //        ttb.ButtonBackgroundColor = ConvertStringToColor("#F8F9FC");
        //        ttb.ButtonForegroundColor = Windows.UI.Colors.Black;
        //        ttb.InactiveBackgroundColor = ConvertStringToColor("#F8F9FC");
        //        ttb.InactiveForegroundColor = Windows.UI.Colors.Black;
        //        ttb.ButtonInactiveBackgroundColor = ConvertStringToColor("#F8F9FC");
        //        ttb.ButtonInactiveForegroundColor = Windows.UI.Colors.Black;

        //        ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size { Width = 400, Height = 450 });

        //        if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
        //        {
        //            Windows.UI.ViewManagement.StatusBar.GetForCurrentView().BackgroundColor = ConvertStringToColor("#F8F9FC");
        //            Windows.UI.ViewManagement.StatusBar.GetForCurrentView().BackgroundOpacity = 1;
        //            Windows.UI.ViewManagement.StatusBar.GetForCurrentView().ForegroundColor = Windows.UI.Colors.Black;
        //        }

        //    }
        //    else
        //    {
        //        this.RequestedTheme = ElementTheme.Dark;
        //        localSettings.Values.Remove("theme");
        //        ThemeButton.Content = "\uE708";


        //        var ttb = ApplicationView.GetForCurrentView().TitleBar;
        //        ttb.BackgroundColor = ConvertStringToColor("#232735");
        //        ttb.ForegroundColor = Windows.UI.Colors.White;
        //        ttb.ButtonBackgroundColor = ConvertStringToColor("#232735");
        //        ttb.ButtonForegroundColor = Windows.UI.Colors.White;
        //        ttb.InactiveBackgroundColor = ConvertStringToColor("#232735");
        //        ttb.InactiveForegroundColor = Windows.UI.Colors.White;
        //        ttb.ButtonInactiveBackgroundColor = ConvertStringToColor("#232735");
        //        ttb.ButtonInactiveForegroundColor = Windows.UI.Colors.White;

        //        ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size { Width = 400, Height = 450 });

        //        if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
        //        {
        //            Windows.UI.ViewManagement.StatusBar.GetForCurrentView().BackgroundColor = ConvertStringToColor("#232735");
        //            Windows.UI.ViewManagement.StatusBar.GetForCurrentView().BackgroundOpacity = 1;
        //            Windows.UI.ViewManagement.StatusBar.GetForCurrentView().ForegroundColor = Windows.UI.Colors.White;
        //        }
        //    }
        //}
    }
}