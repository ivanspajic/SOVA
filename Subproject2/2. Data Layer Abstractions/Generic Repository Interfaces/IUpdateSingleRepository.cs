using System;
using System.Collections.Generic;
using System.Text;

namespace _2._Data_Layer_Abstractions.Generic_Repository_Interfaces
{
    public interface IUpdateSingleRepository<T>
    {
        T Update(T t);
    }
}
