﻿using CodeNav.Models;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace CodeNav.Helpers
{
    public static class HistoryHelper
    {
        private const int MaxHistoryItems = 5;

        public static void AddItemToHistory(CodeDocumentViewModel model, Span span)
        {
            var item = FindCodeItem(model.CodeDocument, span);
            AddItemToHistory(item);
        }

        public static void AddItemToHistory(CodeItem item)
        {
            if (item == null) return;

            var model = item.Control.CodeDocumentViewModel;

            // Clear current indicators
            model.HistoryItems.ForEach(i => i.StatusMonikerVisibility = Visibility.Collapsed);

            // Add new indicator, only keep the five latest history items
            model.HistoryItems.RemoveAll(i => i.Id.Equals(item.Id));
            model.HistoryItems.Insert(0, item);
            model.HistoryItems = model.HistoryItems.Take(MaxHistoryItems).ToList();

            SolutionStorageHelper.SaveToSolutionStorage(item.Control, model);
            ApplyHistoryIndicator(model);
        }

        public static void ApplyHistoryIndicator(CodeDocumentViewModel model)
        {
            for (int i = 0; i < model.HistoryItems.Count; i++)
            {
                CodeItem historyItem = model.HistoryItems[i];
                var codeItem = model.CodeDocument
                    .Flatten()
                    .FirstOrDefault(item => item.Id.Equals(historyItem.Id));
                if (codeItem == null) continue;
                ApplyHistoryIndicator(codeItem, i);
            }
        }

        private static void ApplyHistoryIndicator(CodeItem item, int index = 0)
        {
            item.StatusMoniker = KnownMonikers.History;
            item.StatusMonikerVisibility = Visibility.Visible;
            item.StatusGrayscale = index > 0;
            item.StatusOpacity = GetOpacity(index);
        }

        private static double GetOpacity(int index)
        {
            switch (index)
            {
                case 0:
                    return 1;
                case 1:
                case 2:
                    return 0.9;
                case 3:
                case 4:
                    return 0.7;
                default:
                    return 1;
            }
        }

        public static void ClearHistory(CodeItem item)
        {
            item.Control.CodeDocumentViewModel.HistoryItems.Clear();
            SolutionStorageHelper.SaveToSolutionStorage(item.Control, item.Control.CodeDocumentViewModel);
            item.Control.UpdateDocument(true);
        }

        private static CodeItem FindCodeItem(IEnumerable<CodeItem> items, Span span)
        {
            foreach (var item in items)
            {
                if (item.Span.Contains(span.Start) && !(item is IMembers))
                {
                    return item;
                }

                if (item is IMembers)
                {
                    return FindCodeItem(((IMembers)item).Members, span);
                }
            }

            return null;
        }
    }
}
