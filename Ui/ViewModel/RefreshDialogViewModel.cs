using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ui.Command;

namespace Ui.ViewModel
{
	public class RefreshDialogViewModel<T> : BaseViewModel where T : class
	{
		public RefreshDialogViewModel()
		{
			SaveCommand = new DelegateCommand(Save);
			ExitCommand = new DelegateCommand(Exit);
		}

		public Action<int, T,bool> CallBack { get; set; }

		private T entity;

		public T Entity
		{
			get { return entity; }
			set
			{
				entity = value;
				this.RaisePropertyChanged(nameof(Entity));
			}
		}

		private bool isChanged;

		public bool IsChanged
		{
			get { return isChanged; }
			set
			{
				isChanged = value;
				this.RaisePropertyChanged(nameof(IsChanged));
			}
		}


		public virtual void WithParam(T entity, Action<int, T, bool> callBack)
		{
			Entity = entity;
			CallBack = callBack;
		}

		public DelegateCommand SaveCommand { get; set; }
		public DelegateCommand ExitCommand { get; set; }


		public virtual void Save(object obj)
		{
			CallBack?.Invoke(1, Entity, IsChanged);
		}

		private void Exit(object obj)
		{
			CallBack?.Invoke(0, null, IsChanged);
		}
	}
}
