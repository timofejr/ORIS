namespace MiniTemplateEngine;

public class TextNode: Node
{
    public string Text { get; set; }

    public TextNode(string text)
    {
        Text = text;
    }
    
    public override string Render(object data)
    {
        return Text;
    }
}