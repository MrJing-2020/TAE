using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace TAE.WebServer.Controllers.Admin
{
    using TAE.Data.Model;
    using TAE.WebServer.Common;

    /// <summary>
    /// 组织结构管理
    /// </summary>
    public class OrganizationController : BaseApiController
    {
        /// <summary>
        /// 获取组织结构树(手动递归生成数据)
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //public HttpResponseMessage GetAllOrgz()
        //{
        //    var comList = ServiceBase.FindBy<Company>().ToList();
        //    var treeList = new List<TreeModelView>();
        //    foreach (var item in comList.Where(m=>string.IsNullOrEmpty(m.PreCompanyId)))
        //    {
        //        TreeModelView treeItem = new TreeModelView { id = item.Id, text = item.CompanyName, type = "root" };
        //        treeList.Add(treeItem);
        //    }
        //    InitTreeList(comList, treeList);
        //    return Response(treeList);
        //}

        /// <summary>
        /// 获取组织结构树(让前端jstree自动遍历)，两种方法速度未知
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAllOrgz()
        {
            var comList = ServiceBase.FindBy<Company>().ToList();
            List<JsTreeModel> treeList = new List<JsTreeModel>();
            foreach (var item in comList)
            {
                var treeItem = new JsTreeModel
                {
                    id = item.Id,
                    text = item.CompanyName,
                    parent = item.PreCompanyId,
                    icon = "fa fa-folder"
                };
                treeList.Add(treeItem);
            }
            return Response(treeList);
        }

        /// <summary>
        /// 获取公司下拉框数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage ComSelectList()
        {
            string sql = "select Id as 'Key',CompanyName as 'Value' from Company";
            List<KeyValueModel> list = ServiceBase.FindBy<KeyValueModel>(sql).ToList();
            return Response(list);
        }

        /// <summary>
        /// 根据id获取公司具体信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetComDetail(string id)
        {
            var company = ServiceBase.FindBy<Company>(m => m.Id == id).FirstOrDefault();
            return Response(company);
        }

        /// <summary>
        /// 提交新增和编辑公司数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SubComData(Company model)
        {
            ServiceBase.SaveEntity<Company>(model);
            return Response();
        }

        /// <summary>
        /// 根据公司id获取公司用户
        /// </summary>
        /// <param name="id">部门id</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetComUsers(string id)
        {
            var users = ServiceIdentity.FindUser(m => m.CompanyId == id).ToList();
            return ResponseList<AppUser>(users);
        }

        /// <summary>
        /// 根据id获取公司部门
        /// </summary>
        /// <param name="id">公司id</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetDeps(string id)
        {
            var dep = ServiceBase.FindBy<Department>(m => m.CompanyId == id).ToList();
            return ResponseList<Department>(dep);
        }

        /// <summary>
        /// 根据id获取部门用户
        /// </summary>
        /// <param name="id">部门id</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetDepUsers(string id)
        {
            var users = ServiceIdentity.FindUser(m=>m.DepartmentId==id).ToList();
            return ResponseList<AppUser>(users);
        }

        /// <summary>
        /// 提交新增和编辑部门数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SubDepData(Department model)
        {
            ServiceBase.SaveEntity<Department>(model);
            return Response();
        }

        /// <summary>
        /// 根据id获取公司职位
        /// </summary>
        /// <param name="id">公司id</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetPositions(string id)
        {
            var pos = ServiceBase.FindBy<Position>(m => m.CompanyId == id).ToList();
            return ResponseList<Position>(pos);
        }

        /// <summary>
        /// 提交新增和编辑部门数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SubPosData(Position model)
        {
            ServiceBase.SaveEntity<Position>(model);
            return Response();
        }

        #region 私有方法

        /// <summary>
        /// 递归的方式遍历Company结构，把数据填充到TreeModelView中
        /// </summary>
        /// <param name="comList"></param>
        /// <param name="treeList"></param>
        /// <returns></returns>
        [NonAction]
        private List<TreeModelView> InitTreeList(List<Company> comList, List<TreeModelView> treeList)
        {
            foreach (var item in treeList)
            {
                var comChildList = comList.Where(m => m.PreCompanyId == item.id);
                if (comChildList.Count() <= 0)
                {
                    continue;
                }
                foreach (var item1 in comChildList)
                {
                    TreeModelView treeItem = new TreeModelView { id = item1.Id, text = item1.CompanyName, type = "file" };
                    item.children = new List<TreeModelView>();
                    item.children.Add(treeItem);
                }
                InitTreeList(comList, item.children);
            }
            return treeList;
        } 

        #endregion
    }
}
