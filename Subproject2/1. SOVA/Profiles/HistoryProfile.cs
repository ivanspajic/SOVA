﻿using _0._Models;
using _1._SOVA.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1._SOVA.Profiles
{
    public class UserHistoryProfile : Profile
    {
        public UserHistoryProfile()
        {
            CreateMap<UserHistory, UserHistoryDto>();
        }
    }
}
