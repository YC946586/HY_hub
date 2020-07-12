using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.Application.BaseModel
{
    public class GameModel
    {
        private string _gameName;
        private string _gameIcon;
        private string _gameType;
        private string _gameDescribe;
        private string _gameId;
        private object body;

        public int Sort { get; set; }
        /// <summary>
        ///  游戏名称
        /// </summary>
        public string GameName
        {
            get { return _gameName; }
            set { _gameName = value; }
        }
        /// <summary>
        /// 游戏名称
        /// </summary>
        public string GameIcon
        {
            get { return _gameIcon; }
            set { _gameIcon = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string GameType
        {
            get { return _gameType; }
            set { _gameType = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string GameDescribe
        {
            get { return _gameDescribe; }
            set { _gameDescribe = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string GameId
        {
            get { return _gameId; }
            set { _gameId = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public object Body
        {
            get { return body; }
            set { body = value; }
        }
    }
}
