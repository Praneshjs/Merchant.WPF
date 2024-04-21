using System.Windows;
using System.Windows.Media;

namespace Merchant.Helper
{
    public static class ListViewHelper
    {
        // Helper method to find the ancestor of a specific type
        public static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                current = VisualTreeHelper.GetParent(current);
                if (current is T result)
                {
                    return result;
                }
            } while (current != null);
            return null;
        }

        // Helper method to find a child element with a specific name
        public static T FindChild<T>(DependencyObject parent, string childName) where T : FrameworkElement
        {
            var count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i) as FrameworkElement;
                if (child != null && child is T t && child.Name == childName)
                {
                    return t;
                }
                else
                {
                    var result = FindChild<T>(child, childName);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            return null;
        }
    }
}
