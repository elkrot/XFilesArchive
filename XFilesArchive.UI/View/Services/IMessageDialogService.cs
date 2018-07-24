using System.Threading.Tasks;

namespace XFilesArchive.UI.View.Services
{
    public interface IMessageDialogService
    {
        Task<MessageDialogResult> ShowOKCancelDialogAsync(string text, string title);
        Task ShowInfoDialogAsync(string text);
        MessageDialogResult ShowOKCancelDialog(string v1, string v2);
    }
}