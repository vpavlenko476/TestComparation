using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace TetsComparation.UI.TestCycle.Views.Bahaviors
{
    public class MultiSelectBehavior: Behavior<ListView>
    {
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register(
            "SelectedItems",
            typeof(ObservableCollection<string>),
            typeof(MultiSelectBehavior),
            new PropertyMetadata(new ObservableCollection<string>(), PropertyChangedCallback));
        public ObservableCollection<string> SelectedItems
        {
            get { return (ObservableCollection<string>)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectionChanged += OnSelectionChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.SelectionChanged -= OnSelectionChanged;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {  
            foreach (var item in e.RemovedItems)
            {
                if (SelectedItems.Contains(item.ToString()))
                {
                    SelectedItems.Remove(item.ToString());
                }
            }

            foreach (var item in e.AddedItems)
            {
                if (!SelectedItems.Contains(item.ToString()))
                {
                    SelectedItems.Add(item.ToString());
                }
            }           
        }

        private static void PropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            NotifyCollectionChangedEventHandler handler = (s, e) => SelectedItemsChanged(sender, e);
            if (args.OldValue is ObservableCollection<string>)
            {
                (args.OldValue as ObservableCollection<string>).CollectionChanged -= handler;
            }

            if (args.NewValue is ObservableCollection<string>)
            {
                (args.NewValue as ObservableCollection<string>).CollectionChanged += handler;
            }
        }

        private static void SelectedItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender is MultiSelectBehavior)
            {
                var listViewBase = (sender as MultiSelectBehavior).AssociatedObject;

                var listSelectedItems = listViewBase.SelectedItems;
                if (e.OldItems != null)
                {
                    foreach (var item in e.OldItems)
                    {
                        if (listSelectedItems.Contains(item))
                        {
                            listSelectedItems.Remove(item);
                        }
                    }
                }

                if (e.NewItems != null)
                {
                    foreach (var item in e.NewItems)
                    {
                        if (!listSelectedItems.Contains(item))
                        {
                            listSelectedItems.Add(item);
                        }
                    }
                }
            }
        }
    }
}
