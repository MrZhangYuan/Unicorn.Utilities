using System;
using System.Collections.Generic;
using System.Text;
using Unicorn.Utilities.ViewModel;

namespace Unicorn.Utilities.Model
{
    public abstract class ModelBase<T> : ObservableObject
    {
        private T _entity;
        public T Entity
        {
            get
            {
                return this._entity;
            }
            set
            {
                _entity = value;
                this.NotifyEntityChanged();
            }
        }

        public virtual void NotifyEntityChanged()
        {
            this.RaisePropertyChanged("Entity");
        }
    }
}
