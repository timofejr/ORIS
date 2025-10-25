using System.Collections;
using System.Reflection;

namespace MiniTemplateEngine;

public class VariableNode: Node
{
    public string Expression { get; }

    public VariableNode(string expression)
    {
        Expression = expression.Trim();
    }
    
    public override string Render(object data)
    {
        return GetValueFromExpression(Expression, data).ToString() ?? string.Empty;
    }

    public static object GetValueFromExpression(string expression, object data)
    {
        var parts = expression.Split(".");
        var currentObject = data;

        foreach (var part in parts)
        {
            if (currentObject == null)
                return null;

            if (currentObject is IDictionary<string, object> dict)
            {
                if (dict.TryGetValue(part, out var value))
                {
                    currentObject = value;
                    continue;
                }
                return null;
            }

            var propertyInfo = currentObject.GetType().GetProperty(part, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo == null)
                return null;

            currentObject = propertyInfo.GetValue(currentObject);
        }
        
        return currentObject;
    }
}