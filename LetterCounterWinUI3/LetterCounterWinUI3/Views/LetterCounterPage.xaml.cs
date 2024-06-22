using System;
using System.Text.RegularExpressions;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.UI.ViewManagement;
using Microsoft.UI.Xaml.Navigation;
using Windows.ApplicationModel.DataTransfer;
using Microsoft.Windows.ApplicationModel.Resources;
using LetterCounterWinUI3.Helpers;

namespace LetterCounterWinUI3.Views
{
    public sealed partial class LetterCounterPage : Page
    {
        string backedString = "";
        int easterEgg = 0;
        public bool isFirstLaunch = true;
        private ResourceLoader _resourceLoader;
        public LetterCounterPage()
        {
            this.InitializeComponent();
            _resourceLoader = new ResourceLoader();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (isFirstLaunch)
            {
                //AutoDetectTheme
                string theme = "Light";
                UISettings uiSettings = new UISettings();
                var backgroundColor = uiSettings.GetColorValue(UIColorType.Background);
                //if (backgroundColor == Windows.UI.Colors.Black)
                //    theme = "Dark";

                //if (theme == "Light")
                //{
                //    this.RequestedTheme = ElementTheme.Light;
                //    //ThemeButton.Content = "\uE706";

                //    var ttb = ApplicationView.GetForCurrentView().TitleBar;
                //    ttb.BackgroundColor = GeneralHelper.ConvertStringToColor("#F8F9FC");
                //    ttb.ForegroundColor = Windows.UI.Colors.Black;
                //    ttb.ButtonBackgroundColor = GeneralHelper.ConvertStringToColor("#F8F9FC");
                //    ttb.ButtonForegroundColor = Windows.UI.Colors.Black;
                //    ttb.InactiveBackgroundColor = GeneralHelper.ConvertStringToColor("#F8F9FC");
                //    ttb.InactiveForegroundColor = Windows.UI.Colors.Black;
                //    ttb.ButtonInactiveBackgroundColor = GeneralHelper.ConvertStringToColor("#F8F9FC");
                //    ttb.ButtonInactiveForegroundColor = Windows.UI.Colors.Black;

                //    ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size { Width = 400, Height = 450 });
                //}
                //else
                //{
                //    var ttb = ApplicationView.GetForCurrentView().TitleBar;
                //    ttb.BackgroundColor = GeneralHelper.ConvertStringToColor("#232735");
                //    ttb.ForegroundColor = Windows.UI.Colors.White;
                //    ttb.ButtonBackgroundColor = GeneralHelper.ConvertStringToColor("#232735");
                //    ttb.ButtonForegroundColor = Windows.UI.Colors.White;
                //    ttb.InactiveBackgroundColor = GeneralHelper.ConvertStringToColor("#232735");
                //    ttb.InactiveForegroundColor = Windows.UI.Colors.White;
                //    ttb.ButtonInactiveBackgroundColor = GeneralHelper.ConvertStringToColor("#232735");
                //    ttb.ButtonInactiveForegroundColor = Windows.UI.Colors.White;

                //    ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size { Width = 400, Height = 450 });
                //}
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

            PopupContent.Text = _resourceLoader.GetString("Copied");
            AlertNotification.Begin();
        }

        private async void RateButton_Click(object sender, RoutedEventArgs e)
        {
            await GeneralHelper.Rate("9WZDNCRDXTF1");
        }

        private async void CountButton_Click(object sender, RoutedEventArgs e)
        {
            letterNumber.Text = "";
            punctuationNumber.Text = "";
            chineseNumber.Text = "";
            englishNumber.Text = "";
            digitNumber.Text = "";
            englishWordNumber.Text = "";

            string str = myTextBox.Text;
            char[] ch = str.ToCharArray();

            //int letternumber = await ICountLetter.AllLetterCountAsync(ch);//�ַ�����
            //int chinesenumber = await ICountLetter.ChineseLetterCountAsync(str);//�����ַ�����
            //int englishwordnumber = await ICountLetter.EnglishWordCountAsync(str);//Ӣ�ﵥ������
            //int digitnumber = await ICountLetter.DigitCountAsync(ch);//��������
            //int punctuationnumber = await ICountLetter.PunctuationCount(ch);//�������

            int letternumber = allLetterCount(str);//�ַ�����
            int chinesenumber = chineseLetterCount(str);//�����ַ�����
            int englishwordnumber = englishWordCount(str);//Ӣ�ﵥ������
            int digitnumber = digitCount(str);//��������
            int punctuationnumber = punctuationCount(ch);//�������

            int englishnumber = englishCharacterCount(str);//Ӣ���ַ�����
            //letternumber = letternumber + digitnumber + punctuationnumber;

            //if (chinesenumber > 0 && englishwordnumber > 0)
            //    englishwordnumber = 0;

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
                    myTextBox.Text = _resourceLoader.GetString("EasterEgg");
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
                PopupContent.Text = _resourceLoader.GetString("Cleared");
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
                PopupContent.Text = _resourceLoader.GetString("Restored");
                AlertNotification.Begin();
            }
        }

