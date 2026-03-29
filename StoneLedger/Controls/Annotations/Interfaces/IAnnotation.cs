using System;
using System.Collections.Generic;
using System.Text;

namespace StoneLedger.Controls.Annotations.Interfaces
{
    public interface IAnnotation
    {
        int X { get; }
        int Y { get; }

        void Draw(ICanvas canvas, RectF boardRect, float padding, float cellSize);
    }


}
