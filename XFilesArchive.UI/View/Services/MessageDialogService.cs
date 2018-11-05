using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace XFilesArchive.UI.View.Services
{
    public class MessageDialogService : IMessageDialogService
    {
        private MetroWindow MetroWindow => (MetroWindow)App.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            //App.Current.MainWindow;

        public async Task<MessageDialogResult> ShowOKCancelDialogAsync(string text, string title)
        {
            
            var result = await MetroWindow.ShowMessageAsync(title, text, MessageDialogStyle.AffirmativeAndNegative);
            return result == MahApps.Metro.Controls.Dialogs.MessageDialogResult.Affirmative ? MessageDialogResult.OK :
                            MessageDialogResult.Cancel;
        }


        public async Task ShowInfoDialogAsync(string text)
        {
            await MetroWindow.ShowMessageAsync("Info",text);
        }

        public MessageDialogResult ShowOKCancelDialog(string title, string text)
        {
            var result = MetroWindow.ShowModalMessageExternal(title, text, MessageDialogStyle.AffirmativeAndNegative);
            return result == MahApps.Metro.Controls.Dialogs.MessageDialogResult.Affirmative ? MessageDialogResult.OK :
                            MessageDialogResult.Cancel;
        }
    }


    public enum MessageDialogResult
    {
        OK, Cancel
    }
}