        private async void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("https://icons8.com/"));
        }

        //Tobe Cleared
        //algorithm for counting letters
        public static int allLetterCount(string ch)
        {
            //char[] ch = str.ToCharArray();
            //int letterNumber = 0;
            //for (int i = 0; i < ch.Length; i++)
            //{
            //    if (Char.IsLetter(ch[i]))
            //    {
            //        letterNumber++;
            //    }
            //}

            // ʹ��Length���Ի�ȡ�ַ����ĳ��� Chatgpt
            int length = ch.Length;

            return length;
        }

        public static int chineseLetterCount(string str)
        {
            int chinesenumber = 0;
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
            //int englishWordNumber;
            //string[] words = str.Split(new char[] { ',', ' ', '.', '?', '!', ':', ';', '��', '(', ')', '[', ']', '{', '}', '"', '\'' });
            //if (str == "")
            //    englishWordNumber = 0;
            //else
            //    englishWordNumber = words.Length;

            //ChatGPT
            //ͳ��ƥ�����ĸ���
            Regex regex = new Regex(@"\b[a-zA-Z]+\b");
            int newEnglishWordCount = regex.Matches(str).Count;

            return newEnglishWordCount;
        }

        public static int digitCount(string str)
        {
            //int digitNumber = 0;
            //for (int i = 0; i < ch.Length; i++)
            //{
            //    if (Char.IsDigit(ch[i]))
            //    {
            //        digitNumber++;
            //    }
            //}

            // ʹ��������ʽƥ�䰢��������
            Regex regex = new Regex(@"\d");
            // ͳ��ƥ�����ĸ���
            int digitNumber = regex.Matches(str).Count;
            return digitNumber;
        }

        public static int punctuationCount(Char[] ch)
        {
            int punctuationNumber = 0;
            for (int i = 0; i < ch.Length; i++)
            {
                if (Char.IsPunctuation(ch[i]))
                {
                    punctuationNumber++;
                }
            }

            // ����һ��������ʽ��ƥ���Ǳ�����
            //string pattern = @"[,.!?:;""'`~\-_+=\[\]\(\)\{\}\|\\\/\*#@%&$<>^\s]";

            // ����һ��������ʽ��ƥ�����ĺ�Ӣ�ı�����
            //string pattern = @"[,.!?:;""'`~\-_+=\[\]\(\)\{\}\|\\\/\*#@%&$<>^\s]|[��������������������������������������������������������������]";

            //// ʹ��Regex.Matches��������������ƥ��Ľ��
            //MatchCollection matches = Regex.Matches(str, pattern);
            //punctuationNumber = matches.Count;

            return punctuationNumber;
        }

        public static int englishCharacterCount(string str)
        {
            //
            //����һ��������ʽ����ƥ��Ӣ���ַ�
            Regex regex = new Regex("[A-Za-z]");

            //ʹ��������ʽ���ַ����в�������ƥ���������ƥ���������
            int count = regex.Matches(str).Count;
            return count;
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
