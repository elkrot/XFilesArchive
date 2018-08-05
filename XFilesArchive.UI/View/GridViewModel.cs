using System.ComponentModel;

namespace XFilesArchive.UI.View
{

        public class GridViewModel
        {
            public GridViewModel()
            {
                FirstName = "Shai";
                LastName = "Vashdi";
            }

            [Category("Information")]
            [DisplayName("First Name")]
            [Description("This property uses a TextBox as the default editor.")]
            public string FirstName { get; set; }

            [Category("Information")]
            [DisplayName("Last Name")]
            [Description("This property uses a TextBox as the default editor.")]
            public string LastName { get; set; }
        }
}