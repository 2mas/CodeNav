﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;
using Caliburn.Micro;
using CodeNav.Helpers;
using CodeNav.Properties;

namespace CodeNav.Models
{
    [DataContract]
    public class CodeDocumentViewModel : PropertyChangedBase
    {
        public CodeDocumentViewModel()
        {
            _codeDocument = new List<CodeItem>();
            Bookmarks = new Dictionary<string, int>();
            HistoryItems = new List<CodeItem>();
        }

        private List<CodeItem> _codeDocument;
        public List<CodeItem> CodeDocument {
            get
            {
                return _codeDocument;
            }
            set
            {
                _codeDocument = value;
                NotifyOfPropertyChange();
            }
        }

        public bool ShowFilterToolbar => Settings.Default.ShowFilterToolbar;

        public Visibility ShowFilterToolbarVisibility => Settings.Default.ShowFilterToolbar
            ? Visibility.Visible
            : Visibility.Collapsed;

        public SortOrderEnum SortOrder;

        public Visibility BookmarksAvailable
        {
            get
            {
                return Bookmarks.Any() ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public void AddBookmark(string id, int bookmarkStyleIndex)
        {
            if (Bookmarks.ContainsKey(id))
            {
                Bookmarks.Remove(id);
            }

            Bookmarks.Add(id, bookmarkStyleIndex);

            NotifyOfPropertyChange("BookmarksAvailable");
        }

        public void RemoveBookmark(string id)
        {
            Bookmarks.Remove(id);

            NotifyOfPropertyChange("BookmarksAvailable");
        }

        public void ClearBookmarks()
        {
            BookmarkHelper.ClearBookmarks(this);

            NotifyOfPropertyChange("BookmarksAvailable");
        }

        public Visibility ClearFilterVisibility =>
            string.IsNullOrEmpty(FilterText) ?
            Visibility.Collapsed : Visibility.Visible;

        private string _filterText;
        public string FilterText
        {
            get
            {
                return _filterText;
            }
            set
            {
                _filterText = value;
                NotifyOfPropertyChange("ClearFilterVisibility");
            }
        }

        private Dictionary<string, int> _bookmarks;
        [DataMember]
        public Dictionary<string, int> Bookmarks
        {
            get
            {
                return _bookmarks;
            }
            set
            {
                _bookmarks = value;
                NotifyOfPropertyChange("BookmarksAvailable");
            }
        }

        public bool FilterOnBookmarks;

        [DataMember]
        public string FilePath;

        [DataMember]
        public List<BookmarkStyle> BookmarkStyles;

        [DataMember]
        public List<CodeItem> HistoryItems;
    }
}
