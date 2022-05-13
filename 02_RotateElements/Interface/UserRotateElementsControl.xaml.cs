using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Autodesk.Revit.DB;

namespace RotateElements
{
    /// <summary>
    /// Логика взаимодействия для UserRotateElementsControl.xaml
    /// </summary>
    public partial class UserRotateElementsControl : Window
    {
        public string prevTextBox2 = "";
        public int prevIndexTextbox2 = 0;
        Document _doc;
        Element _elem1;
        Element _elem2;

        public UserRotateElementsControl(Document doc, Element elem1, Element elem2)
        {
            _doc = doc;
            _elem1 = elem1;
            _elem2 = elem2;
            InitializeComponent();
        }

        private void Button_Left(object sender, RoutedEventArgs e)
        {
            if (Check.IsChecked == true)
            {
                
            }

        }

        private void Button_Right(object sender, RoutedEventArgs e)
        {
            if (Check.IsChecked == true)
            {

            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void AngleText_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = (TextBox)sender;
            // Здесь возможно попадется десятичная запятая
            var f = CultureInfo.CurrentUICulture.NumberFormat;

            // Жестко задаем десятичную точку
            f = CultureInfo.GetCultureInfo("en-US").NumberFormat;

            var str = tb.Text;
            var regex = new Regex($"^\\{f.NegativeSign}?\\d*(\\{f.CurrencyDecimalSeparator}\\d*)?$");
            if (regex.IsMatch(str))
            {
                prevTextBox2 = str;
                prevIndexTextbox2 = tb.CaretIndex;
            }
            else
            {
                var savedPrevIndex = prevIndexTextbox2;
                tb.Text = prevTextBox2;
                prevIndexTextbox2 = savedPrevIndex;
                tb.CaretIndex = savedPrevIndex;
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
