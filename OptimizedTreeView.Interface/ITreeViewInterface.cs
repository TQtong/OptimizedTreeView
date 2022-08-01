using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizedTreeView.Interface
{
    public interface ITreeViewInterface
    {
        void Add(object obj);

        void Delete(object obj);

        void Update(string str);

        void Search(string str);

        void Select(object obj);
    }
}
