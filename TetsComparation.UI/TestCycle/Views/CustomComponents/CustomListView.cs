using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace TetsComparation.UI.TestCycle.Views.CustomComponents
{
    public class CustomListView: ListView
    {
        public CustomListView()
        {
            this.SelectionChanged += CustomListView_SelectionChanged;           

        }
        void CustomListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SelectedItemsList = this.SelectedItems as IList;
        }

        public IList SelectedItemsList
        {
            get { return (IList)GetValue(SelectedItemsListProperty); }
            set { SetValue(SelectedItemsListProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemsListProperty =
                DependencyProperty.Register("SelectedItemsList", typeof(IList), typeof(CustomListView), new PropertyMetadata(null));
    }
}
