using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace OptimizedTreeView.Common
{
    public class DragDropAdorner : Adorner
    {
        private readonly FrameworkElement adornedElement;

        public DragDropAdorner(UIElement adornedElement) : base(adornedElement)
        {
            IsHitTestVisible = false;
            this.adornedElement = adornedElement as FrameworkElement;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            Win32.POINT screenPos = new Win32.POINT();
            if (Win32.GetCursorPos(ref screenPos))
            {
                Point pos = PointFromScreen(new Point(screenPos.X, screenPos.Y));
                Rect rect = new Rect(pos.X, pos.Y, adornedElement.ActualWidth, adornedElement.ActualHeight);
                drawingContext.PushOpacity(1.0);
                Brush highlight = adornedElement.TryFindResource(SystemColors.HighlightBrushKey) as Brush;
                if (highlight != null)
                    drawingContext.DrawRectangle(highlight, new Pen(Brushes.Red, 0), rect);
                drawingContext.DrawRectangle(new VisualBrush(adornedElement),
                    new Pen(Brushes.Transparent, 0), rect);
                drawingContext.Pop();
            }
        }

    }
}
