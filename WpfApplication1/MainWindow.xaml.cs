using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace WpfApplication1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string _sword;
        static byte[] _bword;
        static string _skey;
        static byte[] _bkey;
        static string _smes;
        static byte[] _bmes;
        static List<byte[]> _ten = new List<byte[]>();

        Encoding enc = Encoding.Default;
        public MainWindow()
        {
            InitializeComponent();

        }
        public string input;
        public string output;

        public byte[] EncryptDecrypt(byte[] word, byte[] key)
        {

            List<byte> buf = new List<byte>();
            var zip = word.Zip(key, (w, k) => new { W = w, K = k });
            foreach (var i in zip)
            {
                buf.Add((byte)(i.W ^ i.K));
            }

            return buf.ToArray();
        }
        public static void GenerateKey(byte[] word)
        {

            Random random = new Random();
            _bkey = new byte[word.Length];
            random.NextBytes(_bkey);

        }
        //public void GenerateTen(byte[] word)
        //{
        //    //var b = BitConverter.ToUInt64(word, 0);
        //    //var bufWord = word.First();


        //    for (int i = 0; i < 10; i++)
        //    {
        //        var tst = word.Shift(i);
        //        _ten.Add(tst);
        //    }


        //    StringBuilder res = new StringBuilder();
        //    foreach (var item in _ten)
        //    {
        //        res.Append(item.PrintBytes());
        //        res.Append(Environment.NewLine);
        //    }


        //    var respr = res.ToString();

        //    TenKeyHolder.Text = respr;
        //    var desktop = Environment.SpecialFolder.DesktopDirectory.ToString();
        //    MessageBox.Show(desktop);
        //    File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)+"\\keys.txt", respr);
        //}
        //public void GetOne(string tenKeys)
        //{
        //    string[] separateKeys = tenKeys.Split(new String[] { Environment.NewLine }, StringSplitOptions.None);
        //    var buf = separateKeys[_tst].GetBytesFromPrintableString();

        //    BKey.Text = buf.PrintBytes();
        //}
        private void KeyGen_Click(object sender, RoutedEventArgs e)
        {
            GenerateKey(BWord.Text.GetBytesFromPrintableString());
            BKey.Text = _bkey.PrintBytes();

        }

        private void MoreKeyGen_Click(object sender, RoutedEventArgs e)
        {
            var coll = BKey.Text.GetBytesFromPrintableString().GenerateTen();
            StringBuilder sb = new StringBuilder();
            foreach (var key in coll)
            {
                sb.Append(key.PrintBytes());
                sb.Append(Environment.NewLine);
            }
            TenKeyHolder.Text = sb.ToString();
            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)+"\\keys.txt", sb.ToString());
        }
        private void RetrieveTen_Click(object sender, RoutedEventArgs e)
        {
            Window1 window1 = new Window1();
            window1.ShowDialog(); 
            
            List<byte[]> tenKeys = new List<byte[]>();
            string[] keyStrings = TenKeyHolder.Text.Split(new[] { Environment.NewLine },StringSplitOptions.RemoveEmptyEntries);
            foreach (var keey in keyStrings)
            {
               tenKeys.Add(keey.GetBytesFromPrintableString());
            }
            BKey.Text = tenKeys.GetOne(window1.ChosenKey.SelectedIndex).PrintBytes();
        }
        private void Word_TextChanged(object sender, TextChangedEventArgs e)
        {
            var t = Word.Text.GetBytes(enc).PrintBytes();
            if (t != (BWord.Text))
            {
                BWord.Text = t;
            }
        }

        private void Key_TextChanged(object sender, TextChangedEventArgs e)
        {
            var t = Key.Text.GetBytes(enc).PrintBytes();
            if (t != (BKey.Text))
            {
                BKey.Text = t;
            }
        }
        private void Message_TextChanged(object sender, TextChangedEventArgs e)
        {
            var t = Message.Text.GetBytes(enc).PrintBytes();
            if (t != (BMessage.Text))
            {
                BMessage.Text = t;
            }
        }
        private void BWord_TextChanged(object sender, TextChangedEventArgs e)
        {
            BWord.Text = BWord.Text.ToUpper();
            if (BWord.Text.Contains(" "))
            {
                BWord.Text = BWord.Text.Replace(" ", String.Empty);
            }

            if (BWord.Text.Length % 2 == 0)
            {
                var t = BWord.Text.GetBytesFromPrintableString().GetString(enc);
                if (t != (Word.Text))
                {
                    Word.Text = t;
                }
            }
        }
        private void BKey_TextChanged(object sender, TextChangedEventArgs e)
        {
            BKey.Text = BKey.Text.ToUpper();
            if (BKey.Text.Contains(" "))
            {
                BKey.Text = BKey.Text.Replace(" ", String.Empty);
            }
            if (BKey.Text.Length % 2 == 0)
            {
                var t = BKey.Text.GetBytesFromPrintableString().GetString(enc);
                if (t != (Key.Text))
                {
                    Key.Text = t;
                }
            }
        }

        private void BMessage_TextChanged(object sender, TextChangedEventArgs e)
        {
            BMessage.Text = BMessage.Text.ToUpper();
            if (BMessage.Text.Contains(" "))
            {
                BMessage.Text = BMessage.Text.Replace(" ", String.Empty);
            }

            if (BMessage.Text.Length % 2 == 0)
            {
                var t = BMessage.Text.GetBytesFromPrintableString().GetString(enc);
                if (t != (Message.Text))
                {
                    Message.Text = t;
                }
            }
        }

        private void Cypher_Click(object sender, RoutedEventArgs e)
        {
            _bmes = EncryptDecrypt(Word.Text.GetBytes(enc), Key.Text.GetBytes(enc));
            BMessage.Text = _bmes.PrintBytes();
        }

        private void Decypher_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(BMessage.Text))
            {
                _bword = EncryptDecrypt(Message.Text.GetBytes(enc), Key.Text.GetBytes(enc));
                BWord.Text = _bword.PrintBytes();
            }
        }

        private void KeyDecypher_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(BMessage.Text))
            {
                _bword = EncryptDecrypt(Message.Text.GetBytes(enc), Word.Text.GetBytes(enc));
                BKey.Text = _bword.PrintBytes();
            }
        }
    }
    public static class Ext
    {
        public static void AddLine(string str, FileStream fs)
        {
            var bytee = System.Text.Encoding.UTF8.GetBytes(str);
            fs.Write(bytee, 0, bytee.Length);
        }
    }
}
