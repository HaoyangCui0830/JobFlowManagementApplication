using System;
using System.Runtime.Remoting.Contexts;
using SWEN90013;
using SWEN90013.CustomComponents;
using SWEN90013.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(StyledSearchBar), typeof(StyledSearchBarRenderer))]
namespace SWEN90013.iOS.Renderers
{
    public class StyledSearchBarRenderer : SearchBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);

            var searchbar = (UISearchBar)this.Control;
            if (e.NewElement != null)
            {
                //add inner border and corner radius
                Foundation.NSString _searchField = new Foundation.NSString("searchField");
                var textFieldInsideSearchBar = (UITextField)searchbar.ValueForKey(_searchField);
                textFieldInsideSearchBar.Layer.CornerRadius = 10;
                textFieldInsideSearchBar.Layer.BorderWidth = 2;
                //light grey colour for the border
                textFieldInsideSearchBar.Layer.BorderColor = UIColor.FromRGB(200, 200, 200).CGColor;
            }
        }
    }
}
