using System.Threading.Tasks;

namespace XFilesArchive.UI.View.Services
{
    public interface IMessageDialogService
    {
        Task<MessageDialogResult> ShowOKCancelDialogAsync(string text, string title);
        Task ShowInfoDialogAsync(string text);
    }
}