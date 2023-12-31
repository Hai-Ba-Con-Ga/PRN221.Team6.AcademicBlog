﻿using AcademicBlog.BussinessObject.PagingObject;
using AcademicBlog.BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicBlog.Repository.Interface
{
    public interface IFollowingRepository:IBaseRepository<Following>
    { 
        Task<IEnumerable<Following>> GetFollowers(int Id, Pagable? pagable);
        Task<IEnumerable<Following>> GetFollowing(int Id, Pagable? pagable);

    }
}
