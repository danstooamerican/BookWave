using BookWave.Controls;
using System.Collections.Generic;

namespace BookWave.Desktop.Models.MenuNavigation
{
    public class PageItem
    {
        #region Properties

        private string mPagePath;
        public string PagePath
        {
            get
            {
                if (MenuButton != null)
                {
                    return MenuButton.Page;
                }

                return mPagePath;
            }
            private set { mPagePath = value; }
        }

        private string mPageTitle;
        public string PageTitle
        {
            get
            {
                if (MenuButton != null)
                {
                    return MenuButton.PageTitle;
                }

                return mPageTitle;
            }
            private set { mPageTitle = value; }
        }

        private string mTitleBarTemplate;
        public string TitleBarTemplate
        {
            get
            {
                if (MenuButton != null)
                {
                    return MenuButton.TitleBarTemplate;
                }

                return mTitleBarTemplate;
            }
            private set { mTitleBarTemplate = value; }
        }

        private MenuButton mMenuButton;
        public MenuButton MenuButton
        {
            get { return mMenuButton; }
            private set { mMenuButton = value; }
        }

        #endregion

        #region Constructors 

        public PageItem(MenuButton menuButton)
        {
            PagePath = menuButton.Page;
            PageTitle = menuButton.PageTitle;
            TitleBarTemplate = menuButton.TitleBarTemplate;
            MenuButton = menuButton;
        }

        public PageItem(string pagePath, string pageTitle, string titleBarTemplate)
        {
            PagePath = pagePath;
            PageTitle = pageTitle;
            TitleBarTemplate = titleBarTemplate;
            MenuButton = null;
        }

        #endregion

        #region Methods
        public override bool Equals(object o)
        {
            if (o == null)
            {
                return false;
            }

            if (o is PageItem pageItem)
            {
                if (MenuButton != null)
                {
                    return MenuButton.Equals(pageItem.MenuButton);
                }

                return pageItem.PagePath.Equals(PagePath)
                    && pageItem.PageTitle.Equals(PageTitle)
                    && pageItem.TitleBarTemplate.Equals(TitleBarTemplate);
            }

            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 1041303343;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PagePath);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PageTitle);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(TitleBarTemplate);
            hashCode = hashCode * -1521134295 + EqualityComparer<MenuButton>.Default.GetHashCode(MenuButton);

            return hashCode;
        }

        #endregion
    }
}
