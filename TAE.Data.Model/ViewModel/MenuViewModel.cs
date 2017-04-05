using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    public class MenuViewModel
    {
        public  string Id { get; set; }
        public string MenuName { get; set; }
        public string App { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string MenuHtmlUrl { get; set; }
        public string MenuApiUrl { get; set; }
        //1对应到Area，2对应到Controller，3对应到Action
        public int MenuLever { get; set; }
        public int Sort { get; set; }
        public string MenuPareId { get; set; }
        public string MenuIco { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastModifiedTime { get; set; }
        //父菜单名
        public string PareMenuName { get; set; }
        //是否为父菜单(此处针对一级菜单是否有二级子菜单，仅有三级子菜单的仍然为false)
        public bool IsParent { get; set; }
        public bool? IsInAuthority { get; set; }
    }
}
