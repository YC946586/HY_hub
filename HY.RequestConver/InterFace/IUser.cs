using HY.Client.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.RequestConver.InterFace
{
    /// <summary>
    /// 用户数据操作接口
    /// </summary>
    public interface IUser 
    {
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="account">账号</param>
        /// <returns>结果</returns>
        Task<ServiceResponse> Register(string passWord, string phone, string validCode);
        /// <summary>
        /// 根据账户获取账户信息
        /// </summary>
        /// <param name="account">账号</param>
        /// <returns>结果</returns>
        Task<ServiceResponse> GetModelByAccount(Dictionary<string, string> account);
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="account">账号</param>
        /// <returns>结果</returns>
        Task<ServiceResponse> SendSmsCode(string phone, string type);
        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="account">账号</param>
        Task<ServiceResponse> Logout(Dictionary<string, string> account);

        /// <summary>
        /// 登录
        /// </summary>          
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <returns>用户信息</returns>
        Task<ServiceResponse> Login(string macAdd, string password, string phone);


        /// <summary>
        /// 登录
        /// </summary>          
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <returns>用户信息</returns>
        Task<ServiceResponse> ResetPwdByCode(string passWord, string phone, string validCode);
        
    }
}
