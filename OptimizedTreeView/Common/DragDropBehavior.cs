using Microsoft.Xaml.Behaviors;
using OptimizedTreeView.Models;
using OptimizedTreeView.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace OptimizedTreeView.Common
{
    public class DragDropBehavior : Behavior<UIElement>
    {
        Point startPoint;
        bool isDragging = false;
        TreeViewItem selectItem;
        AdornerLayer adornerLayer;

        protected override void OnAttached()
        {
            AssociatedObject.PreviewMouseLeftButtonDown += PreviewMouseLeftButtonDown;
            AssociatedObject.MouseMove += MouseMove;
            AssociatedObject.Drop += TreeView_Drop;
            AssociatedObject.PreviewQueryContinueDrag += PreviewQueryContinueDrag;
        }

        private void PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var item = (e.OriginalSource as FrameworkElement)?.DataContext as TreeViewModel;
                if (item != null)
                {
                    startPoint = e.GetPosition(AssociatedObject);
                }
            }
        }

        private void MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                return;
            }
            var point = e.GetPosition(null);

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (sender is TreeView dragSource)
                {
                    if (Math.Abs(point.X - startPoint.X) > System.Windows.SystemParameters.MinimumHorizontalDragDistance || Math.Abs(point.Y - startPoint.Y) > System.Windows.SystemParameters.MinimumVerticalDragDistance)
                    {
                        selectItem = GetDataFromTreeViewItem(dragSource, e.GetPosition(AssociatedObject));

                        //添加装饰器
                        DragDropAdorner dragDropAdorner = new DragDropAdorner((e.OriginalSource as FrameworkElement).Parent as Grid);
                        adornerLayer = AdornerLayer.GetAdornerLayer(dragSource);
                        adornerLayer.Add(dragDropAdorner);

                        if (selectItem != null)
                        {
                            isDragging = true;
                            DragDrop.DoDragDrop(dragSource, selectItem.DataContext as TreeViewModel, DragDropEffects.Move);
                        }
                        adornerLayer.Remove(dragDropAdorner);
                        adornerLayer = null;
                    }
                }
            }

        }

        private void TreeView_Drop(object sender, DragEventArgs e)
        {
            if (sender is TreeView)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
                // Verify that this is a valid drop and then store the drop target
                TreeViewModel targetItem = (e.OriginalSource as TextBlock)?.DataContext as TreeViewModel;
                MainViewModel vm = (sender as TreeView).DataContext as MainViewModel;
                if (targetItem != null && vm != null)
                {
                    var result = (TreeViewModel)selectItem.DataContext;

                    if (targetItem.NodeType == Enums.TreeNodeType.Children && result.NodeType == Enums.TreeNodeType.Root)
                    {
                    }
                    else if (targetItem.NodeType == Enums.TreeNodeType.Root && result.NodeType == Enums.TreeNodeType.Root && !targetItem.Equals(result))
                    {
                        vm.DeleteCommand.Execute((TreeViewModel)selectItem.DataContext);
                        result.Parent = targetItem;
                        targetItem.Children.Add(result);

                    }
                    else if (targetItem.NodeType == Enums.TreeNodeType.Root && result.NodeType == Enums.TreeNodeType.Root && targetItem.Equals(result))
                    {

                    }
                    else if (targetItem.Id == result.Parent.Id || targetItem.Equals(result))
                    {
                    }
                    else
                    {
                        vm.DeleteCommand.Execute((TreeViewModel)selectItem.DataContext);
                        result.Parent = targetItem;
                        targetItem.Children.Add(result);

                    }
                }
            }

            e.Effects = DragDropEffects.Move;
            isDragging = false;
            Utility.GetParentObject<Grid>(e.OriginalSource as TextBlock).Background = Brushes.Transparent;
        }

        private void PreviewQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            adornerLayer.Update();
        }

        private static TreeViewItem GetDataFromTreeViewItem(TreeView source, Point point)
        {
            UIElement element = source.InputHitTest(point) as UIElement;
            if (element != null)
            {
                return Utility.GetParentObject<TreeViewItem>(element);
            }
            return null;
        }
    }
}
