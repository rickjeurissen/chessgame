using chessgame.wpf.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private const int RowCount = 8;
        private const int ColCount = 8;

        public BoardView()
        {
            InitializeComponent();

            CreateBoard();
            //DataContext = this; // remove?
        }

        private void CreateBoard()
        {
            for (var row = 0; row < RowCount; row++)
            {
                var isBlack = row % 2 == 1;
                for (int col = 0; col < ColCount; col++)
                {
                    var square = new Rectangle
                    {
                        Fill = isBlack ? Brushes.Black : Brushes.White
                    };
                    SquaresGrid.Children.Add(square);
                    isBlack = !isBlack;
                }
            }
        }
    }
}
