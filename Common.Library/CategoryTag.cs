using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Library
{
    public class CategoryTag
    {
        #region Class Static

        public static readonly char DELIMITER = ':';
        public static readonly char[] SPLITTERS = new char[] { ';', ',', '\t', '\r', '\n' }; // Caution: the content are not readonly
        public static readonly string CategoryOthers = "Others";
        public static readonly string CategoryRoot = "*";

        public static void AddCatetories(string tags, CategoryTag root)
        {
            if (String.IsNullOrEmpty(tags) || root == null) return;

            var parentCategory = root;

            while (!String.IsNullOrEmpty(tags))
            {
                var firstTag = CategoryTag.GetFirstCategory(tags);
                var hasSubCategoryVm = false;

                if (firstTag == parentCategory.Name)
                {
                    tags = CategoryTag.GetSubTags(tags);
                    continue;
                }

                CategoryTag category = null;

                foreach(var sub in parentCategory.SubCategories)
                {
                    if (sub.Name == firstTag)
                    {
                        category = sub;
                        hasSubCategoryVm = true;
                        break;
                    }
                }
                if (hasSubCategoryVm == false)
                {
                    category = new CategoryTag(firstTag, parentCategory);
                    parentCategory.SubCategories.Add(category);
                }

                tags = CategoryTag.GetSubTags(tags);
                parentCategory = category;
            }
        }

        /// <summary>
        /// Check if a single category tag (e.g. "a:a1") can be found in a tags (e.g. "a:a1:a1a;b:b2:b2a;c;d:d1")
        /// </summary>
        /// <param name="tag">a single category tag</param>
        /// <param name="tags">a group of category tags delimted by ';'</param>
        /// <returns></returns>
        public static bool CheckCategoryInTags(string tag, string tags)
        {
            if (String.IsNullOrEmpty(tag)) return true;
            if (String.IsNullOrEmpty(tags)) return false;
            if (tag.Length > tags.Length) return false;

            var pos = tags.IndexOf(tag);
            var end = pos + tag.Length; // end > 0
            var chx = end == tags.Length ? ';' : (end < tags.Length ? tags[end] : ' ');

            return pos >= 0 && (chx == ';' || chx == ':');
        }

        public static string GetFirstCategory(string tag)
        {
            if (String.IsNullOrWhiteSpace(tag)) return String.Empty;

            int idx = tag.IndexOf(DELIMITER);
            int len = tag.Length;

            return (idx >= 0 ? tag.Substring(0, idx) : tag).Trim(SPLITTERS);
        }

        public static string GetLastCategory(string tag)
        {
            if (String.IsNullOrWhiteSpace(tag)) return String.Empty;

            tag = tag.Trim(SPLITTERS);

            int idx = tag.LastIndexOf(DELIMITER) + 1;
            int len = tag.Length;

            return (idx > 0 ? (idx < len ? tag.Substring(idx) : String.Empty) : tag).Trim(SPLITTERS);
        }

        public static string GetParentCategory(string tag)
        {
            if (String.IsNullOrWhiteSpace(tag)) return String.Empty;

            int len = tag.LastIndexOf(DELIMITER);

            return (len > 0 ? tag.Substring(0, len).Trim(SPLITTERS) : String.Empty);
        }

        public static string GetSubTags(string tag)
        {
            if (String.IsNullOrWhiteSpace(tag)) return String.Empty;

            int idx = tag.IndexOf(DELIMITER) + 1;
            int len = tag.Length;

            return (idx > 0 ? (idx < len ? tag.Substring(idx).Trim(SPLITTERS) : String.Empty) : String.Empty);
        }

        #endregion

        public CategoryTag(string tag, CategoryTag parent = null)
        {
            if (String.IsNullOrWhiteSpace(tag) || String.IsNullOrEmpty(tag.Trim(SPLITTERS)))
            {
                tag = CategoryOthers;
            }
            _categoryName = tag.Trim(SPLITTERS);
            _name = GetLastCategory(tag);

            SubCategories = new List<CategoryTag>();
            Parent = parent;

            if (parent != null)
            {
                while (parent != null)
                {
                    Root = parent; parent = parent.Parent;
                }
            }
            else // trace up to the root
            {
                Root = null;
            }
        }

        #region Properties

        private string _categoryName = String.Empty; // category tag
        public string CategoryName
        {
            get { return _categoryName; }
        }

        public string FullCategoryName // full tag name of the category prefix with ResourceKeys.Contacts
        {
            get { return CategoryRoot + ":" + _categoryName; }
        }

        public bool IsRoot
        {
            get { return Parent == null; }
        }

        private string _name = String.Empty;
        public string Name
        {
            get { return _name; }
        }

        public CategoryTag Parent { get; private set; }

        public CategoryTag Root { get; private set; }

        public List<CategoryTag> SubCategories { get; private set; }

        #endregion

        public override string ToString()
        {
            return FullCategoryName;
        }

    }
}
