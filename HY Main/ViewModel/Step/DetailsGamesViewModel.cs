using GalaSoft.MvvmLight.Messaging;
using HY.Client.Entity.HomeEntitys;
using HY_Main.Common.CoreLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY_Main.ViewModel.Step
{
    public class DetailsGamesViewModel: BaseHostDialogOperation
    {
        //public event Action ClostEvent;

        private Recommendgame _showContent;

        public Recommendgame ShowContent
        {
            get => _showContent;
            set
            {
                _showContent = value;
                RaisePropertyChanged();
            }
        }
        public void InitViewModel(Recommendgame ShowCo)
        {
            ShowContent = ShowCo;
            Messenger.Default.Send<object>(ShowContent, "ShowVideo");
        }
    }
}
