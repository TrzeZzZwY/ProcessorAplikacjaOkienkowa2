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
        }

        private void Losuj(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            foreach (var item in rejestry)
            {
                item.Value = random.Next(256).ToString();              
            }
            Aktualizuj();
        }
        private void Aktualizuj()
        {
            foreach (var item in rejestry)
            {
                TextBox textBox = (TextBox)FindName(item.Name);
                textBox.Text = item.Value;
            }
        }
        private void Operation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            operation = SelectOperation.SelectedValue.ToString();
            if (operation == "INC" || operation == "DEC" || operation == "NOT" || operation == "NEG")
            {
                RightStackPanel.Visibility = Visibility.Hidden;
                right = -1;
            }
            else
                RightStackPanel.Visibility = Visibility.Visible;

        }
        private void Left_SelectionChanged(object sender, SelectionChangedEventArgs e) => left = rejestry.FindIndex(e => e.Name == SelectLeft.SelectedValue.ToString());


        private void Right_SelectionChanged(object sender, SelectionChangedEventArgs e) => right = rejestry.FindIndex(e => e.Name == SelectRight.SelectedValue.ToString());


        private void Wykonaj(object sender, RoutedEventArgs e)
        {
            DEBUG.Text = left.ToString();
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
    }
}
