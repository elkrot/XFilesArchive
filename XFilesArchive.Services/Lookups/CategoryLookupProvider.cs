using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XFilesArchive.Model;
using XFilesArchive.Services.Repositories;

namespace XFilesArchive.Services.Lookups
{
    public class CategoryLookupProvider : ITreeViewLookupProvider<Category>
    {
        ICategoryRepository _categoryRepository;
        public CategoryLookupProvider(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IEnumerable<LookupItemNode> GetLookup()
        {
            var categories = _categoryRepository.GetAllCategories();

            return GetLookupCategories(categories);

        }

        public ObservableCollection<LookupItemNode> GetLookupCategories(List<Category> categories,int? categoryId = null)
        {
            var result = new ObservableCollection<LookupItemNode>();
            foreach (var item in categories.Where(x=>x.ParentCategoryKey.Equals(categoryId)))
            {
               
                result.Add(new LookupItemNode()
                {
                    DisplayMember = item.CategoryTitle,
                    Id = item.CategoryKey
                    ,
                    Nodes = GetLookupCategories(categories, item.CategoryKey)
                });
            }

            return result;
        }

        public IEnumerable<LookupItemNode> GetLookup(int? DriveId = default(int?))
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LookupItemNode> GetLookupWithCondition(Expression<Func<Category, bool>> where, Expression<Func<Category, object>> orderby)
        {
            throw new NotImplementedException();
        }
    }
}
