using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFilesArchive.Search.Widget
{
    public interface ISearchWidget<T> where T : ISearchWidgetItem
    {
        List<T> Items { get; set; }
        void LoadWidget();
        void AddItem(T item);
        void RemoveItem(T item);
    }


    #region  Базовый класс Виджета
    public abstract class SearchWidget<T> : ISearchWidget<T> where T : ISearchWidgetItem
    {
        public List<T> Items { get; set; }

        #region Конструктор
        public SearchWidget()
        {
            Items = new List<T>();
            LoadWidget();
        }
        #endregion

        #region Загрузка Виджета
        public virtual void LoadWidget()
        {

        }
        #endregion

        #region Добавить Item
        public void AddItem(T item)
        {
            Items.Add(item);
        }
        #endregion

        #region Удалить Item
        public void RemoveItem(T item)
        {
            Items.Remove(item);
        }
        #endregion

        #region Событие после добавления Item 
        public event EventHandler<SearchWidgetEventArgs<T>> ItemAdded;

        protected virtual void OnItemAdded(T item)
        {
            if (ItemAdded != null)
                ItemAdded(this, new SearchWidgetEventArgs<T>() { Item = item });
            //VideoEncoded(this,EventArgs.Empty);
        }
        #endregion

        #region Событие после удаления Item 
        public event EventHandler<SearchWidgetEventArgs<T>> ItemDeleted;

        protected virtual void OnItemDeleted(T item)
        {
            if (ItemDeleted != null)
                ItemDeleted(this, new SearchWidgetEventArgs<T>() { Item = item });
        }
        #endregion

    }
    #endregion

    public class SearchWidgetEventArgs<T> : EventArgs where T : ISearchWidgetItem
    {
        public T Item;
    }
}
