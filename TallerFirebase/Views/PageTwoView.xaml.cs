using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TallerFirebase.ViewModels;

namespace TallerFirebase.Views;

public partial class PageTwoView : ContentPage
{
    public PageTwoView(PageTwoViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}