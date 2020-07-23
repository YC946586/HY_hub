﻿using GalaSoft.MvvmLight.Messaging;
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
    public class EditPwdViewModel: BaseDialogOperation
    {
        private string _newPwd = string.Empty;
        private string _oldPwd = string.Empty;

        public string newPwd
        {
            get { return _newPwd; }
            set { _newPwd = value; RaisePropertyChanged(); }
        }

        public string oldPwd
        {
            get { return _oldPwd; }
            set { _oldPwd = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 确定
        /// </summary>
        public override async void Save()
        {
            try
            {
                IUser user = BridgeFactory.BridgeManager.GetUserManager();
                var gamesGetGames = await user.ResetPwd(newPwd, oldPwd);
                Message.Info(gamesGetGames.Message);
                if (gamesGetGames.code.Equals("000"))
                {
                    Messenger.Default.Send("", "EditPwdClose");
                }
            }
            catch (Exception ex)
            {
                Message.ErrorException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override void Cancel()
        {
            Messenger.Default.Send("", "EditPwdClose");
        }
    }
}
