using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace YANSMBWE;

public partial class ArcEditorTab : FileEditorTab
{
    public override string TabName => dataContext.FilePath ?? "new Arc File";
    public override bool IsActive { get; set; }

    ArcEditorTabDataContext dataContext;

    public ArcEditorTab(string? filePath = null)
    {
        dataContext = new ArcEditorTabDataContext(filePath);
        DataContext = dataContext;

        InitializeComponent();
    }

    public ArcEditorTab() : this(null) { }

    public override void Close()
    {

    }
}