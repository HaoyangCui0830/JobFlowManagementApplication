using System;

using Xamarin.Forms;

namespace SWEN90013.Views
{
    public class TaskListPage : ContentPage
    {
        public TaskListPage()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
}

