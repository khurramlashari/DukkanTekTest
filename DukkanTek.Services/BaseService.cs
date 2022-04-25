using Domain.Interfaces;
using System;

namespace DukkanTek.Services
{
    public class BaseService  
    {
        
        protected readonly IUnitOfWork UnitOfWork;

        public BaseService(IUnitOfWork  unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
    }
}
