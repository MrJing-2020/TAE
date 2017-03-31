﻿using System;
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
        [Key]
        public new int Id { get; set; }
        public string MenuName { get; set; }
        public string App { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string MenuUrl { get; set; }
        //1对应到Area，2对应到Controller，3对应到Action
        public int MenuLever { get; set; }
        public int Sort { get; set; }
        public int MenuPareId { get; set; }
        public string MenuIco { get; set; }
    }
}
