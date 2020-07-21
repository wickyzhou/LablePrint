using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ExportRadioButtonModel:NotificationObject
    {
        private bool radioButton1;

        public bool RadioButton1
        {
            get { return radioButton1; }
            set
            {
                radioButton1 = value;
                this.RaisePropertyChanged(nameof(RadioButton1));
            }
        }


        private bool radioButton2;

        public bool RadioButton2
        {
            get { return radioButton2; }
            set
            {
                radioButton2 = value;
                this.RaisePropertyChanged(nameof(RadioButton2));
            }
        }

        private bool radioButton3;

        public bool RadioButton3
        {
            get { return radioButton3; }
            set
            {
                radioButton3 = value;
                this.RaisePropertyChanged(nameof(RadioButton3));
            }
        }

        private bool radioButton4;

        public bool RadioButton4
        {
            get { return radioButton4; }
            set
            {
                radioButton4 = value;
                this.RaisePropertyChanged(nameof(RadioButton4));
            }
        }

        private bool radioButton5;

        public bool RadioButton5
        {
            get { return radioButton5; }
            set
            {
                radioButton5 = value;
                this.RaisePropertyChanged(nameof(RadioButton5));
            }
        }

    }
}
