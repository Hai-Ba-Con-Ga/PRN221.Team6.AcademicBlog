using AcademicBlog.BussinessObject;
using AcademicBlog.BussinessObject.PagingObject;
using AcademicBlog.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicBlog.Repository
{
    public class FollowingRepository : BaseRepository<Following>, IFollowingRepository
    {
        public async Task<IEnumerable<Following>> GetFollowers(int Id, Pagable? pagable = null)
        {
            if (pagable is null)
            {
                pagable = new Pagable()
                {
                    PageIndex = 1,
                    PageSize = 10,
                };
            }
            pagable.Filter = new Filter()
            {
                Logic = "and",
                Filters = new List<Filter>()
                {
                    new Filter()
                    {
                        Field = "FollowingId",
                        Operator = "eq",
                        Value = Id
                    }
                }
               
            };

            return await _genericDAO.GetListAsync(pagable, f => f.Include(f => f.Follower));
        }

        public async Task<IEnumerable<Following>> GetFollowing(int Id, Pagable? pagable = null)
        {
            if (pagable is null)
            {
                pagable = new Pagable()
                {
                    PageIndex = 1,
                    PageSize  = 10,
                };
            }
            pagable.Filter = new Filter()
            {
                Logic = "and",
                Filters = new List<Filter>()
                {
                    new Filter()
                    {
                        Field = "FollowerId",
                        Operator = "eq",
                        Value = Id
                    }
                }
            };

            return await _genericDAO.GetListAsync(pagable, f => f.Include(f => f.FollowingNavigation));
        }
    }
}
