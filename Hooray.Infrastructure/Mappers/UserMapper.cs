using AutoMapper;
using Hooray.Core.ViewModels;
using Hooray.Infrastructure.DBContexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hooray.Infrastructure.Mappers
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<UserInfoViewModel, HryUserProfile>();
        }
    }
}
