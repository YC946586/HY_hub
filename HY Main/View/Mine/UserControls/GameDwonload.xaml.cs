using HY.Application.Base;
using HY_Main.ViewDialog;
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

namespace HY_Main.View.Mine.UserControls
{
    /// <summary>
    /// GameDwonload.xaml 的交互逻辑
    /// </summary>
    public partial class GameDwonload : UserControl
    {
        public GameDwonload()
        {
            InitializeComponent();
        }
    }
    public class EGameDwonloadDlg : BaseViewDialog<GameDwonload>, IModelDialog
    {
    }
}
