// Decompiled with JetBrains decompiler
// Type: AsBuiltExplorer.clsListviewSorter
// Assembly: AsBuiltExplorer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9083D66F-6E27-42C7-99A4-392C98AEFBC8
// Assembly location: I:\GITHUB\Projects\AsBuiltExplorer\AsBuiltExplorer.exe

using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Windows.Forms;

#nullable disable
namespace AsBuiltExplorer;

public class clsListviewSorter : IComparer
{
  private int m_ColumnNumber;
  private SortOrder m_SortOrder;

  public clsListviewSorter(int column_number, SortOrder sort_order)
  {
    this.m_ColumnNumber = column_number;
    this.m_SortOrder = sort_order;
  }

  public int Compare(object x, object y)
  {
    ListViewItem listViewItem1 = (ListViewItem) x;
    ListViewItem listViewItem2 = (ListViewItem) y;
    string text1 = listViewItem1.SubItems.Count > this.m_ColumnNumber ? listViewItem1.SubItems[this.m_ColumnNumber].Text : "";
    string text2 = listViewItem2.SubItems.Count > this.m_ColumnNumber ? listViewItem2.SubItems[this.m_ColumnNumber].Text : "";
    if (this.m_SortOrder == SortOrder.Ascending)
    {
      if (Versioned.IsNumeric((object) text1) & Versioned.IsNumeric((object) text2))
        return Conversion.Val(text1).CompareTo(Conversion.Val(text2));
      return Information.IsDate((object) text1) & Information.IsDate((object) text2) ? DateTime.Parse(text1).CompareTo(DateTime.Parse(text2)) : string.Compare(text1, text2);
    }
    if (Versioned.IsNumeric((object) text1) & Versioned.IsNumeric((object) text2))
      return Conversion.Val(text2).CompareTo(Conversion.Val(text1));
    return Information.IsDate((object) text1) & Information.IsDate((object) text2) ? DateTime.Parse(text2).CompareTo(DateTime.Parse(text1)) : string.Compare(text2, text1);
  }
}
