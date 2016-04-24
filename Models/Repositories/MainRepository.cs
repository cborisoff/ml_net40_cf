using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models.Main;
namespace Models.Repositories
{
    public class MainRepository : Base.Model.BaseRepository
    {

        public MainRepository(UnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentException("An instance of AccountsEntityContext is required to use this repository.", "context");
            }

            this.data = unitOfWork;
            this.Context = unitOfWork.Context;
        }

        public IEnumerable<News> Select_News()
        {
            return Query<News>();
        }
    }
}
