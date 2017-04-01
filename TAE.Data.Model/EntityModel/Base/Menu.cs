using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 菜单实体
    /// </summary>
    public class Menu : BaseModel
    {
        public string MenuName { get; set; }
        public string App { get; set; }
        //例：Admin
        public string Area { get; set; }
        //例：UserManager
        public string Controller { get; set; }
        //例：AllUsers
        public string Action { get; set; }
        //页面地址
        public string MenuHtmlUrl { get; set; }
        //api地址
        public string MenuApiUrl { get; set; }
        //1对应到Area，2对应到Controller，3对应到Action
        public int MenuLever { get; set; }
        public int Sort { get; set; }
        public string MenuPareId { get; set; }
        public string MenuIco { get; set; }
    }
}
