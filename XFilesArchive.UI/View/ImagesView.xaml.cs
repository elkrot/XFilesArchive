using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XFilesArchive.UI.View
{
    /// <summary>
    /// Логика взаимодействия для ImagesView.xaml
    /// </summary>
    public partial class ImagesView : UserControl
    {
        public ImagesView()
        {
            InitializeComponent();
        }
        #region RemoveItem
        public static readonly DependencyProperty RemoveItemProperty =
            DependencyProperty.Register(
                "RemoveItem",
                typeof(ICommand),
                typeof(ImagesView),
                new UIPropertyMetadata(null));
        public ICommand RemoveItem
        {
            get { return (ICommand)GetValue(RemoveItemProperty); }
            set { SetValue(RemoveItemProperty, value); }
        }
        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (imageListView.SelectedValue != null)
            {
                string imagePath = ((XFilesArchive.UI.Wrapper.ImageWrapper)imageListView.SelectedValue).Model.ImagePath;
                if (!string.IsNullOrWhiteSpace(imagePath))
                {
                    Process.Start(imagePath);
                }
            }


        }
    }
}
