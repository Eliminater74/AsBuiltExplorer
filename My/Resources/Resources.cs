// Decompiled with JetBrains decompiler
// Type: AsBuiltExplorer.My.Resources.Resources
// Assembly: AsBuiltExplorer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9083D66F-6E27-42C7-99A4-392C98AEFBC8
// Assembly location: I:\GITHUB\Projects\AsBuiltExplorer\AsBuiltExplorer.exe

using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace AsBuiltExplorer.My.Resources;

[StandardModule]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
[HideModuleName]
internal sealed class Resources
{
  private static ResourceManager resourceMan;
  private static CultureInfo resourceCulture;

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static ResourceManager ResourceManager
  {
    get
    {
      if (object.ReferenceEquals((object) AsBuiltExplorer.My.Resources.Resources.resourceMan, (object) null))
        AsBuiltExplorer.My.Resources.Resources.resourceMan = new ResourceManager("AsBuiltExplorer.Resources", typeof (AsBuiltExplorer.My.Resources.Resources).Assembly);
      return AsBuiltExplorer.My.Resources.Resources.resourceMan;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static CultureInfo Culture
  {
    get => AsBuiltExplorer.My.Resources.Resources.resourceCulture;
    set => AsBuiltExplorer.My.Resources.Resources.resourceCulture = value;
  }
}
