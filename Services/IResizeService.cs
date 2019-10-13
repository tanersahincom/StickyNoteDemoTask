using System.Drawing;
using System.Windows.Forms;

namespace StickyNote.Services
{
    public interface IResizeService
    {
        string GetMousePosition(Point mouse, Form form);
    }
}