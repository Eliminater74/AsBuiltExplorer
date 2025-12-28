using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Windows.Forms;

namespace AsBuiltExplorer
{
    public class clsListviewSorter : IComparer
    {
        int m_ColumnNumber;
        SortOrder m_SortOrder;

        public clsListviewSorter(int column_number, SortOrder sort_order)
        {
            m_ColumnNumber = column_number;
            m_SortOrder = sort_order;
        }

        public int Compare(object x, object y)
        {
            var listViewItem1 = (ListViewItem)x;
            var listViewItem2 = (ListViewItem)y;
            var text1 = listViewItem1.SubItems.Count > m_ColumnNumber ? listViewItem1.SubItems[m_ColumnNumber].Text : "";
            var text2 = listViewItem2.SubItems.Count > m_ColumnNumber ? listViewItem2.SubItems[m_ColumnNumber].Text : "";

            if (m_SortOrder == SortOrder.Ascending)
            {
                if (Versioned.IsNumeric((object)text1) & Versioned.IsNumeric((object)text2))
                    return Conversion.Val(text1).CompareTo(Conversion.Val(text2));

                return Information.IsDate((object)text1) & Information.IsDate((object)text2) ? DateTime.Parse(text1).CompareTo(DateTime.Parse(text2)) : string.Compare(text1, text2);
            }

            if (Versioned.IsNumeric((object)text1) & Versioned.IsNumeric((object)text2))
                return Conversion.Val(text2).CompareTo(Conversion.Val(text1));

            return Information.IsDate((object)text1) & Information.IsDate((object)text2) ? DateTime.Parse(text2).CompareTo(DateTime.Parse(text1)) : string.Compare(text2, text1);
        }
    }
}