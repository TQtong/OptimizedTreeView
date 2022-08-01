using OptimizedTreeView.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizedTreeView.Common
{
    public static class TreeViewCommon
    {
        /// <summary>
        /// 非递归广度优先遍历树
        /// </summary>
        /// <param name="item"></param>
        /// <param name="action"></param>
        public static void BFSTraverseTree(this TreeViewModel item, Action<TreeViewModel> action)
        {
            Queue<TreeViewModel> queue = new Queue<TreeViewModel>();
            queue.Enqueue(item);
            while (queue.Count > 0)
            {
                var curItem = queue.Dequeue();
                action(curItem);
                foreach (var subItem in curItem.Children)
                {
                    queue.Enqueue(subItem);
                }
            }
        }
    }
}
