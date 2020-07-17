using HY.Client.Entity.UserEntitys;
using HY.Client.Execute.Commons;
using HY.RequestConver.Bridge;
using HY.RequestConver.InterFace;
using HY_Main.Common.CoreLib;
using HY_Main.Model.Mine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HY_Main.ViewModel.Mine
{
    public class MineViewModel : BaseOperation<UserGamesEntity>
    {
        private string _showUser;
        /// <summary>
        ///  
        /// </summary>
        public string ShowUser
        {
            get { return _showUser; }
            set { _showUser = value; RaisePropertyChanged(); }
        }

        public override void InitViewModel()
        {
            base.InitViewModel();
            InitHotRecomenAsync();
        }
        public async void InitHotRecomenAsync()
        {
            try
            {
                //viptype类型 0：普通用户  1：月费用户  2：年费用户
                string vipType = string.Empty;
                switch (Loginer.LoginerUser.vipType)
                {
                    case "0":
                        {
                            vipType = "普通用户";
                            break;
                        }
                    case "1":
                        {
                            vipType = "月费用户";
                            break;
                        }
                    case "2":
                        {
                            vipType = "年费用户";
                            break;
                        }
                }
                ShowUser = Loginer.LoginerUser.UserName + "余额:" + Loginer.LoginerUser.balance + "鹰币   " + vipType + ":    " + "剩余下载次数" + Loginer.LoginerUser.freeCount + "次,会员有效期至" + Loginer.LoginerUser.vipValidTo;

                IUser user = BridgeFactory.BridgeManager.GetUserManager();
                var genrator = await user.GetUserGames("");
                if (genrator.code.Equals("000"))
                {
                    var Results = JsonConvert.DeserializeObject<List<UserGamesEntity>>(genrator.result.ToString());
                    Results.OrderBy(s => s.id).ToList().ForEach((ary) => GridModelList.Add(ary));
                }
            }
            catch (Exception ex)
            {
                Message.ErrorException(ex);
            }
        }
    }
}
