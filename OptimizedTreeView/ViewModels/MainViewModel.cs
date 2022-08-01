using OptimizedTreeView.Common;
using OptimizedTreeView.Interface;
using OptimizedTreeView.Managers;
using OptimizedTreeView.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizedTreeView.ViewModels
{
    public class MainViewModel
    {
        public TreeViewModel TreeModel => TreeViewManager.CurrentTree;

        private readonly ITreeViewInterface @interface;

        #region 命令
        public DelegateCommand<BaseModel> AddCommand { get; set; }
        public DelegateCommand<BaseModel> DeleteCommand { get; set; }
        public DelegateCommand<string> UpdateCommand { get; set; }
        public DelegateCommand<string> SearchCommand { get; set; }
        public DelegateCommand<BaseModel> SelectCommand { get; set; }
        #endregion

        public MainViewModel(ITreeViewInterface @interface)
        {
            this.@interface = @interface;
            AddCommand = new DelegateCommand<BaseModel>(Add);
            DeleteCommand = new DelegateCommand<BaseModel>(Delete);
            UpdateCommand = new DelegateCommand<string>(Update);
            SearchCommand = new DelegateCommand<string>(Search);
            SelectCommand = new DelegateCommand<BaseModel>(Select);
            TreeViewManager.CreateTree();
        }

        private void Add(BaseModel obj)
        {
            @interface.Add(obj);
        }

        private void Delete(BaseModel obj)
        {
            @interface.Delete(obj);
        }

        private void Update(string str)
        {
            @interface.Update(str);
        }

        private void Search(string str)
        {
            @interface.Search(str);
        }

        private void Select(BaseModel obj)
        {
            @interface.Select(obj);
        }
    }
}
