using OptimizedTreeView.Interface;
using OptimizedTreeView.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimizedTreeView.Common;

namespace OptimizedTreeView.Managers
{
    public class TreeViewManager : ITreeViewInterface
    {

        private static TreeViewModel _currentTree;

        public static TreeViewModel CurrentTree
        {
            get => _currentTree;
            set
            {
                _currentTree = value;
                OnCurrentProjectChanged();
            }
        }

        private static ObservableCollection<TreeViewModel> selectTreeModel;

        public static ObservableCollection<TreeViewModel> SelectTreeModel
        {
            get => selectTreeModel;
            set
            {
                selectTreeModel = value;
                OnCurrentProjectChanged();
            }
        }

        public static event EventHandler CurrentProjectChanged;
        public static void OnCurrentProjectChanged()
        {
            CurrentProjectChanged?.Invoke(CurrentTree, new EventArgs());
        }

        private TreeViewModel searchModel;


        public static void CreateTree()
        {
            CurrentTree = TreeViewModel.CreateModel();
        }

        public void Add(object obj)
        {
            if (obj == null)
            {
                AddRoot();
                return;
            }

            GetTreeModel(obj);

            AddChild();
        }

        public void Delete(object obj)
        {
            GetTreeModel(obj);
            if (searchModel.Parent == null)
            {
                CurrentTree.Children.Remove(searchModel);
            }
            else
            {
                searchModel.Parent.Children.Remove(searchModel);
            }

        }

        public void Update(string str)
        {
            searchModel.Name = str;
            searchModel.IsTextBoxVisibility = System.Windows.Visibility.Hidden;
        }

        public void Search(string str)
        {
            CurrentTree.Children.ToList().ForEach(x =>
            {
                x.BFSTraverseTree((item) =>
                {
                    // 中文包含
                    if (item.Name.Contains(str))
                    {
                        ShowSearchResult(item);
                    }
                    else
                    {
                        item.IsNodeVisibility = System.Windows.Visibility.Collapsed;
                    }
                });
            });
        }

        public void Select(object obj)
        {
            GetTreeModel(obj);
            searchModel.IsTextBoxVisibility = System.Windows.Visibility.Visible;
        }

        private void AddRoot()
        {
            CurrentTree.Children.Add(new TreeViewModel()
            {
                Id = Guid.NewGuid(),
                Name = $"{CurrentTree.Children.Count}"
            });
        }

        private void AddChild()
        {
            searchModel.Children.Add(new TreeViewModel()
            {
                Id = Guid.NewGuid(),
                Name = $"{searchModel.Name}_{searchModel.Children.Count}",
                Parent = searchModel
            });
        }

        private TreeViewModel TraversalTree(TreeViewModel model, Guid guid)
        {
            foreach (var item in model.Children)
            {
                if (item.Id == guid)
                {
                    searchModel = item;
                    return searchModel;
                }
                TraversalTree(item, guid);
            }
            return null;
        }

        private void GetTreeModel(object obj)
        {
            var baseResult = obj as BaseModel;

            TraversalTree(CurrentTree, baseResult.Id);
        }

        private void ShowSearchResult(TreeViewModel model)
        {
            if (model.Parent != null)
            {
                model.Parent.IsNodeVisibility = System.Windows.Visibility.Visible;
                ShowSearchResult(model.Parent);
            }
            else
            {
                model.IsNodeVisibility = System.Windows.Visibility.Visible;
            }
        }


    }

}
