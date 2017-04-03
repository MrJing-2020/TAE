using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAE.WebServer.Common
{
    using TAE.Data.Model;

    /// <summary>
    /// AutoMapper配置类
    /// </summary>
    public class ConfigurationAutoMapper
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<AppUser, UserViewModel>();
                cfg.CreateMap<UserViewModel, AppUser>();
            });

        }
    }
}