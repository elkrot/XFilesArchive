using MahApps.Metro.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Tulpep.ActiveDirectoryObjectPicker;

namespace XFilesArchive.UI.View.Security
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : MetroWindow
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DirectoryObjectPickerDialog picker = new DirectoryObjectPickerDialog()
            {
                AllowedObjectTypes = ObjectTypes.All,
                DefaultObjectTypes = ObjectTypes.All,
                AllowedLocations = Locations.All,
                DefaultLocations = Locations.JoinedDomain,
                MultiSelect = true,
                ShowAdvancedView = true,
                AttributesToFetch = new List<string>() {"objectSid" }
            };

            if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DirectoryObject[] results = picker.SelectedObjects;
                if (results == null)
                {
                    return;
                }
                
                var sid = "";
                for (int i = 0; i <= results.Length - 1; i++)
                {
                    string downLevelName = "";
                    try
                    {
                        if (!string.IsNullOrEmpty(results[i].Upn))
                            downLevelName = NameTranslator.TranslateUpnToDownLevel(results[i].Upn);
                    }
                    catch (Exception ex)
                    {
                        downLevelName = string.Format("{0}: {1}", ex.GetType().Name, ex.Message);
                    }

                    for (int j = 0; j < results[i].FetchedAttributes.Length; j++)
                    {
                        object multivaluedAttribute = results[i].FetchedAttributes[j];
                        if (!(multivaluedAttribute is IEnumerable) || multivaluedAttribute is byte[] || multivaluedAttribute is string)
                            multivaluedAttribute = new[] { multivaluedAttribute };

                        foreach (object attribute in (IEnumerable)multivaluedAttribute)
                        {
                            if (attribute is byte[])
                            {
                                byte[] bytes = (byte[])attribute;
                                sid = BytesToString(bytes);
                            }
                        }
                    }
                }
                var zzz = sid;
            }
        }

        private string BytesToString(byte[] bytes)
        {
            try
            {
                Guid guid = new Guid(bytes);
                return guid.ToString("D");
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception)
            {
            }

            try
            {
                SecurityIdentifier sid = new SecurityIdentifier(bytes, 0);
                return sid.ToString();
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception)
            {
            }

            return "0x" + BitConverter.ToString(bytes).Replace('-', ' ');
        }

    }
}
