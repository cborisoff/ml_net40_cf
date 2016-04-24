using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models.Repositories;

namespace Models
{
    public class UnitOfWork : Base.Model.BaseUnitOfWork
    {
        public UnitOfWork(Main.MainDBC context)
        {
            this.Context = context;
        }

        private MainRepository main = null;
        public MainRepository Main
        {
            get
            {
                if (main == null)
                {
                    main = new MainRepository(this);
                }
                return main;
            }
        }

    }
}
