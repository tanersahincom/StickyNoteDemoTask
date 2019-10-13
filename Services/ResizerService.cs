using System.Drawing;
using System.Windows.Forms;

namespace StickyNote.Services
{
    internal class ResizeService : IResizeService
    {
        private bool _above, _right, _under, _left, _rightAbove, _rightUnder, _leftUnder, _leftAbove;

        //Thickness of border
        private readonly int _thickness;

        //Thickness of Angle border
        private readonly int _area = 8;

        public ResizeService() => _thickness = 10;

        //Get Mouse Position
        public string GetMousePosition(Point mouse, Form form)
        {
            var aboveUnderArea = mouse.X > _area && mouse.X < form.ClientRectangle.Width - _area;
            var rightLeftArea = mouse.Y > _area && mouse.Y < form.ClientRectangle.Height - _area;

            var above = mouse.Y <= _thickness;  //Mouse in Above All Area
            var right = mouse.X >= form.ClientRectangle.Width - _thickness;
            var under = mouse.Y >= form.ClientRectangle.Height - _thickness;
            var left = mouse.X <= _thickness;

            _above = above && (aboveUnderArea); if (_above) return "a";   /*Mouse in Above All Area WithOut Angle Area */
            _right = right && (rightLeftArea); if (_right) return "r";
            _under = under && (aboveUnderArea); if (_under) return "u";
            _left = left && (rightLeftArea); if (_left) return "l";

            _rightAbove =/*Right*/ (right && (!rightLeftArea)) && /*Above*/ (above && (!aboveUnderArea)); if (_rightAbove) return "ra";
            _rightUnder =/* Right*/((right) && (!rightLeftArea)) && /*Under*/(under && (!aboveUnderArea)); if (_rightUnder) return "ru";
            _leftUnder = /*Left*/((left) && (!rightLeftArea)) && /*Under*/ (under && (!aboveUnderArea)); if (_leftUnder) return "lu";
            _leftAbove = /*Left*/((left) && (!rightLeftArea)) && /*Above*/(above && (!aboveUnderArea)); if (_leftAbove) return "la";

            return "";
        }
    }
}