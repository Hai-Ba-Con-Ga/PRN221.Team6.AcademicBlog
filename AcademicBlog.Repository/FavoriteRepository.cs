using AcademicBlog.BussinessObject;
using AcademicBlog.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicBlog.Repository
{
    public class FavoriteRepository:BaseRepository<Favourite>,IFavoriteRepository
    {
    }
}
