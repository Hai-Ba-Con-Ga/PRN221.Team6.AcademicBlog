using AcademicBlog.BussinessObject;
using AcademicBlog.DAO;
using AcademicBlog.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicBlog.Repository
{
    public class PostTagRepository:IPostTagRepository
    {
        private readonly GenericDAO<PostTag> _tagDAO = new GenericDAO<PostTag>();

        public async Task<IEnumerable<PostTag>> GetAll()
        {
            return await _tagDAO.GetAllAsync();
        }
        //get id include post
        public async Task<PostTag> Add(PostTag postTag)
        {
            return await _tagDAO.AddAsync(postTag);
        }

        public async Task<PostTag> Update(PostTag postTag)
        {
            return await _tagDAO.UpdateAsync(postTag);
        }

        public async Task<PostTag> GetById(int id)
        {
            return await _tagDAO.GetByIdAsync(id);
        }

        public async void Delete(PostTag postTag)
        {
            await _tagDAO.DeleteAsync(postTag);

        }

        public Task<PostTag> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
