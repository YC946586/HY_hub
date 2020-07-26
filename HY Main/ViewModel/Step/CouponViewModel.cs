using HY.Client.Entity.CommonEntitys;
using HY.Client.Execute.Commons;
using HY.RequestConver.Bridge;
using HY.RequestConver.InterFace;
using HY_Main.Common.CoreLib;
using Newtonsoft.Json;
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
                if (gamesGetGames.code.Equals("000"))
                {
                    var Results = JsonConvert.DeserializeObject<CouponEntity>(gamesGetGames.result.ToString());
                    Loginer.LoginerUser.balance = Results.balance;
                    CommonsCall.UserBalance = Loginer.LoginerUser.balance;
                    CommonsCall.ShowUser = Loginer.LoginerUser.UserName + "  余额：" + Loginer.LoginerUser.balance + "鹰币   " + Loginer.LoginerUser.vipInfo;
                }
                Msg.Info(gamesGetGames.Message);
                ClostEvent?.Invoke();
            }
            catch (Exception ex)
            {
                Msg.Error(ex);
            }
        }
    }
}
