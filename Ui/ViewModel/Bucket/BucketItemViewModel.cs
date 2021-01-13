using Model;

namespace Ui.ViewModel.Bucket
{
    public class BucketItemViewModel: BaseViewModel
    {
        public BucketModel BucketItem { get; set; }

        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                this.RaisePropertyChanged(nameof(IsSelected));
            }
        }

    }
}
