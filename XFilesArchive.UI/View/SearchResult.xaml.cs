using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для SearchResult.xaml
    /// </summary>
    public partial class SearchResult : UserControl
    {
        public SearchResult()
        {
            InitializeComponent();
        }


        //#region OpenSearchResultArchiveEntity
        //public static readonly DependencyProperty OpenSearchResultArchiveEntityProperty =
        //    DependencyProperty.Register(
        //        "OpenSearchResultArchiveEntity",
        //        typeof(ICommand),
        //        typeof(SearchResult),
        //        new UIPropertyMetadata(null));
        //public ICommand OpenSearchResultArchiveEntity
        //{
        //    get { return (ICommand)GetValue(OpenSearchResultArchiveEntityProperty); }
        //    set { SetValue(OpenSearchResultArchiveEntityProperty, value); }
        //}
        //#endregion
    }
}
