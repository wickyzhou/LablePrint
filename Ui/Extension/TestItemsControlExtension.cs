using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Windows.Controls;

namespace Ui.Extension
{
    public class TestItemsControlExtension:ItemsControl
    {
        protected override void OnItemTemplateSelectorChanged(DataTemplateSelector oldItemTemplateSelector, DataTemplateSelector newItemTemplateSelector)
        {
            base.OnItemTemplateSelectorChanged(oldItemTemplateSelector, newItemTemplateSelector);
        }

    }
}
