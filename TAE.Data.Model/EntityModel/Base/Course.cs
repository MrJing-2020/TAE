using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Data.Model
{
    /// <summary>
    /// 课程
    /// </summary>
    public class Course : ComBaseModel
    {
        //课程名
        public string Name { get; set; }
        //课程简介
        public string Introduction { get; set; }
        //是否公开
        public bool IsPublic { get; set; }
        //课程类型（必修，选修，公共）
        public string TypeId { get; set; }
        //课程分类
        public string CategoryId { get; set; }
        //能学到的
        public string CanStudy { get; set; }
        //须知
        public string NeedKown { get; set; }
        public string CreateUserId { get; set; }
    }
}
