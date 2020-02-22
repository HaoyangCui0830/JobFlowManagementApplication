using System;
using System.Collections.Generic;
using SWEN90013.ViewModels;

using Xamarin.Forms;

namespace SWEN90013.Views
{
    public partial class ErrorPage : ContentPage
    {
        public ErrorPage(Page page)
        {
            InitializeComponent();
            var viewModel = new ErrorViewModel(page);
            BindingContext = viewModel;
        }
    }
}
