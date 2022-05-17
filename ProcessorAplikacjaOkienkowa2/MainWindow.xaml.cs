using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Procesor;
namespace ProcessorAplikacjaOkienkowa2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Rejestr> rejestry = new List<Rejestr>();
        string? operation;
        int left;
        int right;
        int baseLength;
        Rejestr[] DISP = new Rejestr[65536];
        bool ready = false;
        public MainWindow()
        {
            InitializeComponent();
            rejestry.Add(new Rejestr("al"));
            rejestry.Add(new Rejestr("ah"));
            rejestry.Add(new Rejestr("bl"));
            rejestry.Add(new Rejestr("bh"));
            rejestry.Add(new Rejestr("cl"));
            rejestry.Add(new Rejestr("ch"));
            rejestry.Add(new Rejestr("dl"));
            rejestry.Add(new Rejestr("dh"));
            baseLength = rejestry.Count;
            Random random = new Random();
            for (int i = 0; i < 65536; i++)
            {
                rejestry.Add(new Rejestr("AZ"));
                //DISP[i] = new Rejestr("AZ");
            }
            ready = true;
        }

        private void Losuj(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            foreach (var item in rejestry)
            {
                item.Value = random.Next(256).ToString();
            }
            for (int i = 0; i < 65536; i++)
            {
                DISP[i] = new Rejestr("AZ", random.Next(256));
            }
            BP.Text = random.Next(65536).ToString("x2").ToUpper();
            DI.Text = random.Next(65536).ToString("x2").ToUpper();
            SI.Text = random.Next(65536).ToString("x2").ToUpper();
            Aktualizuj();
        }
        private void Aktualizuj()
        {
            foreach (var item in rejestry)
            {
                if (item.Name == "AZ")
                    break;
                TextBox textBox = (TextBox)FindName(item.Name);
                textBox.Text = item.Value;
            }
            int index = int.Parse(DISP_ADDRESS.Text, System.Globalization.NumberStyles.HexNumber);
            DISP_VALUE.Text = rejestry[index + baseLength].Value;
        }
        private void Operation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            operation = SelectOperation.SelectedValue.ToString();
            if (operation == "INC" || operation == "DEC" || operation == "NOT" || operation == "NEG")
            {
                RightStackPanel.Visibility = Visibility.Hidden;
                RightStackPanelButton.Visibility = Visibility.Hidden;
                RightStackPanelDISP.Visibility = Visibility.Hidden;
                right = -1;
            }
            else
            {
                RightStackPanel.Visibility = Visibility.Visible;
                RightStackPanelButton.Visibility= Visibility.Visible;
            }

        }
        private void Left_SelectionChanged(object sender, SelectionChangedEventArgs e) => left = rejestry.FindIndex(e => e.Name == SelectLeft.SelectedValue.ToString());


        private void Right_SelectionChanged(object sender, SelectionChangedEventArgs e) => right = rejestry.FindIndex(e => e.Name == SelectRight.SelectedValue.ToString());


        private void Wykonaj(object sender, RoutedEventArgs e)
        {
            if (operation != null && left != -1)
            {
                switch (operation)
                {
                    case "MOV":
                        if (right != -1)
                            Rejestr.MOV(rejestry[left], rejestry[right]);
                        break;
                    case "XCHG":
                        if (right != -1)
                            Rejestr.XCHG(rejestry[left], rejestry[right]);
                        break;
                    case "INC":
                        var inc = rejestry[left];
                        Rejestr.INC(ref inc);
                        rejestry[left] = inc;
                        break;
                    case "DEC":
                        var dec = rejestry[left];
                        Rejestr.DEC(ref dec);
                        rejestry[left] = dec;
                        break;
                    case "NOT":
                        var not = rejestry[left];
                        Rejestr.NOT(ref not);
                        rejestry[left] = not;
                        break;
                    case "NEG":
                        var neg = rejestry[left];
                        Rejestr.NEG(ref neg);
                        rejestry[left] = neg;
                        break;
                    case "ADD":

                        if (right != -1)
                        {
                            var add = rejestry[left];
                            Rejestr.ADD(ref add, rejestry[right]);
                            rejestry[left] = add;
                        }
                        break;
                    case "SUB":
                        if (right != -1)
                        {
                            var sub = rejestry[left];
                            Rejestr.SUB(ref sub, rejestry[right]);
                            rejestry[left] = sub;
                        }
                        break;
                    case "AND":
                        if (right != -1)
                        {
                            var and = rejestry[left];
                            Rejestr.AND(ref and, rejestry[right]);
                            rejestry[left] = and;
                        }
                        break;
                    case "OR":
                        if (right != -1)
                        {
                            var or = rejestry[left];
                            Rejestr.OR(ref or, rejestry[right]);
                            rejestry[left] = or;
                        }
                        break;
                    case "XOR":
                        if (right != -1)
                        {
                            var xor = rejestry[left];
                            Rejestr.XOR(ref xor, rejestry[right]);
                            rejestry[left] = xor;
                        }
                        break;
                }
                Aktualizuj();
            }
        }
        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            if (textBox.Name != "DISP_ADDRESS" &&
                textBox.Name != "DIPS_VALUE")
            {
                var index = rejestry.FindIndex(e => e.Name == textBox.Name);
                try
                {
                    rejestry[index].Value = Convert.ToInt32(textBox.Text, 16).ToString();
                    if (index == 3)
                    {
                        BX.Text = $"{rejestry[index].Value}{BX.Text[2]}{BX.Text[3]}";
                    }
                    if (index == 2)
                    {
                        BX.Text = $"{BX.Text[0]}{BX.Text[1]}{rejestry[index].Value}";
                    }
                }
                catch (Exception)
                {

                }
            }
            else if (ready)
            {
                if (textBox.Name == "DISP_VALUE")
                {
                    BL.Text = "69";
                }

                if (textBox.Name == "DISP_ADDRESS")
                {
                    try
                    {
                        int index = int.Parse(textBox.Text, System.Globalization.NumberStyles.HexNumber);
                        //DISP_VALUE.Text = DISP[index].Value;
                        DISP_VALUE.Text = rejestry[index + baseLength].Value;
                    }
                    catch (Exception)
                    {
                   
                    }
                }



            }
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void MinimizeApp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.WindowState = WindowState.Minimized;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void CloseApp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }


        private void ChangeValueDisp(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            if (!ready) return;
            try
            {
                int value = int.Parse(textBox.Text, System.Globalization.NumberStyles.HexNumber);
                int index = int.Parse(DISP_ADDRESS.Text, System.Globalization.NumberStyles.HexNumber);
                //DISP[index].Value = value.ToString();
                rejestry[index + baseLength].Value = value.ToString();
            }
            catch (Exception)
            {
            }
        }

        private void ChangeVisibilityRight(object sender, MouseButtonEventArgs e)
        {
            if (RightStackPanel.Visibility == Visibility.Visible)
            {
                RightStackPanel.Visibility = Visibility.Hidden;
                RightStackPanelDISP.Visibility = Visibility.Visible;
                if(LeftStackPanelDISP.Visibility == Visibility.Visible)
                {
                    LeftStackPanelDISP.Visibility = Visibility.Hidden;
                    LeftStackPanel.Visibility = Visibility.Visible;
                    if (SelectLeft.SelectedValue != null)
                        left = rejestry.FindIndex(e => e.Name == SelectLeft.SelectedValue.ToString());
                }
                Right_SelectionDISP(sender);
            }
            else
            {
                RightStackPanelDISP.Visibility = Visibility.Hidden;
                RightStackPanel.Visibility = Visibility.Visible;
                if (SelectLeft.SelectedValue != null)
                    right = rejestry.FindIndex(e => e.Name == SelectRight.SelectedValue.ToString());
            }
        }
        private void ChangeVisibilityLeft(object sender, MouseButtonEventArgs e)
        {
            if (LeftStackPanel.Visibility == Visibility.Visible)
            {
                LeftStackPanel.Visibility = Visibility.Hidden;
                LeftStackPanelDISP.Visibility = Visibility.Visible;
                if (RightStackPanelDISP.Visibility == Visibility.Visible)
                {
                    RightStackPanelDISP.Visibility = Visibility.Hidden;
                    RightStackPanel.Visibility = Visibility.Visible;
                    if (SelectLeft.SelectedValue != null)
                        right = rejestry.FindIndex(e => e.Name == SelectRight.SelectedValue.ToString());
                }
                Left_SelectionDISP(sender);
            }
            else
            {
                LeftStackPanelDISP.Visibility = Visibility.Hidden;
                LeftStackPanel.Visibility = Visibility.Visible;
                if (SelectLeft.SelectedValue != null)
                    left = rejestry.FindIndex(e => e.Name == SelectLeft.SelectedValue.ToString());
            }
        }

        private void Left_SelectionDISPChanged(object sender, SelectionChangedEventArgs e)
        {
            Left_SelectionDISP(sender);
        }
        private void Left_SelectionDISP(object sender)
        {
            int a = 0, b = 0, c = 0;

            if (SelectLeftDISPB.SelectedValue != null && SelectLeftDISPB.SelectedValue.ToString() != "-")
            {
                if (SelectLeftDISPB.SelectedValue.ToString() == "BX")
                    a = int.Parse(BX.Text, System.Globalization.NumberStyles.HexNumber);
                if (SelectLeftDISPB.SelectedValue.ToString() == "BP")
                    a = int.Parse(BP.Text, System.Globalization.NumberStyles.HexNumber);
            }
            if (SelectLeftDISPI.SelectedValue != null && SelectLeftDISPI.SelectedValue.ToString() != "-")
            {
                if (SelectLeftDISPI.SelectedValue.ToString() == "SI")
                    b = int.Parse(SI.Text, System.Globalization.NumberStyles.HexNumber);
                if (SelectLeftDISPI.SelectedValue.ToString() == "DI")
                    b = int.Parse(DI.Text, System.Globalization.NumberStyles.HexNumber);
            }
            try
            {
                c = int.Parse(SelectLeftDISPV.Text, System.Globalization.NumberStyles.HexNumber);
            }
            catch (Exception)
            {

                c = 0;
            }

            left = a + b + c + baseLength;
        }
        private void Left_SelectionDISPTextChanged(object sender, TextChangedEventArgs e)
        {
            Left_SelectionDISP(sender);
        }
        private void Right_SelectionDISPChanged(object sender, SelectionChangedEventArgs e)
        {
            Right_SelectionDISP(sender);
        }
        private void Right_SelectionDISP(object sender)
        {
            int a = 0, b = 0, c = 0;

            if (SelectRightDISPB.SelectedValue != null && SelectRightDISPB.SelectedValue.ToString() != "-")
            {
                if (SelectRightDISPB.SelectedValue.ToString() == "BX")
                    a = int.Parse(BX.Text, System.Globalization.NumberStyles.HexNumber);
                if (SelectRightDISPB.SelectedValue.ToString() == "BP")
                    a = int.Parse(BP.Text, System.Globalization.NumberStyles.HexNumber);
            }
            if (SelectRightDISPI.SelectedValue != null && SelectRightDISPI.SelectedValue.ToString() != "-")
            {
                if (SelectRightDISPI.SelectedValue.ToString() == "SI")
                    b = int.Parse(SI.Text, System.Globalization.NumberStyles.HexNumber);
                if (SelectRightDISPI.SelectedValue.ToString() == "DI")
                    b = int.Parse(DI.Text, System.Globalization.NumberStyles.HexNumber);
            }
            try
            {
                c = int.Parse(SelectRightDISPV.Text, System.Globalization.NumberStyles.HexNumber);
            }
            catch (Exception)
            {

                c = 0;
            }

            right = a + b + c + baseLength;
        }
        private void Right_SelectionDISPTextChanged(object sender, TextChangedEventArgs e)
        {
            Right_SelectionDISP(sender);
        }
    }
}
