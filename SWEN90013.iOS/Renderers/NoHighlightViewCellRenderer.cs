using System;
using SWEN90013;
using SWEN90013.CustomComponents;
using SWEN90013.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NoHighlightViewCell), typeof(NoHighlightViewCellRenderer))]
namespace SWEN90013.iOS.Renderers
{
    public class NoHighlightViewCellRenderer : ViewCellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var cell = base.GetCell(item, reusableCell, tv);
            var view = item as NoHighlightViewCell;
            //prevent selection so item cant be selected - any onclick/ontapped button will still be executed
            cell.SelectionStyle = UITableViewCellSelectionStyle.None;

            return cell;
        }
    }
}
