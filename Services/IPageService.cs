using System.Windows.Forms;

namespace StickyNote.Services
{
    public interface IPageService
    {
        void CreatePage();

        void CreateMainPageIfNotExists();

        bool LoadPages();

        void SavePageLocationInfo(string text, string x, string y);

        void SavePageSizeInfo(string text, string h, string w);

        void SaveForegroundColorInfo(string text, ColorDialog colorDialog);

        void DeletePage(string text);

        void SavePenColor(FontDialog fd, string text);

        void SaveBackgroundInfo(string text, ColorDialog cd);
    }
}