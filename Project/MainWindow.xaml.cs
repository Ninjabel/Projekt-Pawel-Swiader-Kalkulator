﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;

namespace Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CreateDirectory();
        }

        /// <summary>
        /// Variables
        /// </summary>
        int numOp = 0;
        float TheResult = 0;
        List<string> operStore = new List<string>();
        bool resultShowing = false;
        static string date = DateTime.Now.ToString("MM-dd-yyyy_HH-mm");
        int counter = 0;
        static string name = "save";
        static string dir = "CalculatorSavings";
        string path = dir + "\\" + date + "_" + name + ".txt";

        /// <summary>
        /// UI Events
        /// <summary>
        
        void Btn_num(object sender, RoutedEventArgs e) => NumberFunc(((Button)sender).Content.ToString());
        void Btn_dot(object sender, RoutedEventArgs e) => DotCheck();
        void Operator(object sender, RoutedEventArgs e) => CheckandCal(((Button)sender).Content.ToString());
        void Btn_equal(object sender, RoutedEventArgs e) => TheEnterKey();
        void Clear(object sender, RoutedEventArgs e) => ClearEverything();
        void Back(object sender, RoutedEventArgs e) => BackSpace();
        /// <remarks> 
        /// Function that allows using keyboard to enter numbers.
        /// </remarks>
        void Keyboard_press(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.NumPad1:
                case Key.D1:
                    NumberFunc("1"); break;
                case Key.NumPad2:
                case Key.D2:
                    NumberFunc("2"); break;
                case Key.NumPad3:
                case Key.D3:
                    NumberFunc("3"); break;
                case Key.NumPad4:
                case Key.D4:
                    NumberFunc("4"); break;
                case Key.NumPad5:
                case Key.D5:
                    NumberFunc("5"); break;
                case Key.NumPad6:
                case Key.D6:
                    NumberFunc("6"); break;
                case Key.NumPad7:
                case Key.D7:
                    NumberFunc("7"); break;
                case Key.NumPad8:
                case Key.D8:
                    NumberFunc("8"); break;
                case Key.NumPad9:
                case Key.D9:
                    NumberFunc("9"); break;
                case Key.NumPad0:
                case Key.D0:
                    NumberFunc("0"); break;

                case Key.Decimal: DotCheck(); break;
                case Key.Enter: TheEnterKey(); break;
                case Key.Back: BackSpace(); break;
                case Key.Escape: ClearEverything(); break;

                case Key.Add: CheckandCal("+"); break;
                case Key.Subtract: CheckandCal("-"); break;
                case Key.Multiply: CheckandCal("x"); break;
                case Key.Divide: CheckandCal("/"); break;
            }
        }

        /// <remarks> 
        /// Function that checks if a directory exists (for saves calculation results) and creates it.
        /// </remarks>
        private void CreateDirectory()
        {
            try
            {
                if (Directory.Exists(dir))
                {
                    return;
                }
                DirectoryInfo di = Directory.CreateDirectory(dir);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString());
            }
        }

        /// <remarks> 
        /// Function which displays numbers in TextBox named "boxResult".
        /// </remarks>
        private void NumberFunc(string? i)
        {
            if (boxResult.Text.Length == 1 && boxResult.Text.IndexOf("0") == 0)
                boxResult.Text = i;
            else if (resultShowing)
                boxResult.Text = i;
            else
                boxResult.Text += i;

            resultShowing = false;
        }

        /// <remarks>
        /// Function which put dot to TextBox named "boxResult".
        /// </remarks>
        private void DotCheck()
        {
            if (!boxResult.Text.Contains(".")) { boxResult.Text += "."; }
        }

        /// <remarks>
        /// Function which calculates and displays numbers and operators used to perform calculation.
        /// </remarks>
        private void CheckandCal(string? x)
        {
            if (resultShowing && boxMain.Text != "")
            {
                boxMain.Text = boxMain.Text.Substring(0, boxMain.Text.Length - 1) + x;
                if (x != null)
                operStore[numOp - 1] = x;
            }
            else
            {
                boxMain.Text += $" {boxResult.Text} {x}";
                if (x != null)
                operStore.Add(x);
                numOp += 1;
            }

            if (numOp == 1)
            {
                TheResult = float.Parse(boxMain.Text.Substring(0, boxMain.Text.Length - 2));
                resultShowing = true;
            }
            else if (numOp > 1 && !resultShowing)
            {
                TheResult = Calu(TheResult, operStore[numOp - 2], float.Parse(boxResult.Text));
                boxResult.Text = $"{TheResult}";
                resultShowing = true;
            }
        }

        /// <remarks>
        /// Calculation function.
        /// </remarks>
        private float Calu(float x, string a, float y)
        {
            float resu;
            switch (a)
            {
                case "+":
                    resu = x + y; break;
                case "-":
                    resu = x - y; break;
                case "x":
                    resu = x * y; break;
                case "/":
                    resu = x / y; break;
                default: throw new Exception("Operator Error");
            }
            return resu;
        }

        /// <remarks>
        /// Function which deletes last typed number.
        /// </remarks>
        private void BackSpace()
        {
            if (resultShowing)
                boxResult.Text = "0";
            else if (boxResult.Text.Length > 1)
                boxResult.Text = boxResult.Text.Substring(0, boxResult.Text.Length - 1);
            else
                boxResult.Text = "0";
        }

        /// <remarks>
        /// Function which displays calculations in TextBox named "boxResult"
        /// </remarks>
        private void TheEnterKey()
        {
            if (resultShowing)
            {
                boxResult.Text = $"{TheResult}";
            }
            else if (numOp > 0)
            {
                float num1 = float.Parse(boxResult.Text);
                TheResult = Calu(TheResult, operStore[numOp - 1], num1);
                boxResult.Text = $"{TheResult}";
                resultShowing = true;
            }
            else
            {
                boxResult.Text = boxResult.Text;
            }
            numOp = 0;
            TheResult = 0;
            boxMain.Text = "";
            operStore.Clear();
        }

        /// <remarks>
        /// Function which clears everything which was typed and stored before.
        /// </remarks>
        private void ClearEverything()
        {
            numOp = 0;
            TheResult = 0;
            boxMain.Text = "";
            boxResult.Text = "0";
            operStore.Clear();
            resultShowing = false;
        }
        /// <remarks>
        /// Function which saves result of calculation to .txt file.
        /// </remarks>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    writer.WriteLine(boxResult.Text);
                }

                if (File.Exists(path))
                {
                counter++;
                string newPath = dir + "\\" + date + "_" + name + "(" + counter + ")" + ".txt";
                using (StreamWriter writer = new StreamWriter(newPath))
                    {
                        writer.WriteLine(boxResult.Text);
                    }
                }

                string messageBoxText = "The result was saved successfully.";
                string caption = "Save";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Information;
                MessageBoxResult result;
                result = MessageBox.Show(messageBoxText, caption, button, icon);
            
        }

        /// <remarks>
        /// Function which loads selected .txt file with saved calculation result.
        /// </remarks>
        private void Load_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.InitialDirectory = dir;
            dialog.FileName = "Document"; 
            dialog.DefaultExt = ".txt"; 
            dialog.Filter = "Text documents (.txt)|*.txt";
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                ClearEverything();
                string filename = dialog.FileName;
                string loaded = File.ReadLines(filename).First();
                boxResult.Text = loaded;
            }
        }
    }
}
