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

namespace chessgame.wpf.View
{
    /// <summary>
    /// Interaction logic for BoardView.xaml
    /// </summary>
    public partial class BoardView : UserControl
    {
        public BoardView()
        {
            InitializeComponent();

            InitializeBoard();
        }

        private void InitializeBoard()
        {
            bool white = true;

            for (int y = 0; y < 8; y++)
            {
                if (y % 2 == 0)
                {
                    white = true;
                }
                else
                {
                    white = false;
                }
                for (int x = 0; x < 8; x++)
                {
                    Rectangle rect = new Rectangle();

                    if (white)
                    {
                        rect.Fill = Brushes.WhiteSmoke;
                        white = false;
                    }
                    else
                    {
                        rect.Fill = Brushes.SandyBrown;
                        white = true;
                    }

                    Grid.SetRow(rect, y + 1);
                    Grid.SetColumn(rect, x + 1);

                    grid.Children.Add(rect);
                }
            }
        }
    }
}
