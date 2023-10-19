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
    public class TagRepository: ITagRepository
    {
        private readonly GenericDAO<Tag> _tagDAO = new GenericDAO<Tag>();

        public async Task<IEnumerable<Tag>> GetAll()
        {
            return await _tagDAO.GetAllAsync();
        }
        //get id include post
        public async Task<Tag> Add(Tag tag)
        {
            return await _tagDAO.AddAsync(tag);
        }
        public async Task<Tag> FindByName(string name)
        {
            return await _tagDAO.GetOneByConditionAsync( t =>  t.Name == name );
        }
        public async Task<Tag> Update(Tag tag)
        {
            return await _tagDAO.UpdateAsync(tag);
        }

        public async Task<Tag> GetById(int id)
        {
            return await _tagDAO.GetByIdAsync(id);
        }

        public void Delete(Tag tag)
        {
            _tagDAO.DeleteAsync(tag);

        }
    }
}