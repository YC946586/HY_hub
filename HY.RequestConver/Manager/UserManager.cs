using HY.Client.Entity;
using HY.RequestConver.InterFace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.RequestConver.Manager
{
    /// <summary>
    /// 用户管理器 
    /// </summary>
    public class UserManager : IUser
    {
        /// <summary>
        /// 根据账户获取账户信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public Task<ServiceResponse> GetModelByAccount(Dictionary<string, string> account)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public Task<ServiceResponse> Login(string macAdd,string password,string phone)
        {
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>
                {
                    { "macAdd ", macAdd},
                    { "passWord  ", password},
                    { "phone", phone},
                };
                var genrator = Task.Run(() => Network.ApiPost("user", "login", dic));
                return genrator;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public Task<ServiceResponse> Logout(Dictionary<string, string> account)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public Task<ServiceResponse> Register(string passWord,string phone,string validCode)
        {
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>
                {
                    { "passWord ", passWord},
                    { "phone ", phone},
                    { "validCode ", validCode},
                };
                var genrator = Task.Run(() => Network.ApiPost("user", "register", dic));
                return genrator;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="passWord"></param>
        /// <param name="phone"></param>
        /// <param name="validCode"></param>
        /// <returns></returns>
        public Task<ServiceResponse> ResetPwdByCode(string passWord, string phone, string validCode)
        {
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>
                {
                    { "passWord", passWord },
                    { "phone", phone },
                    { "validCode", validCode  }
                };
                var genrator = Task.Run(() => Network.ApiPost("user", "resetPwdByCode", dic));
                return genrator;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public Task<ServiceResponse> SendSmsCode(string phone,string type)
        {
            try
            {
                //(query) 短信类型：1 注册，2 重置密码
                Dictionary<string, object> dic = new Dictionary<string, object>
                {
                    { "phone", phone},
                    { "type", type}
                };
                var genrator = Task.Run(() => Network.ApiPost("user", "sendSmsCode", dic));
                return genrator;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
