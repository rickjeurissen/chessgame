using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chessgame.wpf.ViewModel
{
    public class RootViewModel : Conductor<IScreen>.StackNavigation
    {
        private readonly Func<BoardViewModel> boardViewFactory;
        public RootViewModel(Func<BoardViewModel> boardViewFactory)
        {
            this.boardViewFactory = boardViewFactory;
            InitialiseBoardView();
        }

        private void InitialiseBoardView()
        {
            BoardViewModel boardVM = boardViewFactory();
            //boardVM
            this.ActivateItem(boardVM);
        }
    }

    
}
