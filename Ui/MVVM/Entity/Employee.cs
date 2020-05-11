using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ui.MVVM.ViewModel;

namespace Ui.MVVM.Entity
{
    public class Employee : ViewModelBase
    {
        private string _name;
        private string _sex;
        private int _age;

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string Sex
        {
            get => _sex;
            set
            {
                _sex = value;
                OnPropertyChanged(nameof(Sex));
            }
        }
        public int Age
        {
            get => _age;
            set
            {
                _age = value;
                OnPropertyChanged(nameof(Age));
            }
        }
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

    }
}
