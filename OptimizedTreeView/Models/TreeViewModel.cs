using OptimizedTreeView.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using OptimizedTreeView.Common;

namespace OptimizedTreeView.Models
{
    public class TreeViewModel : BaseModel
    {
        private  Guid id;

        public override Guid Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }

        private string name;

        public override string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        private Visibility isTextBoxVisibility = Visibility.Hidden;

        public Visibility IsTextBoxVisibility
        {
            get => isTextBoxVisibility;
            set
            {
                isTextBoxVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility isNodeVisibility = Visibility.Visible;

        public Visibility IsNodeVisibility
        {
            get => isNodeVisibility;
            set
            {
                isNodeVisibility = value;
                OnPropertyChanged();
            }
        }

        public TreeViewModel Parent { get; set; }

        public ObservableCollection<TreeViewModel> Children { get; set; } = new ObservableCollection<TreeViewModel>();

        public static TreeViewModel CreateModel()
        {
            TreeViewModel tree = new TreeViewModel();
            for (int i = 0; i < 3; i++)
            {
                TreeViewModel treeViewModel = new TreeViewModel()
                {
                    Id = Guid.NewGuid(),
                    Name = $"{i}"
                };
                for (int j = 0; j < 5; j++)
                {
                    treeViewModel.Children.Add(new TreeViewModel()
                    {
                        Id= Guid.NewGuid(),
                        Name = $"{i}_{j}",
                        Parent = treeViewModel
                    });
                }
                tree.Children.Add(treeViewModel);
            }
            return tree;
        }
    }
}
