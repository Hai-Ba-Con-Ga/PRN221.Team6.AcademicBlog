using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AcademicBlog.BussinessObject;
using AcademicBlog.DAO;
using AcademicBlog.Repository.Interface;

namespace AcademicBlog.Repository
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        private readonly GenericDAO<Category> _categoryDAO = new GenericDAO<Category>();

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _categoryDAO.GetAllAsync();
        }
        //get id include post
        public async Task<Category> GetById(int id)
        {
            return await _categoryDAO.GetByIdAsync(id,
            includeProperties: new Expression<Func<Category, object>>[] { x => x.Posts }
            );
        }
        // get id include post for front
        public async Task<Category> GetByIdFront(int id)
        {
            return await _categoryDAO.GetByIdAsync(id,
            filter: x => x.Posts.Any(y => y.IsPublic == true),
            includeProperties: new Expression<Func<Category, object>>[] { x => x.Posts }
            );
        }
        // add category
        public async Task<Category> Add(Category category)
        {
            return await _categoryDAO.AddAsync(category);
        }
        // update category
        public async Task<Category> Update(Category category)
        {
            return await _categoryDAO.UpdateAsync(category);
        }
        // delete category
        public void Delete(Category category)
        {
            _categoryDAO.DeleteAsync(category);
        }

    }
}
