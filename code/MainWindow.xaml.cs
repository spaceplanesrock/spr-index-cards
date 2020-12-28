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

namespace spr_index_cards
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Lektion> heisig;

        List<IndexCard> currentCards;
        int currentCardIndex;
        int currentCardStatus; //0 = front, first; 1 = back, first; 2 = back, second; 3 = front, second
        Random r = new Random();
        TextBox tb;


        public MainWindow()
        {
            InitializeComponent();
            heisig = Lektion.LoadLektionenFromXML();
            LoadMenu();
        }

        public void LoadMenu()
        {
            ContentPanel.Children.Clear();

            foreach(Lektion l in heisig)
            {
                Button b = new Button();
                b.Content = "Lektion " + l.number.ToString();
                b.Tag = l.number;
                b.Click += B_Click;
                ContentPanel.Children.Add(b);
            }
        }

        public void LoadLektion(uint number)
        {
            //behindert aber wurst
            Lektion l = heisig[(int) number - 1];
            currentCards = l.cards;
            currentCards.Shuffle();
            currentCardIndex = -1;
            currentCardStatus = -1;
            ShowNextCard();
        }

        public void ShowNextCard()
        {
            if (currentCardStatus == -1)
            {
                //no card shown yet
                ContentPanel.Children.Clear();
                tb = new TextBox();
                tb.FontSize = 30;
                ContentPanel.Children.Add(tb);
            }
            switch (currentCardStatus)
            { 
                
                case -1:
                case 2:
                case 3:
                    currentCardIndex++;
                    currentCardStatus = r.Next(0, 2);
                    break;
                case 0:
                case 1:
                    currentCardStatus += 2;
                    break;
            }
            if (currentCards.Count() <= currentCardIndex) LoadMenu();
            else
            {
                tb.Text = (currentCardStatus == 0 || currentCardStatus == 3) ? currentCards[currentCardIndex].front : currentCards[currentCardIndex].back;
                this.Title =
                    "spr index cards | " +
                    ((currentCardStatus < 2) ? "question" : "answer") +
                    " phase | " + (currentCardIndex + 1).ToString() + " of " + currentCards.Count().ToString();
            }

        }

        private void B_Click(object sender, RoutedEventArgs e)
        {
            uint number = (uint)((Button) sender).Tag;
            LoadLektion(number);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) ShowNextCard();
        }
    }

    //https://forum.unity.com/threads/clever-way-to-shuffle-a-list-t-in-one-line-of-c-code.241052/
    public static class IListExtensions
    {
        private static Random rng = new Random();

        /// <summary>
        /// Shuffles the element order of the specified list.
        /// </summary>
        public static void Shuffle<T>(this IList<T> ts)
        {
            var count = ts.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = rng.Next(i, count);
                var tmp = ts[i];
                ts[i] = ts[r];
                ts[r] = tmp;
            }
        }
    }
}
