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
using System.Windows.Input;
using System.Windows.Media;

namespace OptimizedTreeView.Common
{
    public class DragDropBehavior : Behavior<UIElement>
    {
        protected override void OnAttached()
        {
            AssociatedObject.PreviewMouseLeftButtonDown += PreviewMouseLeftButtonDown;
            AssociatedObject.MouseMove += MouseMove;
            AssociatedObject.Drop += TreeView_Drop;
        }

        Point startPoint;
        bool isDragging = false;
        TreeViewItem selectItem;
        bool flag;

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
                if (sender is TreeView)
                {
                    if (Math.Abs(point.X - startPoint.X) > System.Windows.SystemParameters.MinimumHorizontalDragDistance || Math.Abs(point.Y - startPoint.Y) > System.Windows.SystemParameters.MinimumVerticalDragDistance)
                    {
                        TreeView dragSource = (TreeView)sender;
                        selectItem = GetDataFromListBox(dragSource, e.GetPosition(AssociatedObject));
                        if (selectItem != null)
                        {
                            isDragging = true;
                            DragDrop.DoDragDrop(dragSource, selectItem, DragDropEffects.Move);

                        }
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
                        isDragging = false;
                        e.Effects = DragDropEffects.Move;
                        return;
                    }
                    else if (targetItem.NodeType == Enums.TreeNodeType.Root && result.NodeType == Enums.TreeNodeType.Root)
                    {
                        vm.DeleteCommand.Execute((TreeViewModel)selectItem.DataContext);
                        result.Parent = targetItem;
                        targetItem.Children.Add(result);

                        e.Effects = DragDropEffects.Move;
                        isDragging = false;
                        return;
                    }
                    else if (targetItem.Id == result.Parent.Id || targetItem.Equals(result))
                    {
                        isDragging = false;
                        e.Effects = DragDropEffects.Move;
                        return;
                    }

                    vm.DeleteCommand.Execute((TreeViewModel)selectItem.DataContext);
                    result.Parent = targetItem;
                    targetItem.Children.Add(result);

                    e.Effects = DragDropEffects.Move;
                }
            }
            isDragging = false;
        }

        private static TreeViewItem GetDataFromListBox(TreeView source, Point point)
        {
            UIElement element = source.InputHitTest(point) as UIElement;
            if (element != null)
            {
                var tt =Utility.GetParentObject<TreeViewItem>(element);

                return tt;
            }
            return null;
        }
    }
}
