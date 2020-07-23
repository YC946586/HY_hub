using HY.Client.Execute.Commons;
using HY.RequestConver.Bridge;
using HY.RequestConver.InterFace;
using HY_Main.Common.CoreLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY_Main.ViewModel.Step
{
    public class CouponViewModel : BaseDialogOperation
    {
        private string _code = string.Empty;

        public string code
        {
            get { return _code; }
            set { _code = value; RaisePropertyChanged(); }
        }

        public event Action ClostEvent;

        public override async void Save()
        {
            try
            {
                ICommon common = BridgeFactory.BridgeManager.GetCommonManager();
                var gamesGetGames = await common.UseCoupon(code);
                Msg.Info(gamesGetGames.Message);
                if (gamesGetGames.code.Equals("000"))
                {
                    ClostEvent?.Invoke();
                }
            }
            catch (Exception ex)
            {
                Msg.Error(ex);
            }
        }
    }
}
